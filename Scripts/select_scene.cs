using BestHTTP;
using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class select_scene : MonoBehaviour
{
    // Start is called before the first frame update
    public Text Create_error;
    public InputField Create_scence_name;
    public string Host;
    public void SetSence(string name)
    {
        ES3.Save<string>("StartScence", name);
        bl_SceneLoaderManager.LoadScene("Startup");
    }
    public void LoadSence(string name)
    {
        bl_SceneLoaderManager.LoadScene(name);
    }
    public void LoadSence_file(string file_name,string url,int id)
    {
        ES3FormUser.Save<int>("CustomToolScenceid", id);
        ES3FormUser.Save<string>("CustomToolScenceName", file_name);
        scence_name = file_name;
        Debug.Log(Application.persistentDataPath + "/" + ES3FormUser.Load<string>("CustomToolScenceName"));

       


    }

    public void Create_scence()
    {
        if (Create_scence_name.text == "")
        {
            Create_error.text = "error";
            return;
        }
        var request = new HTTPRequest(new Uri(ServerConfig .host+ "/api/ES3_File"), HTTPMethods.Post , onFinished);
        request.AddField("filename", Create_scence_name.text+".txt");
        request.AddField("user_id", ES3.Load("user_id").ToString());
        request.AddField("scencename", Create_scence_name.text);

        request.Send();
        void onFinished(HTTPRequest originalRequest, HTTPResponse response)
        {
            Debug.Log(response.DataAsText);

            JSONObject jd = new JSONObject(response.DataAsText);

            Create_error.text = ServerConfig.ParseUnicode(jd["message"].stringValue);
            if (jd["result"].stringValue == "success")
            {

                ES3FormUser.Save<int>("CustomToolScenceid", jd["file_id"].intValue);
                ES3FormUser.Save<string>("CustomToolScenceName", Create_scence_name.text + ".txt");
                Edit_Scence();
            }
        }


    }
    public void Create_Room(string s)
    {
        if (s == "")
            return;
        ES3FormUser.Save<string>("CustomRoom", s + ".txt");
        bl_SceneLoaderManager.LoadScene("DungeonLayout");
    }
    public void Create_Space(string s)
    {
        if (s == "")
            return;
        ES3FormUser.Save<string >("CustomSpace", s +".txt");
        bl_SceneLoaderManager.LoadScene("custon_tool_top_camera");
    }
    string scence_name;
    public void Edit_Scence()
    {
        bl_SceneLoaderManager.LoadScene("custon_tool_top_camera");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
