using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.Video;
using Defective.JSON;

public class TTSSender : MonoBehaviour
{
    public VideoClip VideoCapture, VideoCaptureLop;
    public VideoPlayer VideoPlayer;
    public string url = "http://tts.hkicai.com";
    [TextArea(30,50)]
    public string data = @"{
            'text': '你想要合成的文本内容',
            'text_lang': '语言代码',
            'ref_audio_path': '参考音频的路径',
            'aux_ref_audio_paths': ['额外的参考音频路径1', '额外的参考音频路径2'],
            'prompt_text': '提示文本内容',
            'prompt_lang': '提示文本的语言',
            'top_k': 5,
            'top_p': 1,
            'temperature': 1,
            'text_split_method': 'cut0',
            'batch_size': 1,
            'batch_threshold': 0.75,
            'split_bucket': true,
            'return_fragment': false,
            'speed_factor': 1.0,
            'streaming_mode': false,
            'seed': -1,
            'parallel_infer': true,
            'repetition_penalty': 1.35
        }";

    void Start()
    {
        //StartCoroutine(PostRequest());
    }
    public void Send(string text)
    {
        StartCoroutine(PostRequest(text, Text_lang));
    }
    public string Text_lang;

  public  IEnumerator PostRequest(string text,string text_lang)
    {
   

        // 创建一个WebRequest对象
        UnityWebRequest webRequest = new UnityWebRequest(url, "POST");

        JSONObject jSONObject = new JSONObject(data);
        jSONObject["text"].stringValue= ( text);
        jSONObject["text_lang"].stringValue = (text_lang);
        // 构造JSON数据
        string jsonData = jSONObject.ToString();

        // 设置请求头
        webRequest.SetRequestHeader("Content-Type", "application/json");

        // 将JSON数据转为字节并设置到请求体
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // 等待请求返回结果
        yield return webRequest.SendWebRequest();
     
        // 检查是否有错误发生
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogError("Response: " + webRequest.downloadHandler.text);
            Debug.LogError("Error: " + webRequest.error);
        }
        else
        {
            Debug.Log("Response: " + webRequest.downloadHandler.text);
            PlayAudioFromBytes(webRequest.downloadHandler.data);
            VideoPlayer.clip = VideoCapture;
            VideoPlayer.Play();
        }
    }
    public AudioSource audioSource;  // Attach AudioSource component through Unity Inspector

    public void PlayAudioFromBytes(byte[] audioBytes)
    {
        // Convert the byte array to an AudioClip
        AudioClip audioClip = WavUtility.ToAudioClip(audioBytes);

        if (audioClip == null)
        {
            Debug.LogError("Failed to convert byte array to AudioClip.");
            return;
        }

        // Play the audio
        audioSource.clip = audioClip;
        audioSource.Play();
        StartCoroutine(WaitForAudioEnd());
    }

    private IEnumerator WaitForAudioEnd()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        VideoPlayer.clip = VideoCaptureLop;
        VideoPlayer.Play();
    }


}
public static class WavUtility
{
    // Converts two bytes to one float in the range -1 to 1
    static float BytesToFloat(byte firstByte, byte secondByte)
    {
        // Convert two bytes to one short (little endian)
        short s = (short)((secondByte << 8) | firstByte);

        // Convert to range from -1 to (just below) 1
        return s / 32768.0F;
    }

    static int BytesToInt(byte[] bytes, int offset = 0)
    {
        int value = 0;
        for (int i = 0; i < 4; i++)
        {
            value |= (int)bytes[offset + i] << (i * 8);
        }
        return value;
    }

    public static AudioClip ToAudioClip(byte[] wavBytes)
    {
        // Determine if mono or stereo
        int channels = wavBytes[22];  // Usually 1 or 2

        // Sample rate (matches Unity's clip frequency)
        int sampleRate = BytesToInt(wavBytes, 24);

        int byteCount = wavBytes.Length - 44;  // 44 byte header
        int sampleCount = byteCount / 2;       // 2 bytes per sample
        int dataSize = sampleCount / channels;

        float[] data = new float[dataSize];

        int resolution = 16;
        int dataOffset = 44;
        for (int i = 0; i < dataSize; i++)
        {
            int bytePos = dataOffset + (i * 2);
            data[i] = BytesToFloat(wavBytes[bytePos], wavBytes[bytePos + 1]);
        }

        AudioClip audioClip = AudioClip.Create("ClipFromBytes", dataSize, channels, sampleRate, false);
        audioClip.SetData(data, 0);

        return audioClip;
    }
}
