using UnityEngine;
using EasyBuildSystem.Features.Runtime.Buildings.Part;
using UnityEngine.Events;

using System.Collections;
using UnityEngine.UI;
using BestHTTP;
using System;
using System.IO;
using Defective.JSON;
using Vuplex.WebView;

public class BuildingEditor : MonoBehaviour
{


    public string Host;
    BuildingPart m_BuildingPart;
    public BuildingPart BuildingPart;
    public Canvas canvas;


    public UnityEvent OnPreviewStateEvent;
    public UnityEvent OnDestroyStateEvent;
    public UnityEvent OnEditStateEvent;
    public UnityEvent OnPlacedStateEvent;
    public UnityEvent OnQueueStateEvent;

    void Awake()
    {
        if (this.name == "BuildingEditor")
            return;

        if (GameObject.Find("BuildingEditor") != null)
        {
            BuildingEditor buildingEditor = GameObject.Find("BuildingEditor").GetComponent<BuildingEditor>();
            ImageUrl = buildingEditor.ImageUrl;
            WebLink = buildingEditor.WebLink;
            ModelLink = buildingEditor.ModelLink;

            Destroy(buildingEditor.gameObject);
        }

        Debug.Log(ES3FormUser.Load<string>("CustomToolScenceName"));
        BuildingPart.OnChangedStateEvent.AddListener(OnChangedState);



       // ES3AutoSaveMgr.Current.settings.path = ES3FormUser.Load<string>("CustomToolScenceName"); 
    }
    private void OnDestroy()
    {
       // savefile();
      //  Debug.Log("OnDestroy");
    }
    
    void savefile()
    {
        Debug.Log("Onsavefile");
        Debug.Log("Onsavefile    " + ES3AutoSaveMgr.Current.settings.path);
        ES3AutoSaveMgr.Current.Save();
    }
    void OnChangedState(BuildingPart.StateType state)
    {
      
        Debug.Log(state);

        if(state==BuildingPart.StateType.EDIT)
        {
            //BuildingEditor buildingEditor = this;
            //GameObject obj = new GameObject("BuildingEditor");
            //BuildingEditor newBuildingEditor = obj.AddComponent<BuildingEditor>();
            //newBuildingEditor.ImageUrl = buildingEditor.ImageUrl;
            //newBuildingEditor.WebLink = buildingEditor.WebLink;
            //newBuildingEditor.ModelLink = buildingEditor.ModelLink;
        }

        //switch (state)
        //{
        //    case BuildingPart.StateType.PREVIEW:
        //        OnPreviewStateEvent.Invoke();
        //        break;

        //    case BuildingPart.StateType.DESTROY:
        //        OnDestroyStateEvent.Invoke();
        //        break;

        //    case BuildingPart.StateType.EDIT:
        //        OnEditStateEvent.Invoke();
        //        break;

        //    case BuildingPart.StateType.PLACED:
        //        OnPlacedStateEvent.Invoke();
        //       // Invoke("savefile",1);
             
        //        break;

        //    case BuildingPart.StateType.QUEUE:
        //        OnQueueStateEvent.Invoke();
        //        break;
        //}
    }
    public void LoadImage()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    
    }
    public CanvasWebViewPrefab canvasWebViewPrefab;
    public void SetWebLink()
    {
        GameObject.Find("WebLinkCanvas").GetComponent<CanvasGroup>().alpha=1;
        GameObject.Find("WebLinkCanvas").GetComponent<CanvasGroup>().blocksRaycasts = true;
        GameObject.Find("WebLink_button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("WebLink_button").GetComponent<Button>().onClick.AddListener(()=> { WebLink = GameObject.Find("WebLink_InputField").GetComponent<InputField>().text;

            canvasWebViewPrefab.InitialUrl = WebLink;
            canvasWebViewPrefab.gameObject.SetActive(true );


        });
    
    }

    IEnumerator ShowLoadDialogCoroutine()
    {

        yield return SimpleFileBrowser.FileBrowser.WaitForLoadDialog(SimpleFileBrowser.FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

 
        Debug.Log(SimpleFileBrowser.FileBrowser.Success);

        if (SimpleFileBrowser.FileBrowser.Success)
        {
          
            for (int i = 0; i < SimpleFileBrowser.FileBrowser.Result.Length; i++)
            {
                UploadFile(SimpleFileBrowser.FileBrowser.Result[i]);
                Debug.Log(SimpleFileBrowser.FileBrowser.Result[i]);

                IMBX.ImageLoader imageLoader = IMBX.ImageLoader.Create(1);

                imageLoader.Load( 0, "file:///" + SimpleFileBrowser.FileBrowser.Result[i], (texture, index) =>
                {
                    if (texture != null)
                    {
                      this.GetComponentInChildren<DImageDisplayHandler>().SetRawImage(this.GetComponentInChildren<RawImage>(), texture);

                      //UploadFile(SimpleFileBrowser.FileBrowser.Result[i]);
                    }
                }, 0, 10);
            }
              

          
        }
    }

    void UploadFile(string path)
    {
        string fileName = Path.GetFileName(path);
        string fileType = "";
        if (fileName.EndsWith(".png"))
        {
            fileType = "image/png";
        }
        else if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg"))
        {
            fileType = "image/jpeg";
        }
        else if (fileName.EndsWith(".gif"))
        {
            fileType = "image/gif";
        }


        Debug.Log("UploadFile  " + path+"  "+ fileName + "  "+ fileType);
        var request = new HTTPRequest(new Uri(Host+ "/api/Upload"), HTTPMethods.Post, onFinished);
        request.AddField("user_id", ES3.Load("user_id").ToString());
        
        request.AddBinaryData("file", File.ReadAllBytes(path), fileName, fileType);
        //request.AddField("file", ES3AutoSaveMgr.Current.settings.FullPath);


        request.Send();

        void onFinished(HTTPRequest originalRequest, HTTPResponse response)
        {
            Debug.Log(response.DataAsText);
            JSONObject jd = new JSONObject(response.DataAsText);
            if (jd["url"] != null)
            {
                ImageUrl = jd["url"].stringValue ;
            }

        }
    }
    public string ImageUrl, WebLink,ModelLink;
 

    private void Start()
    {
        if (this.name == "BuildingEditor")
            return;


        canvas.worldCamera = Camera.main;

        if (ImageUrl != "")
        {
            IMBX.ImageLoader imageLoader = IMBX.ImageLoader.Create(1);

            imageLoader.Load(0, ImageUrl, (texture, index) =>
            {
                if (texture != null)
                {
                    this.GetComponentInChildren<DImageDisplayHandler>().SetRawImage(this.GetComponentInChildren<RawImage>(), texture);

                    //UploadFile(SimpleFileBrowser.FileBrowser.Result[i]);
                }
            }, 0, 10);
        }


        if (WebLink != "")
        {
            canvasWebViewPrefab.InitialUrl = WebLink;
            canvasWebViewPrefab.gameObject.SetActive(true);
        }
      
    }
    public void OpenLink()
    {
        Application.OpenURL(WebLink);
    }
    void Update()
    {
        
    }
}
