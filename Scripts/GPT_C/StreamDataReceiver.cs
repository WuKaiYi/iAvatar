using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Data;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using BestHTTP;
using RuntimeInspectorNamespace;
using System.Collections.Generic;
using System;
using System.Text;
using GLTFast.Schema;
using Defective.JSON;
using UnityEngine.Analytics;
using UnityEngine.Windows;

public class StreamDataReceiver : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public GameObject textMeshProParent;
    public Invertex.Unity.Audio.StreamingAudioPlayer streamingAudioPlayer;

    [TextArea]
    public string character = "you are Doctor SONG an Associate Professor in the Department of Mathematics and Information Technology, and Associate Director in Immersive Learning and Metaverse in Education at The Education University of Hong Kong.";
    [TextArea]
    public string language = "English";
    List<Data> dataToProcess = new List<Data>();
    public string AgentUrl = "http://llms.hkicai.com/v1/chat-messages";
    public string EncodeAgentUrl = "http://llms.hkicai.com/v1/workflows/run";
    [TextArea]
    public string apikey = "app-lDa5APLiBAtqK7kXFIYJPZ5o";
    [TextArea]
    public string EncodeApikey = "app-y2UWq3Gsa9gWI8Xm6kHdWtaD";


    [TextArea]
    public string Pname = "五塊一", intimacy = "初次見面", conversation_id = "";

    public void SendDify(string content)
    {
        if (content == "")
            return;

      

        var request = new HTTPRequest(
           new Uri(AgentUrl),
           HTTPMethods.Post,
           OnRequestFinishedDify
       );

        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", $"Bearer {apikey}");

        // 創建JSON對象
        JSONObject jsonObject = new JSONObject();

        // 創建inputs對象並添加數據
        JSONObject inputs = new JSONObject();
        //inputs.AddField("language", language);
        //inputs.AddField("name", Pname);
        //inputs.AddField("intimacy", intimacy);
        //inputs.AddField("character", character);
        //inputs.AddField("memory", memory);

        // 將inputs對象添加到主JSON對象中
        jsonObject.AddField("inputs", inputs);
        jsonObject.AddField("query", content);
        jsonObject.AddField("response_mode", "blocking");
        jsonObject.AddField("conversation_id", conversation_id);
        jsonObject.AddField("user", AnalyticsSessionInfo.userId);

        // 將JSON對象轉換為字符串
        string jsonString = jsonObject.ToString();
        Debug.Log(jsonString);

        request.RawData = Encoding.UTF8.GetBytes(jsonString);
        request.Send();
    }
    void OnRequestFinishedDify(HTTPRequest request, HTTPResponse response)
    {
        Debug.Log(response.DataAsText);
        //var totalDataReceived = TotalDataReceived();
        //HandleReceivedData(totalDataReceived);
        JSONObject jSON = new JSONObject(response.DataAsText);

        fullText = ServerConfig.ConvertChineseComma(ServerConfig.ParseUnicode(jSON["answer"].stringValue));
        conversation_id = jSON["conversation_id"].stringValue;

        //if (this.GetComponent<VideoControl>() != null)
        //{
        //    this.GetComponent<VideoControl>().videoPlayer.Play();
        //}

        StartCoroutine(TypeText());
        if (this.GetComponent<TTSSender>() != null)
        {
            this.GetComponent<TTSSender>().Send(fullText);
        }

    }

    public void Send(string content)
    {
        var request = new HTTPRequest(
           new Uri("http://nekopainter.hkicai.com:8852/api/v1/chat/completions"),
           HTTPMethods.Post,
           OnRequestFinished
       );



        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", apikey);
        var rawJsonString = "{\"chatId\": \"asdasdwasdc\", \"stream\": false, \"detail\": false,\"variables\": {\"language\": \"這裡替換語言\",\"character\": \"這裡替換角色\" },\"messages\": [ { \"content\": \"這裡需要替換\", \"role\": \"user\" } ] }".Replace("這裡需要替換", $"{content}").Replace("asdasdwasdc", sss).Replace("這裡替換角色", $"{character}").Replace("這裡替換語言", $"{language}");



        request.RawData = Encoding.UTF8.GetBytes(rawJsonString);
        Debug.Log(rawJsonString);
        request.Send();
        Debug.Log(" request.Send();");
    }
    string sss;
    void Start()
    {
        //character = (string)ES3FormUser.Load("custom_name");

        System.Random random = new System.Random();

        for (int i = 0; i < 8; i++)
        {
            sss += (char)random.Next('A', 'Z' + 1);
        }
        SendDifyStart();
    }
    public void SendDifyStart()
    {
       

        var request = new HTTPRequest(
           new Uri("https://llms.hkicai.com/v1/parameters"),
           HTTPMethods.Get,
           OnRequestFinishedDifyStart
       );

        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", $"Bearer {apikey}");

        request.Send();
    }
    void OnRequestFinishedDifyStart(HTTPRequest request, HTTPResponse response)
    {
        Debug.Log(response.DataAsText);
        //var totalDataReceived = TotalDataReceived();
        //HandleReceivedData(totalDataReceived);
        JSONObject jSON = new JSONObject(response.DataAsText);

        fullText = ServerConfig.ParseUnicode(jSON["opening_statement"].stringValue);
        StartCoroutine(TypeText());
    }
        bool OnDataDownloaded(HTTPRequest request, HTTPResponse response, byte[] dataFragment, int dataFragmentLength)
    {
        Debug.Log(Encoding.UTF8.GetString(dataFragment));
        Debug.Log(Encoding.UTF8.GetString(dataFragment).Substring(5) + "   " + DateTime.Now.ToString());
        JSONObject jSON = new JSONObject(Encoding.UTF8.GetString(dataFragment).Substring(5));
        textMeshPro.text += jSON["choices"][0]["delta"]["content"].stringValue;
        // dataFragment is saved to process it later
        dataToProcess.Add(new Data
        {
            buffer = dataFragment,
            length = dataFragmentLength
        });

        // the callback must return false, otherwise the plugin would reuse the byte[] overwriting the data in it
        return false;
    }

    public string text;
    public string text_language = "en";
    void OnRequestFinished(HTTPRequest request, HTTPResponse response)
    {
        Debug.Log(response.DataAsText);
        //var totalDataReceived = TotalDataReceived();
        //HandleReceivedData(totalDataReceived);
        JSONObject jSON = new JSONObject(response.DataAsText);

        fullText =ServerConfig.ParseUnicode( jSON["choices"][0]["message"]["content"].stringValue);
        StartCoroutine(TypeText());


        if (streamingAudioPlayer != null)
        {
            string s = $"http://tts2.hkicai.com:8901/?text=需要替換&text_language={text_language}".Replace("需要替換", fullText).Replace("\n", "");
            Debug.Log(s);
            streamingAudioPlayer.PlayAudioAsync(s);
        }


    }


    private string fullText;
    private float timePerCharacter;
    private IEnumerator TypeText()
    {
        if (textMeshProParent != null)
        {
            textMeshProParent.SetActive(true );
        }
        textMeshPro.text = "";
        int characterIndex = 0;

        while (characterIndex < fullText.Length)
        {
            textMeshPro.text += fullText[characterIndex];
            characterIndex++;
            yield return new WaitForSeconds(timePerCharacter);
        }

        // Stop the video after typing is done
        this.GetComponent<VideoControl>().SetPlayBackPosition(0);
    }
    string TotalDataReceived()
    {
        var allData = new List<byte>();
        foreach (var data in dataToProcess)
        {
            for (int i = 0; i < data.length; i++)
            {
                allData.Add(data.buffer[i]);
            }
        }
        return Encoding.UTF8.GetString(allData.ToArray());
    }
    /// <summary>
    /// 根據傳入音頻的長度將傳入的string按照音頻時間textMeshPro用打字機效果打出,並且在打字機效果執行完以後停止播放視頻
    /// </summary>




    /// <param name="data"></param>
    void HandleReceivedData(string data)
    {
        // Here you can handle your received data
        Debug.Log(data);
    }

    struct Data
    {
        public byte[] buffer;
        public int length;
    }
}
