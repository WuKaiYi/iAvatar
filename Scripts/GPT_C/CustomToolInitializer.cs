using BestHTTP;
using System;
using UnityEngine;
using EasyBuildSystem.Features.Runtime.Buildings.Placer;
using static EasyBuildSystem.Features.Runtime.Buildings.Placer.BuildingPlacer;
using EasyBuildSystem.Features.Runtime.Buildings.Part;
using Vuplex.WebView;
using System.IO;
using DungeonArchitect;
using System.Collections;
using SickscoreGames.HUDNavigationSystem;
using System.Collections.Generic;

using Defective.JSON;
using BestHTTP.JSON;
using Room;
using IMBX;
using static IMBX.ImageLoader;

public class CustomToolInitializer : MonoBehaviour
{

    public static CustomToolInitializer Instance
    {
        get
        {

            return (CustomToolInitializer)FindObjectOfType(typeof(CustomToolInitializer));
        }
    }
    public JSONObject jd;


    public bool test = false;
    // 在此聲明變量、屬性或其他成員
    private void Awake()
    {


        //Web.SetCameraAndMicrophoneEnabled(true);
    }

    private void OnApplicationQuit()
    {
        SaveScence();
    }

    private void OnDestroy()
    {

    }
    public Dungeon Dungeon;
    public GameObject FPS;


    public BuildingPlacer buildingPlacer;
    //public HUDNavigationCanvas _HUDNavigationCanvas;
    //public HNSMapProfile _HNSMapProfile;
    public void Generate3DScene()
    {
        //if (ES3.KeyExists("CustomRoomMap", settings))
        //{
        //    downloadMap(ES3.Load<string>("CustomRoomMap", settings));
        //}
        //Dungeon.DestroyDungeon();
        StartCoroutine("UIDungeonBuild3D");
    }

    IEnumerator UIDungeonBuild3D()
    {
        yield return new WaitForSeconds(0.5f);
        //Dungeon.GetComponent<DungeonRuntimeNavigation>().enableRuntimeNavigation = true;
        //Dungeon.Build();

        FPS.SetActive(true);





    }
    string CustomRoomUrl = "";

    private void GenerateMiniMap(Texture2D texture)
    {
        // 将纹理转换为Sprite
        //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        // 填充到hNSMapProfile.MapTexture
        //_HNSMapProfile.MapTexture = sprite;
        //_HUDNavigationCanvas.SetMinimapProfile(_HNSMapProfile);
    }
    void downloadMap(string url)
    {
        //IMBX.ImageLoader imageLoader = IMBX.ImageLoader.Create();
        //imageLoader.Load(0, url, "", "", CacheMode.NoCache, (texture, index) =>
        //{
        //    if (texture != null)
        //    {
        //        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        //        // 填充到hNSMapProfile.MapTexture
        //        _HNSMapProfile.MapTexture = sprite;

        //        _HUDNavigationCanvas.InitMinimap(_HNSMapProfile);
        //    }
        //}, 0, 10);
    }
    ES3Settings settings;
    private void Start()
    {
        settings = new ES3Settings();

        settings.path = ES3FormUser.Load<string>("CustomSpace");
        Debug.Log(settings.FullPath + " ES3AutoSaveMgr.Current.settings.path ");
        //if (ES3.KeyExists("CustomRoomList", settings))
        //{
        //    ES3.Load("CustomRoomList", settings);

        //    Generate3DScene();
        //}
        //else
        //{//下載模板房間
        //    var request = new HTTPRequest(new Uri( ServerConfig.host+ "/Templates/test.txt"), HTTPMethods.Get, onFinished);
        //    request.Send();
        //    void onFinished(HTTPRequest originalRequest, HTTPResponse response)
        //    {
        //        Debug.Log(response.DataAsText);
        //        File.WriteAllBytes(Application.persistentDataPath + "/" + Path.GetFileName(originalRequest.Uri.ToString()), response.Data);

        //        var settingsT = new ES3Settings();

        //        ES3FormUser.Save<string>("CustomRoom", "test.txt");

        //        settingsT.path = "test.txt";
        //        Debug.Log(settingsT.FullPath + " ES3AutoSaveMgr.Current.settings.path ");
        //        if (ES3.KeyExists("CustomRoomList", settingsT))
        //        {
        //            ES3.Load("CustomRoomList", settingsT);
        //        }


        //        settingsT = new ES3Settings();
        //        settingsT.path = ES3FormUser.Load<string>("CustomSpace");
        //        ES3.Save("CustomRoomMap", ServerConfig.host + "/Templates/test.jpeg", settingsT);

        //        CustomToolInitializer.Instance.Generate3DScene();
        //    }
        //}


        if (ES3.KeyExists("CustomToolList", settings))
        {
            ES3.Load("CustomToolList", settings);
        }

        //Called when the build mode is changed.
        buildingPlacer.OnChangedBuildModeEvent.AddListener((BuildMode mode) => { Debug.Log(mode); });

        //Called when the Building Part selection is changed.
        buildingPlacer.OnChangedBuildingPartEvent.AddListener((BuildingPart part) =>
        {
            Debug.Log(part.gameObject.name);
            if (buildingPlacer.GetBuildMode == BuildMode.EDIT)
            {
                BuildingPart buildingPart = buildingPlacer.GetCurrentPreview;
                GameObject obj = new GameObject("BuildingEditor");
                BuildingEditor newBuildingEditor = obj.AddComponent<BuildingEditor>();
                newBuildingEditor.ImageUrl = buildingPart.gameObject.GetComponent<BuildingEditor>().ImageUrl;
                newBuildingEditor.WebLink = buildingPart.gameObject.GetComponent<BuildingEditor>().WebLink;
                newBuildingEditor.ModelLink = buildingPart.gameObject.GetComponent<BuildingEditor>().ModelLink;
            }

        });

    }
    public string Host;
    public void SaveScence()
    {
        Debug.Log("SaveScence");
        if (!ES3FormUser.KeyExists("CustomSpace"))
            ES3FormUser.Save<string>("CustomSpace", "CustomSpace.txt");

        var settings = new ES3Settings();
        settings.path = ES3FormUser.Load<string>("CustomSpace");
        Debug.Log(settings.FullPath + " ES3AutoSaveMgr.Current.settings.path ");

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("CustomRoom"))
        {
            if (!g.activeSelf)
            {
                Destroy(g);
            }

        }


        ES3.Save("CustomToolList", GameObject.FindGameObjectsWithTag("CustomTool"), settings);
        ES3.Save("CustomRoomList", GameObject.FindGameObjectsWithTag("CustomRoom"), settings);

        string fileName = Path.GetFileName(settings.FullPath);

        var request = new HTTPRequest(new Uri(ServerConfig.host + "/api/ES3_Space_Upload"), HTTPMethods.Post, onFinished);
        request.AddField("user_id", ES3.Load("user_id").ToString());
        request.AddField("Space_name", fileName);


        byte[] bytes = CaptureScreenshotWithoutUI().EncodeToJPG();
        request.AddBinaryData("pic_file", bytes, Path.ChangeExtension(fileName, ".jpeg"), "image/jpeg");

        request.AddBinaryData("file", File.ReadAllBytes(settings.FullPath), fileName, "text/plain");
        request.Send();
        Debug.Log("SaveScence_Send");

        void onFinished(HTTPRequest originalRequest, HTTPResponse response)
        {
            Debug.Log(response.DataAsText);

            bl_SceneLoaderManager.LoadScene("select_scene");
        }
    }

    public void BackScence()
    {
        SaveScence();


    }


    //unity編寫函數拍攝當前相機畫面,不包括UI 
    public Texture2D CaptureScreenshotWithoutUI()
    {
        // 获取当前相机的渲染纹理
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        Camera.main.targetTexture = renderTexture;
        Camera.main.Render();

        // 创建一个2D纹理，并将渲染纹理的像素数据复制到2D纹理中
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();

        // 重置相机的渲染目标和激活的渲染纹理
        Camera.main.targetTexture = null;
        RenderTexture.active = null;

        // 销毁渲染纹理
        RenderTexture.Destroy(renderTexture);

        return screenshotTexture;
    }


    private void Update()
    {
        // 在每一幀更新時執行的邏輯
    }

    // 其他自定義函數和事件處理函數等
}