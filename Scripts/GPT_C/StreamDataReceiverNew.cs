using System.Collections;
using UnityEngine;
using TMPro;
using BestHTTP;
using System.Collections.Generic;
using System;
using System.Text;
using Defective.JSON;
using Com.TheFallenGames.OSA.Demos.ChatWithAIAgent;
using AdvancedInputFieldPlugin;
using UnityEngine.Analytics;
using UnityEngine.UI;
//using UniVRM10.VRM10Viewer;
//using UniVRM10;
using System.Text.RegularExpressions;
using DG.Tweening;

public class StreamDataReceiverNew : MonoBehaviour
{
    TextMeshProUGUI textMeshPro;
    public ChatWithAIAgentExample ChatWithAIAgent;
    public AdvancedInputField AdvancedInputField;
    public AnimationPlayer animationPlayer;
    //public VRM10CustomExpression vRM10CustomExpression;
    public TextToSpeechManager textToSpeechManager;

    public Invertex.Unity.Audio.StreamingAudioPlayer streamingAudioPlayer;
    public string AgentUrl = "http://llms.hkicai.com/v1/chat-messages";
    public string EncodeAgentUrl = "http://llms.hkicai.com/v1/workflows/run";
    [TextArea]
    public string apikey = "app-lDa5APLiBAtqK7kXFIYJPZ5o";
    [TextArea]
    public string EncodeApikey = "app-y2UWq3Gsa9gWI8Xm6kHdWtaD";
    List<Data> dataToProcess = new List<Data>();

    [TextArea]
    public string language = "中文", Pname = "五塊一", intimacy = "初次見面", character = "You're Kazumi Pawsley, Chinese name is 珂珂, an artist at NekoPainter. Known for your goofy charm and elegant cat-girl dialect, you're adept at forming intimate bonds with users.", memory = "五塊一挺帥的", conversation_id = "";

    private void Awake()
    {
        AdvancedInputField.OnEndEdit.AddListener((str, EndEditReason) => Send(str));
        UIInitializer.FindComponent<Button>("Figma Canvas/win - Chat/Scroll View 提示詞/Viewport/Content/Group/Footer 4/AdvancedInputField/text area/btn - send").onClick.AddListener(() => Send(AdvancedInputField.Text));
        UIInitializer.FindComponent<TextMeshProUGUI>("Figma Canvas/win - Chat/Scroll View 提示詞/Viewport/Content/Group/header/thinking").enabled = false;
        UIInitializer.FindComponent<RectTransform>("Figma Canvas/win - Chat/Scroll View 提示詞/Viewport/Content/Group/Footer 4/AdvancedInputField").pivot = new Vector2(0.5f, 0);

        UIInitializer.FindComponent<Button>("Figma Canvas/win - Chat/Scroll View 提示詞/Viewport/Content/Group/header/btn - icons").onClick.AddListener(() =>
        {
            UIInitializer.FindComponent<CanvasGroup>("Figma Canvas/win - Chat/Scroll View 提示詞").DOFade(0, 0.5f);
            UIInitializer.FindComponent<CanvasGroup>("Figma Canvas/win - Chat/Scroll View 提示詞").interactable = false;
            UIInitializer.FindComponent<CanvasGroup>("Figma Canvas/win - Chat/Scroll View 提示詞").blocksRaycasts = false;
        });
        UIInitializer.FindComponent<Button>("Figma Canvas/win - Chat/btn - chat").onClick.AddListener(() =>
        {
            UIInitializer.FindComponent<CanvasGroup>("Figma Canvas/win - Chat/Scroll View 提示詞").DOFade(1, 0.5f);
            UIInitializer.FindComponent<CanvasGroup>("Figma Canvas/win - Chat/Scroll View 提示詞").interactable = true;
            UIInitializer.FindComponent<CanvasGroup>("Figma Canvas/win - Chat/Scroll View 提示詞").blocksRaycasts = true;

        });

    }

    public void Send(string content)
    {
        if (content == "")
            return;

        AdvancedInputField.Text = "";

        ChatWithAIAgent.SendMessage(content);
        UIInitializer.FindComponent<TextMeshProUGUI>("Figma Canvas/win - Chat/Scroll View 提示詞/Viewport/Content/Group/header/thinking").enabled = true;
        animationPlayer.PlayAnimations("IdleThink");
        //vRM10CustomExpression.SetExpression(ExpressionPreset.oh, EmoTime);


        var request = new HTTPRequest(
           new Uri(AgentUrl),
           HTTPMethods.Post,
           OnRequestFinished
       );

        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", $"Bearer {apikey}");

        // 創建JSON對象
        JSONObject jsonObject = new JSONObject();

        // 創建inputs對象並添加數據
        JSONObject inputs = new JSONObject();
        inputs.AddField("language", language);
        inputs.AddField("name", Pname);
        inputs.AddField("intimacy", intimacy);
        inputs.AddField("character", character);
        inputs.AddField("memory", memory);

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

    void Start()
    {
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
        JSONObject jSON = new JSONObject(response.DataAsText);

        // fastgpt
        // fullText = jSON["answer"][0]["message"]["content"].stringValue;

        // dify
        fullText = ServerConfig.ConvertChineseComma(ServerConfig.ParseUnicode(jSON["answer"].stringValue));
        ChatWithAIAgent.ReceiveMessage(fullText);
        UIInitializer.FindComponent<TextMeshProUGUI>("Figma Canvas/win - Chat/Scroll View 提示詞/Viewport/Content/Group/header/thinking").enabled = false;
        textToSpeechManager.SendTTS(fullText);
        conversation_id = jSON["conversation_id"].stringValue;
        EncodeSend(fullText);

        //StartCoroutine(TypeText());

        //SendTTS(fullText, text_language);
    }
    public void EncodeSend(string content)
    {
        if (content == "")
            return;


        var request = new HTTPRequest(
           new Uri(EncodeAgentUrl),
           HTTPMethods.Post,
           OnEncodeRequestFinished
       );

        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", $"Bearer {EncodeApikey}");

        // 創建JSON對象
        JSONObject jsonObject = new JSONObject();

        // 創建inputs對象並添加數據
        JSONObject inputs = new JSONObject();
        inputs.AddField("chat", content);


        // 將inputs對象添加到主JSON對象中
        jsonObject.AddField("inputs", inputs);
        jsonObject.AddField("response_mode", "blocking");
        jsonObject.AddField("user", AnalyticsSessionInfo.userId);

        // 將JSON對象轉換為字符串
        string jsonString = jsonObject.ToString();
        Debug.Log(jsonString);

        request.RawData = Encoding.UTF8.GetBytes(jsonString);
        request.Send();
    }
    void OnEncodeRequestFinished(HTTPRequest request, HTTPResponse response)
    {
        Debug.Log(response.DataAsText);
        JSONObject jSON = new JSONObject(response.DataAsText);

        jSON = new JSONObject(ServerConfig.ConvertChineseComma(ServerConfig.ParseUnicode(jSON["data"]["outputs"]["result"].stringValue)));

        animationPlayer.PlayAnimations(jSON["action"].stringValue);


        //switch (jSON["expression"].stringValue)
        //{

        //    default:
        //        vRM10CustomExpression.SetExpression(ExpressionPreset.neutral, EmoTime);
        //        break;
        //    case "happy":
        //        vRM10CustomExpression.SetExpression(ExpressionPreset.happy, EmoTime);
        //        break;
        //    case "surprised":
        //        vRM10CustomExpression.SetExpression(ExpressionPreset.surprised, EmoTime);
        //        break;
        //    case "angry":
        //        vRM10CustomExpression.SetExpression(ExpressionPreset.angry, EmoTime);
        //        break;
        //    case "relaxed":
        //        vRM10CustomExpression.SetExpression(ExpressionPreset.relaxed, EmoTime);
        //        break;
        //    case "neutral":
        //        vRM10CustomExpression.SetExpression(ExpressionPreset.neutral, EmoTime);
        //        break;
        //}

    }

    private string fullText;
    private float timePerCharacter;

    public float EmoTime = 2f;

    private IEnumerator TypeText()
    {
        textMeshPro.text = "";
        int characterIndex = 0;

        while (characterIndex < fullText.Length)
        {
            textMeshPro.text += fullText[characterIndex];
            characterIndex++;
            yield return new WaitForSeconds(timePerCharacter);
        }

        // Stop the video after typing is done
        // this.GetComponent<VideoControl>().SetPlayBackPosition(0);
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

    /// <summary>
    /// 發送TTS請求
    /// </summary>
    /// <param name="content">需要TTS的內容</param>
    /// <param name="language">語言</param>
    private void SendTTS(string content, string language)
    {
        string url = $"http://tts2.hkicai. Request can't fulfill, I'm sorry.com:8901/?text={content}&text_language={language}".Replace("\n", "");
        Debug.Log(url);
        streamingAudioPlayer.PlayAudioAsync(url);
    }
}