using UnityEngine;
using UnityEngine.UI;
using Vuplex.WebView.Demos;
using Vuplex.WebView;
using Unity.VisualScripting;
using UnityEngine.Events;
using SickscoreGames.HUDNavigationSystem;
using frame8.Logic.Misc.Other.Extensions;

using System;
// using Unity.Barracuda;
using GLTFast;
using UnityEngine.SceneManagement;
using Defective;
using BestHTTP;
using Defective.JSON;
using BestHTTP.JSON;
using ChatBoard;
using HighlightPlus;

public class CustomLoader : MonoBehaviour
{
    public enum DataType
    {
        Image,
        Model3D,
        WebLink,
        TaskFlag,
        Null,
        ChatBoard,
        other
    }

    public bool DynamicEditing;



    public DataType dataType;

    public Text TaskFlagUI;

    public RawImage RawImageUI;

    [TextArea]
    public string data, InfoData;

    public UnityEvent Selected, UnSelected, Hover, UnHover;

    private void Start()
    {
        if (dataType == DataType.other)
        {
            return;
        }


        FindAssemblyByPath();
        if (dataType == DataType.WebLink)
        {
            this.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
            _canvasWebViewPrefab = this.GetComponentInChildren<CanvasWebViewPrefab>();
        }
        if (GameObject.FindAnyObjectByType<CustomObjectController>() != null)
        {
            if (!DynamicEditing)
            {
                LoadData();
                GetComponent<HighlightEffect>().ignore = true;
            }
            else
            {
                if (!GameObject.FindAnyObjectByType<CustomObjectController>().IsHost)
                {
                    Destroy(this.gameObject);
                }

            }

            return;
        }
        else
        {

        }
        LoadData();


    }
    /// <summary>
    /// 更具路徑找到組件
    /// </summary>
    private void FindAssemblyByPath()
    {
        this.transform.GetComponentAtPath("Canvas/Text (Legacy)", out TaskFlagUI);


    }

    CanvasWebViewPrefab _canvasWebViewPrefab;
    //HardwareKeyboardListener _hardwareKeyboardListener;

    void HandleKeyDownEvent(object sender, KeyboardInputEventArgs eventArgs)
    {
        var webViewWithKeyDown = _canvasWebViewPrefab.WebView as IWithKeyDownAndUp;
        if (webViewWithKeyDown != null)
        {
            webViewWithKeyDown.KeyDown(eventArgs.Value, eventArgs.Modifiers);
        }
        else
        {
            _canvasWebViewPrefab.WebView.SendKey(eventArgs.Value);
        }
    }
    void HandleKeyUpEvent(object sender, KeyboardInputEventArgs eventArgs)
    {

        var webViewWithKeyUp = _canvasWebViewPrefab.WebView as IWithKeyDownAndUp;
        webViewWithKeyUp?.KeyUp(eventArgs.Value, eventArgs.Modifiers);

    }
    CanvasWebViewPrefab canvasWebViewPrefab;
    public void LoadData()
    {
        switch (dataType)
        {
            case DataType.Image:
                // 實現圖片上傳和載入的邏輯
                LoadImage(data);
                break;
            case DataType.Model3D:
                // 實現3D模型上傳和載入的邏輯
                LoadModel3D(data);
                break;
            case DataType.WebLink:
                canvasWebViewPrefab = this.GetComponentInChildren<CanvasWebViewPrefab>();
                // 實現網頁鏈接上傳和載入的邏輯
                LoadWebLink(data);
                break;
            case DataType.TaskFlag:
                // 實現網頁鏈接上傳和載入的邏輯
                LoadTaskFlag(data);
                break;
            case DataType.ChatBoard:
                // 實現網頁鏈接上傳和載入的邏輯
                LoadChatBoard(data);
                break;
        }
    }
    public void SendComment(string comment)
    {

    }

    public void LoadChatBoard(string data)
    {
        if (data == "")
        {

        }
        else
        {

        }


    }

    public void LoadImage(string path)
    {
        IMBX.ImageLoader imageLoader = IMBX.ImageLoader.Create(1);

        imageLoader.Load(0, path, (texture, index) =>
        {
            if (texture != null)
            {
                this.GetComponentInChildren<DImageDisplayHandler>().SetRawImage(this.GetComponentInChildren<RawImage>(), texture);

                //UploadFile(FileBrowser.Result[i]);
            }
        }, 0, 10);
    }

    public void LoadTaskFlag(string t)
    {
        TaskFlagUI.text = t;
        this.GetComponentInChildren<HUDNavigationElement>().indicatorOnscreenDistanceTextFormat = t + "\n{0}m";
        data = t;
    }
    private Bounds bounds;

    Vector3 oldSize;
    public async void LoadModel3D(string path)
    {
        oldSize = this.transform.localScale;
        this.transform.localScale = Vector3.one;
        if (path != "")
        {
            this.transform.GetChild(1).gameObject.SetActive(false);
            if (GetComponent<Animation>() != null)
            {
                Destroy(GetComponent<Animation>());
            }
            for (int i = 2; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
            Debug.Log($"path:  {path}");
            var gltfImport = new GltfImport();
            await gltfImport.Load(path);
            var instantiator = new GameObjectInstantiator(gltfImport, transform);
            var success = await gltfImport.InstantiateMainSceneAsync(instantiator);
            if (success)
            {

                // Get the SceneInstance to access the instance's properties
                var sceneInstance = instantiator.SceneInstance;

                // Enable the first imported camera (which are disabled by default)
                //if (sceneInstance.Cameras is { Count: > 0 })
                //{
                //    sceneInstance.Cameras[0].enabled = true;
                //}

                // Decrease lights' ranges
                if (sceneInstance.Lights != null)
                {
                    foreach (var glTFLight in sceneInstance.Lights)
                    {
                        glTFLight.range *= 0.1f;
                    }
                }

                // Play the default (i.e. the first) animation clip
                var legacyAnimation = instantiator.SceneInstance.LegacyAnimation;
                if (legacyAnimation != null)
                {
                    legacyAnimation.Play();
                }

            }
            GameObject obj = this.transform.GetChild(2).gameObject;
            // obj .transform.localEulerAngles = Vector3.zero ;
            // 设置模型的最大尺寸为3m
            float maxScale = 5;

            // 获取模型的最大边界尺寸
            bounds = GetModelBounds(obj);

            // 计算模型的缩放比例
            float scale = maxScale / Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

            // 缩放模型
            obj.transform.localScale = new Vector3(scale, scale, scale);
            // 计算模型底部与地面的距离
            // float distanceToGround = bounds.extents.y - bounds.center.y;
            // bounds = GetModelBounds(obj);
            // 移动模型使其底部与地面对齐
            //obj.transform.position -= new Vector3(0, distanceToGround, 0);
            //   this.transform.localScale =oldSize;
        }
        return;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    // 获取模型的最大边界尺寸
    private Bounds GetModelBounds(GameObject model)
    {
        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds();

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }


    /// <summary>
    /// 刷新網頁鏈接
    /// </summary>

    public void RefreshPageLink()
    {
        this.GetComponentInChildren<CanvasWebViewPrefab>().WebView.LoadUrl(data);
    }
    public void BackPageLink()
    {
        this.GetComponentInChildren<CanvasWebViewPrefab>().WebView.GoBack();
    }
    /// <summary>
    /// 打開瀏覽器
    /// </summary>
    public void OpenBrowser()
    {
        Application.OpenURL(data);
    }


    public async void LoadWebLink(string Link)
    {
        if (Link == "")
            return;

        await this.GetComponentInChildren<CanvasWebViewPrefab>().WaitUntilInitialized();
        this.GetComponentInChildren<CanvasWebViewPrefab>().WebView.LoadUrl(Link);
        data = Link;
        // 實現網頁鏈接上傳和載入的邏輯
        Debug.Log("Loading web link: " + Link);
        showWebView=true;
    }
    bool showWebView=false;
    public void Update()
    {
        if (Vector3.Distance(Camera.main.transform.position, this.transform.position) <= 30)
        {
            if (showWebView == true)
                if (dataType == DataType.WebLink && !canvasWebViewPrefab.Visible)
                {
                    if (!canvasWebViewPrefab.Visible)
                    {
                        canvasWebViewPrefab.WebView.Reload();
                    }
                    
                    canvasWebViewPrefab.Visible= !canvasWebViewPrefab.Visible;
                    canvasWebViewPrefab.WebView.SetDefaultBackgroundEnabled(canvasWebViewPrefab.Visible);
                    canvasWebViewPrefab.WebView.SetRenderingEnabled(canvasWebViewPrefab.Visible);
                }
        }
        else
        {
            if (showWebView == true)
                if (dataType == DataType.WebLink && canvasWebViewPrefab.Visible)
                {
                    canvasWebViewPrefab.Visible = !canvasWebViewPrefab.Visible;
                    canvasWebViewPrefab.WebView.SetDefaultBackgroundEnabled(canvasWebViewPrefab.Visible);
                    canvasWebViewPrefab.WebView.SetRenderingEnabled(canvasWebViewPrefab.Visible);
                 
                }
        }


    }
}