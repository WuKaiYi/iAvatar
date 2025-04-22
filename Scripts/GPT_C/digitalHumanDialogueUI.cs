using RuntimeInspectorNamespace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class digitalHumanDialogueUI : MonoBehaviour
{
    public VideoControl videoControl;
    public StreamDataReceiver streamDataReceiver;
    public TMP_InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        videoControl.SetPlayBackPosition(0);
    }
    [ContextMenu("开始通话请求Dify")]
    public void StartConversationRequestDify()
    {
        streamDataReceiver.SendDify("hello");
        // 实现代码
    }
    /// <summary>
    /// 發起對話請求
    /// </summary>
    [ContextMenu("开始通话请求")]
    public void StartConversationRequest()
    {
        streamDataReceiver.Send(inputField.text);
        // 实现代码
    }
    public void StartConversationRequest(string s  )
    {
        streamDataReceiver.Send(  s);
        // 实现代码
    }
    public void StartConversationRequestDify(string s)
    {
        streamDataReceiver.SendDify( s);

        //streamDataReceiver.SendDify((string)ES3FormUser.Load("custom_name") + "\\n" + s);
        inputField.text = "";
        // 实现代码
    }


    // Update is called once per frame
    void Update()
    {

    }
}
