using UnityEngine;
using Unity.Netcode;
using Unity.Multiplayer.Samples.Utilities;
using BestHTTP;
using System;
using UnityStandardAssets.Effects;
using Defective.JSON;
using System.IO;
using DungeonArchitect;
using Unity.Collections.LowLevel.Unsafe;
using SickscoreGames.HUDNavigationSystem;
using static IMBX.ImageLoader;

public class CustomSpaceLoader : MonoBehaviour
{
  public  GameObject FPS;
    public HUDNavigationCanvas _HUDNavigationCanvas;
    public HNSMapProfile _HNSMapProfile;
    private void Start()
    {
       
    }
    //NetworkSyncScript networkSyncScript ;


    public void LoadCustomSpace(int id)
    {
       
        var request = new HTTPRequest(new Uri(ServerConfig.host + "/api/ES3_File_Space_id/" + id), HTTPMethods.Get, onFinished);
        Debug.Log(request.Uri);
        request.Send();

        void onFinished(HTTPRequest originalRequest, HTTPResponse response)
        {
            Debug.Log(response.DataAsText);
            JSONObject jd = new JSONObject(response.DataAsText);

            var request2 = new HTTPRequest(new Uri(jd[0]["file_url"].stringValue), HTTPMethods.Get, onFinished2);
            request2.Send();
            void onFinished2(HTTPRequest originalRequest, HTTPResponse response)
            {
                Debug.Log(response.DataAsText);
                File.WriteAllBytes(Application.persistentDataPath + "/" + jd[0]["scencename"].stringValue, response.Data);
                BuildSpace(jd[0]["scencename"].stringValue);
              
            }

            //var request3 = new HTTPRequest(new Uri(jd[0]["pic_url"].stringValue), HTTPMethods.Get, onFinished3);
            //request3.Send();
            //void onFinished3(HTTPRequest originalRequest, HTTPResponse response)
            //{
            //    Debug.Log(response.DataAsText);
            //    File.WriteAllBytes(Application.persistentDataPath + "/" + jd[0]["scencename"].stringValue, response.Data);
            //}
        }

    }
    void downloadMap(string url)
    {
        IMBX.ImageLoader imageLoader = IMBX.ImageLoader.Create();
        imageLoader.Load(0, url, "", "", CacheMode.NoCache, (texture, index) =>
        {
            if (texture != null)
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

                // Ìî³äµ½hNSMapProfile.MapTexture
                _HNSMapProfile.MapTexture = sprite;

                _HUDNavigationCanvas.InitMinimap(_HNSMapProfile);

                //FPS.SetActive(true);
            }
        }, 0, 10);
    }
    ES3Settings settings;
    void BuildSpace(string path)
    {
        settings = new ES3Settings();

        settings.path = path;
        Debug.Log(settings.FullPath + " ES3AutoSaveMgr.Current.settings.path ");
        //if (ES3.KeyExists("CustomRoomList", settings))
        //{
        //    ES3.Load("CustomRoomList", settings);

        //    Generate3DScene();
        //}
        if (ES3.KeyExists("CustomToolList", settings))
        {
            ES3.Load("CustomToolList", settings);
        }

        //GameObject.FindFirstObjectByType<CustomObjectController>().InitializeData();

    }
    public Dungeon Dungeon;
    private void Generate3DScene()
    {
        Dungeon.GetComponent<DungeonRuntimeNavigation>().enableRuntimeNavigation = true;
        Dungeon.Build();
        if (ES3.KeyExists("CustomRoomMap", settings))
        {
            downloadMap(ES3.Load<string>("CustomRoomMap", settings));
        }
    }


}