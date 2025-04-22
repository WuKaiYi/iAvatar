using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class TextToSpeechManager : MonoBehaviour
{
    public string[] segments; // 分段的文本
    public List<AudioClip> audioClips = new List<AudioClip>(); // 保存音频的列表
    public float delayBetweenClips = 0.5f; // 音频播放间隔时间
    public AudioSource audioSource;

    [ContextMenu("測試分段")]
    void TestProcessText()
    {
        string inputText = "(用毛茸茸的貓掌指了指你，害羞地說)\n喵喵～你好呀，五塊一！ ₍˄·͈༝·͈˄*₎◞ ̑̑ (眼睛閃閃發光)\n你...你今天看起來好帥呢！ (* >ω<)";
        segments = ProcessText(inputText).ToArray();

        StartCoroutine(ProcessSegments());
    }

    public void SendTTS(string str)
    {
        foreach (AudioClip clip in audioClips)
        {
            Destroy(clip);
        }
        audioClips.Clear();

        segments = ProcessText(str).ToArray();

        StartCoroutine(ProcessSegments());
    }

    List<string> ProcessText(string input)
    {
        // 去除括号及其内容
        string cleanedText = Regex.Replace(input, @"\([^)]*\)", "");

        // 去除特殊字符（保留“...”等）
        cleanedText = Regex.Replace(cleanedText, @"[^\w\s\u4e00-\u9fa5，。！？…]", "");

        // 分段
        string[] lines = cleanedText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        List<string> result = new List<string>();
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                result.Add(line.Trim());
            }
        }

        return result;
    }

    IEnumerator ProcessSegments()
    {
        foreach (string segment in segments)
        {
            yield return StartCoroutine(SendTextToSpeechRequest(segment));
        }
    }

    IEnumerator SendTextToSpeechRequest(string text)
    {
        string url = $"https://tts2.hkicai.com/?text={UnityWebRequest.EscapeURL(text)}&text_language=zh&cut_punc=,.";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            byte[] data = request.downloadHandler.data;
            string path = Path.Combine(Application.persistentDataPath, $"{text.GetHashCode()}.wav");
            File.WriteAllBytes(path, data);
            yield return StartCoroutine(LoadAndPlayAudioClip(path));
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }

    IEnumerator LoadAndPlayAudioClip(string path)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.WAV))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioClips.Add(clip);

                // 播放音频片段
                audioSource.clip = clip;
                audioSource.Play();
                yield return new WaitForSeconds(clip.length + delayBetweenClips);
            }
            else
            {
                Debug.LogError(www.error);
            }
        }
    }
}