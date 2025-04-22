using BestHTTP;
using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;

public class Es3LoadNet : NetworkBehaviour
{
    public NetworkVariable<int> m_SomeValue = new NetworkVariable<int>();
    public string Host;


    void DownLoadFile(int u_id)
    {
        var request = new HTTPRequest(new Uri(Host + "/api/ES3_File/id/" + u_id.ToString()), HTTPMethods.Get, onFinished);
        Debug.Log(request.Uri);
        request.Send();

        void onFinished(HTTPRequest originalRequest, HTTPResponse response)
        {
            Debug.Log(response.DataAsText);
            JSONObject jd = new JSONObject(response.DataAsText);
            if (jd["filename"] != null)
            {
                var request = new HTTPRequest(new Uri(jd["url"].stringValue), HTTPMethods.Get, onFinished);
                request.Send();
                void onFinished(HTTPRequest originalRequest, HTTPResponse response)
                {
                    Debug.Log(response.DataAsText);
                    File.WriteAllBytes(Application.persistentDataPath + "/" + jd["filename"].stringValue, response.Data);

                    ES3AutoSaveMgr.Current.settings.path = jd["filename"].stringValue;
                    // 在遊戲開始時執行的邏輯

                    Debug.Log(ES3AutoSaveMgr.Current.settings.FullPath + " ES3AutoSaveMgr.Current.settings.path ");
                    ES3AutoSaveMgr.Current.Load();
                }

        
            }

        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            ES3AutoSaveMgr.Current.settings.path = ES3FormUser.Load<string >("CustomToolScenceName");
            // 在遊戲開始時執行的邏輯

            Debug.Log(ES3AutoSaveMgr.Current.settings.FullPath + " ES3AutoSaveMgr.Current.settings.path ");
            ES3AutoSaveMgr.Current.Load();

            m_SomeValue.Value = ES3FormUser.Load<int>("CustomToolScenceid");
            DownLoadFile(m_SomeValue.Value);
            // NetworkManager.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        }
        else
        {

           
            m_SomeValue.OnValueChanged += OnSomeValueChanged;
            DownLoadFile(m_SomeValue.Value);
        }
    }

 

    private void OnSomeValueChanged(int previous, int current)
    {
        DownLoadFile(m_SomeValue.Value);

        Debug.Log($"Detected NetworkVariable Change: Previous: {previous} | Current: {current}");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
