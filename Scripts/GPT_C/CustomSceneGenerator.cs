using UnityEngine;

using DungeonArchitect;
using System.Collections.Generic;
using DungeonArchitect.Graphs;
using System.Collections;
using SickscoreGames.HUDNavigationSystem;

using EasyBuildSystem.Features.Runtime.Buildings.Part;
using EasyBuildSystem.Features.Runtime.Buildings.Placer;
using EasyBuildSystem.Features.Runtime.Buildings.Manager;

using BestHTTP;
using System;
using System.IO;
using static EasyBuildSystem.Features.Runtime.Buildings.Placer.BuildingPlacer;

public class CustomSceneGenerator : MonoBehaviour
{
    public Dungeon Dungeon;
    public GameObject FPS;
    public GameObject MiniMapCamera;

    public List<Graph> dungeonThemes;
    public HUDNavigationCanvas _HUDNavigationCanvas;
    public HNSMapProfile _HNSMapProfile;

    // 生成场景
    public void Generate3DScene()
    {
        GenerateMiniMap();

        Dungeon.dungeonThemes[0] = dungeonThemes[1];
        Dungeon.DestroyDungeon();
        StartCoroutine("UIDungeonBuild3D");
    }

    public GameObject prefabCamera; // 預製體相機

    public void SaveRoom()
    {
        if (!ES3FormUser.KeyExists("CustomRoom"))
            ES3FormUser.Save<string>("CustomRoom", "CustomRoom.txt");


        var settings = new ES3Settings();
        settings.path = ES3FormUser.Load<string>("CustomRoom");
        Debug.Log(settings.FullPath + " ES3AutoSaveMgr.Current.settings.path ");

        ES3.Save("CustomRoomList", GameObject.FindGameObjectsWithTag("CustomRoom"), settings);





        string fileName = Path.GetFileName(settings.FullPath);


        var request = new HTTPRequest(new Uri(ServerConfig.host + "/api/ES3_Room_Upload"), HTTPMethods.Post, onFinished);
        request.AddField("user_id", ES3.Load("user_id").ToString());
        request.AddField("Room_name", fileName);
        request.AddBinaryData("file", File.ReadAllBytes(settings.FullPath), fileName, "text/plain");

        if (_HNSMapProfile.MapTexture != null)
        {
            Texture2D texture = _HNSMapProfile.MapTexture.texture;
            byte[] bytes = texture.EncodeToJPG();
            request.AddBinaryData("map_file", bytes, Path.ChangeExtension(fileName, ".jpeg"), "image/jpeg");
        }

        request.Send();
        Debug.Log("SaveScence_Send");

        void onFinished(HTTPRequest originalRequest, HTTPResponse response)
        {
            Debug.Log(response.DataAsText);
            bl_SceneLoaderManager.LoadScene("select_scene");
        }
    }
    /// <summary>
    /// 在建筑中放置一个建筑部件
    /// </summary>
    /// <param name="buildingPart">要放置的建筑部件</param>
    public void PlaceBuildingPart(BuildingPart buildingPart)
    {
        // 选择要放置的建筑部件
        BuildingPlacer.Instance.SelectBuildingPart(buildingPart);

        // 将建筑模式设置为放置模式
        BuildingPlacer.Instance.ChangeBuildMode(BuildingPlacer.BuildMode.PLACE);
    }

    public void ChangeBuildMode(int buildMode)
    {
        if (buildMode == 1)
            BuildingPlacer.Instance.ChangeBuildMode(BuildingPlacer.BuildMode.PLACE);
        if (buildMode == 2)
            BuildingPlacer.Instance.ChangeBuildMode(BuildingPlacer.BuildMode.DESTROY);
        if (buildMode == 3)
            BuildingPlacer.Instance.ChangeBuildMode(BuildingPlacer.BuildMode.EDIT);
    }

    private void GenerateMiniMap()
    {
        // 创建预制体相机
        GameObject cameraObject = Instantiate(prefabCamera);
        Camera miniMapCamera = cameraObject.GetComponent<Camera>();

        // 设置相机的输出为渲染纹理
        RenderTexture renderTexture = miniMapCamera.targetTexture;

        // 渲染相机内容
        miniMapCamera.Render();

        // 将渲染的纹理转换为Sprite
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        // 将纹理转换为Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        // 填充到hNSMapProfile.MapTexture
        _HNSMapProfile.MapTexture = sprite;

        // 销毁预制体相机
        Destroy(cameraObject);
    }


    IEnumerator UIDungeonBuild2D()
    {
        yield return new WaitForSeconds(0.5f);
        Dungeon.GetComponent<DungeonRuntimeNavigation>().enableRuntimeNavigation = false;
        Dungeon.Build();

    }
    public GameObject Build2DUI;

    IEnumerator UIDungeonBuild3D()
    {
        Build2DUI.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Dungeon.GetComponent<DungeonRuntimeNavigation>().enableRuntimeNavigation = true;
        Dungeon.Build();
        MiniMapCamera.SetActive(false);
        FPS.SetActive(true);
        _HUDNavigationCanvas.InitMinimap(_HNSMapProfile);
        // _HUDNavigationCanvas.SetMinimapProfile(_HNSMapProfile);
    }
    public void Generate2DScene()
    {
        Build2DUI.SetActive(true);
        MiniMapCamera.SetActive(true);
        FPS.SetActive(false);

        Dungeon.dungeonThemes[0] = dungeonThemes[0];
        Dungeon.DestroyDungeon();
        StartCoroutine("UIDungeonBuild2D");
    }
    // 保存场景
    public void SaveScene()
    {
        // 在此处编写保存场景的逻辑
        // 使用Unity的场景保存功能来保存当前场景
        // 可以使用EditorSceneManager.SaveScene方法
    }

    IEnumerator UpdateDungeonBuild2D()
    {

        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (buildMode == BuildMode.PLACE)
            {
                Dungeon.Build();
            }

        }


    }
    BuildMode buildMode;
    void Start()
    {
        if (ES3FormUser.KeyExists("CustomRoom"))
        {
            var settings = new ES3Settings();
            settings.path = ES3FormUser.Load<string>("CustomRoom");
            Debug.Log(settings.FullPath + " ES3AutoSaveMgr.Current.settings.path ");
            if (ES3.KeyExists("CustomRoomList", settings))
            {
                ES3.Load("CustomRoomList", settings);
            }


            Generate2DScene();
        }
        StartCoroutine(UpdateDungeonBuild2D());


        BuildingPlacer.Instance.OnChangedBuildModeEvent.AddListener((BuildMode mode) => { buildMode = mode; });

        BuildingManager.Instance.OnPlacingBuildingPartEvent.AddListener((BuildingPart buildingPart) =>
        {

        });

        BuildingManager.Instance.OnDestroyingBuildingPartEvent.AddListener((BuildingPart buildingPart) =>
        {
            if (buildingPart.State == BuildingPart.StateType.PREVIEW)
            {

                return;
            }

        });
    }
    private void Update()
    {

    }
}