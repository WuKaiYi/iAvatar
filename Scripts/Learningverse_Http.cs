using BestHTTP;
using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Learningverse_Http : MonoBehaviour
{
    public string Host = "";
    public TMPro.TMP_InputField username, password;

    public UnityEvent<string> Error;
    // Start is called before  the first frame update
    void Start()
    {
        HTTPManager.MaxConnectionIdleTime = TimeSpan.FromSeconds(120);
    }

    public void TestLogin()
    {
        JSONObject json = new JSONObject();
        json.AddField("username", "tom");
        json.AddField("password", "123456");
        var request = new HTTPRequest(new Uri(Host + "/api/login"), HTTPMethods.Post, callback: OnRequestFinished);

        request.SetHeader("Content-Type", "application/json; charset=UTF-8");
        request.RawData = System.Text.Encoding.UTF8.GetBytes(json.ToString());
        request.Send();
    }

    public void Login()
    {
        JSONObject json = new JSONObject();

        json.AddField("username", username.text);
        json.AddField("password", password.text);
        var request = new HTTPRequest(new Uri(Host+ "/api/login"), HTTPMethods.Post, callback: OnRequestFinished);

        request.SetHeader("Content-Type", "application/json; charset=UTF-8");
        request.RawData = System.Text.Encoding.UTF8.GetBytes(json.ToString());
        request.Send();

       
    }
    void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
    {

        switch (req.State)
        {
            // The request finished without any problem.
            case HTTPRequestStates.Finished:
                if (resp.IsSuccess)
                {
                    Error.Invoke("");
                    // Everything went as expected!
                    JSONObject js = new JSONObject(resp.DataAsText);
                    Debug.Log(js);
                    ES3.Save("user_id", js["id"].intValue);

                    ES3FormUser.Save("username", js["username"].stringValue);
                    ES3FormUser.Save("custom_name", js["custom_name"].stringValue);
                    ES3FormUser.Save("email", js["email"].stringValue);

                    if (js["avatar_url"].stringValue != null)
                    {
                        if (js["avatar_url"].stringValue.Substring(0, 3) == "int")
                        {
                            ES3FormUser.Save("AvatarUrl", js["avatar_url"].stringValue);
                        }
                        else
                        {
                            ES3FormUser.Save("AvatarUrl", js["avatar_url"].stringValue);
                            ES3FormUser.Save<string>("CustomizeAvatarUrl", js["avatar_url"].stringValue);
                        }
                    }
                    else
                    {

                    }


                    ES3FormUser.Save("log_data", js["custom_name"].stringValue + "-" + js["id"].intValue);
                    bl_SceneLoaderManager.LoadScene("profile");

                    /*                        ES3.Save("username", js["username"].stringValue, ES3.Load("user_id")+".es3");
                                            ES3.Save("custom_name", js["custom_name"].stringValue, ES3.Load("user_id") + ".es3");
                                            ES3.Save("email", js["email"].stringValue, ES3.Load("user_id") + ".es3");
                                            ES3.Save("avatar_url", js["custom_name"].stringValue, ES3.Load("user_id") + ".es3");*/

                    // 创建文件夹
                    string folderPath = Application.streamingAssetsPath + "/Logs/" + ES3FormUser.Load("log_data");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    Debug.Log(folderPath);
                }
                else
                {
                    Error.Invoke(resp.DataAsText);

                    Debug.LogWarning(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                    resp.StatusCode,
                                                    resp.Message,
                                                    resp.DataAsText));
                }
                break;

            // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
            case HTTPRequestStates.Error:
                Debug.LogError("Request Finished with Error! " + (req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
                break;

            // The request aborted, initiated by the user.
            case HTTPRequestStates.Aborted:
                Debug.LogWarning("Request Aborted!");
                break;

            // Connecting to the server is timed out.
            case HTTPRequestStates.ConnectionTimedOut:
                Debug.LogError("Connection Timed Out!");
                break;

            // The request didn't finished in the given time.
            case HTTPRequestStates.TimedOut:
                Debug.LogError("Processing the request Timed Out!");
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
