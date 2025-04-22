using BestHTTP;
using Defective.JSON;
using System;
using System.CodeDom.Compiler;
using UnityEngine;
using UnityEngine.UI;

public class CommentReply : MonoBehaviour
{
    public InputField commentInputField; // 評論輸入框
    public Text replyText; // 回復文本

    // 當按下回復按鈕時調用
    public void OnReplyButtonClicked()
    {
        JSONObject json = new JSONObject();
        json.AddField("user_id",""+ ES3.Load ("user_id"));
        json.AddField("content", commentInputField.text);

        var request = new HTTPRequest(new Uri(ServerConfig.host + $"/comment/comments/{Comment_id}/children"), HTTPMethods.Post, callback: OnRequestFinished);
        request.SetHeader("Content-Type", "application/json; charset=UTF-8");
        request.RawData = System.Text.Encoding.UTF8.GetBytes(json.ToString());  
        request.Send();
       this.GetComponent<Canvas>().enabled = false; 

       
    }
    int Comment_id;
    Outline Outline;
    public void SetComment(int id,string name,bool on,Outline outline)
    {
        commentInputField.text = null;
        if (Outline == null)
        {
            Outline = outline;
        }
        if (Outline != outline)
        {
            Outline.enabled = false;
            Outline = outline;
        }

        Comment_id = id;
        replyText.text =$"Reply to {name}: ";
        this.GetComponent<Canvas>().enabled = on;
    }
    void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
    {
        Debug.Log(resp.DataAsText);
      
    }
}