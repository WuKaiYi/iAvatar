using BestHTTP;
using Crosstales.FB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defective.JSON;
using GLTFast;
using System;
using System.IO;
public class groupModelUpload : MonoBehaviour
{
    public GameObject targetObject, CameraPivot;
    public string GlbUrl;
    private string[] Model3D_extensions = { "glb", "GLB" };
    private Vector3 oldSize;
    private string glbFileType;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnpenModel3DFile()
    {



        if (targetObject != null)
        {
            //string path = FileBrowser.Instance.OpenSingleFile("Open file", "", "", Model3D_extensions);
            //string fullpath = "file:///" + path;
            //LoadModel3D(fullpath);
            //ServerConfig.UploadFile(path, onFinished);
#if UNITY_STANDALONE || UNITY_EDITOR
            glbFileType = NativeFilePicker.ConvertExtensionToFileType("glb"); // Returns "application/pdf" on Android and "com.adobe.pdf" on iOS
            Debug.Log("pdf's MIME/UTI is: " + glbFileType);

            if (NativeFilePicker.IsFilePickerBusy())
                return;


            // Pick a PDF file
            NativeFilePicker.Permission permissionPC = NativeFilePicker.PickFile((path) =>
            {
                if (path == null)
                    Debug.Log("Operation cancelled");
                else
                    LoadModel3D(path);
            }, new string[] { glbFileType });

            Debug.Log("Permission result: " + permissionPC);

#endif

#if UNITY_IOS || UNITY_ANDROID

            if (NativeFilePicker.IsFilePickerBusy())
                return;


            // Pick a PDF file
            NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
            {
                if (path == null)
                    Debug.Log("Operation cancelled");
                else
                    LoadModel3D(path);
            });

            Debug.Log("Permission result: " + permission);


#endif


        }

    }
    void onFinished(HTTPRequest originalRequest, HTTPResponse response)
    {
        Debug.Log(response.DataAsText);
        JSONObject jd = new JSONObject(response.DataAsText);
        if (jd["url"] != null)
        {
            GlbUrl = jd["url"].stringValue;
            FindFirstObjectByType<runtimeModelDownload>().UploadModels();
        }

    }


    public void Saved3Dmodel()
    {

       var settings = new ES3Settings();

        settings.path = ES3FormUser.Load<string>("runtimeModel");
        Debug.Log(settings.FullPath + " ES3AutoSaveMgr.Current.settings.path ");

 

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("CustomTool"))
        {
            if (!g.activeSelf)
            {
                Destroy(g);
            }

        }


        ES3.Save("Saved3DmodelList", GameObject.FindGameObjectsWithTag("CustomTool"), settings);


        string fileName = Path.GetFileName(settings.FullPath);

        var request = new HTTPRequest(new Uri(ServerConfig.host + "/api/ES3_Space_Upload"), HTTPMethods.Post, onFinished);
        request.AddField("user_id", ES3.Load("user_id").ToString());
        request.AddField("Space_name", "StarClub");


        byte[] bytes = CaptureScreenshotWithoutUI().EncodeToJPG();
        request.AddBinaryData("pic_file", bytes, Path.ChangeExtension(fileName, ".jpeg"), "image/jpeg");

        request.AddBinaryData("file", File.ReadAllBytes(settings.FullPath), fileName, "text/plain");
        request.Send();
        Debug.Log("SaveScence_Send");


    }
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
    public async void LoadModel3D(string path)
    {
        //oldSize = this.transform.localScale;
        //this.transform.localScale = Vector3.one;
        if (path != "")
        {
            //this.transform.GetChild(1).gameObject.SetActive(false);
            //if (GetComponent<Animation>() != null)
            //{
            //    Destroy(GetComponent<Animation>());
            //}
            for (int i = 0; i < targetObject.transform.childCount; i++)
            {
                Destroy(targetObject.transform.GetChild(i).gameObject);
            }
            Debug.Log($"path:  {path}");
            var gltfImport = new GltfImport();
            await gltfImport.Load(path);
            var instantiator = new GameObjectInstantiator(gltfImport, targetObject.transform);
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
                GameObject obj = targetObject.transform.GetChild(0).gameObject;

                obj.tag = "CustomTool";

                SetLayer(obj, 12);
                if (!path.StartsWith("http"))
                {
                    ServerConfig.UploadFile(path, onFinished);
                }
               
               
            }

        }
        return;

    }

    // 这个方法设置对象及其所有子对象的 layer
    public void SetLayer(GameObject root, int layer)
    {
        root.layer = layer;
        foreach (Transform child in root.transform)
        {
            SetLayer(child.gameObject, layer);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

}
