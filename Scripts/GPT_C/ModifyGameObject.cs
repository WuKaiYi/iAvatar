using BestHTTP;
using Crosstales.FB;
using Defective.JSON;
using HighlightPlus;
using Lean.Gui;
using SickscoreGames.HUDNavigationSystem;
using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Effects;


public class ModifyGameObject : MonoBehaviour
{
    private GameObject targetObject;
    private Vector3 position, m_position;
    private Vector3 localEulerAngles, m_localEulerAngles;
    private Vector3 localScale, m_localScale;
    public LeanWindow canvas;
    public GameObject Info;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "custon_tool_top_camera")
        {
            HighlightSelection.instance.OnObjectSelected += OnObjectSelected;
            HighlightSelection.instance.OnObjectUnSelected += OnObjectUnSelected;
        }


        HighlightSelection.instance.Hover += Hover;
        HighlightSelection.instance.UnHover += UnHover;
        canvas.TurnOff();
        //canvas = this.GetComponent<Canvas>();


        ////編寫這些按鈕的初始化,使用gameobject.find 物體的路徑路徑獲取對應的組件
        //moveButton = GameObject.Find("MainCanvas/EditorCanvas/6_CustomToolEditing/panel/move").GetComponent<Button>();
        //rotateButton = GameObject.Find("MainCanvas/EditorCanvas/6_CustomToolEditing/panel/rotate").GetComponent<Button>();
        //scaleButton = GameObject.Find("MainCanvas/EditorCanvas/6_CustomToolEditing/panel/rotate").GetComponent<Button>();
        //inputTextButton = GameObject.Find("MainCanvas/EditorCanvas/6_CustomToolEditing/panel/Input").GetComponent<Button>();
        //uploadImageButton = GameObject.Find("MainCanvas/EditorCanvas/6_CustomToolEditing/panel/image").GetComponent<Button>();
        //uploadModelButton = GameObject.Find("MainCanvas/EditorCanvas/6_CustomToolEditing/panel/model").GetComponent<Button>();
        //DuplicateButton = GameObject.Find("MainCanvas/EditorCanvas/6_CustomToolEditing/panel/duplicate").GetComponent<Button>();
        //deleteButton = GameObject.Find("MainCanvas/EditorCanvas/6_CustomToolEditing/panel/delete").GetComponent<Button>();
        //ModifyToggle = GameObject.Find("MainCanvas/EditorCanvas/6_CustomToolEditing/panel/modify").GetComponent<Toggle>();


    }

    //unity中定義 public Button 移動,旋轉,縮放,輸入文字,上傳圖片,上傳模型,複製,刪除

    public LeanButton moveButton;
    public LeanButton rotateButton;
    public LeanButton scaleButton;
    public LeanButton inputTextButton;
    public LeanButton uploadImageButton;
    public LeanButton uploadModelButton;
    public LeanButton DuplicateButton;
    public LeanButton deleteButton;

    public LeanToggle  ModifyToggle;




    //定義togger 是否可以自定義




    void ShowInfo(GameObject go, bool show)
    {
        if (go.transform.GetComponent<CustomLoader>().dataType == CustomLoader.DataType.Model3D)
        {
            if (go.transform.GetComponent<CustomLoader>().InfoData != "")
            {
                Info.SetActive(show);
                //Info這個ui image 的屏幕上的位置 與 go這個GameObject的位置重合
                Vector3 screenPos = Camera.main.WorldToScreenPoint(go.transform.position);
                Info.transform.position = screenPos;
                Info.GetComponentInChildren<Text>().text = go.transform.GetComponent<CustomLoader>().InfoData;
            }
            else
            {
                Info.SetActive(false);
            }

        }
    }

    bool Hover(GameObject go)
    {
        if (go.tag != HighlightSelection.instance.TagMask)
            return true;

        Debug.Log(go.name + " Hover!");
        go.transform.GetComponent<CustomLoader>().Hover.Invoke();
        ShowInfo(go, true);
        return true;
    }
    bool UnHover(GameObject go)
    {
        if (go.tag != HighlightSelection.instance.TagMask)
            return true;

        Debug.Log(go.name + " UnHover!");
        go.transform.GetComponent<CustomLoader>().UnHover.Invoke();
        ShowInfo(go, false);
        return true;
    }

    bool OnObjectSelected(GameObject go)
    {
        Debug.Log(go.name + " selected!");
        canvas.TurnOn();
        targetObject = go;
        position = go.transform.position;
        localEulerAngles = go.transform.localEulerAngles;
        localScale = go.transform.localScale;
        go.transform.GetComponent<CustomLoader>().Selected.Invoke();

        ModifyToggle.On = go.transform.GetComponent<CustomLoader>().DynamicEditing;

        //DataType 有一線
        //    Image,
        //    Model3D,
        //    WebLink,
        //    TaskFlag,
        //    Null,
        //    ChatBoard
        //

        switch (go.transform.GetComponent<CustomLoader>().dataType)
        {
            case CustomLoader.DataType.Model3D:

                inputTextButton.transform.parent.gameObject.SetActive(false);

                uploadImageButton.transform.parent.gameObject.SetActive(false);

                uploadModelButton.transform.parent.gameObject.SetActive(true);

                break;
            case CustomLoader.DataType.Image:
                inputTextButton.transform.parent.gameObject.SetActive(false);

                uploadImageButton.transform.parent.gameObject.SetActive(true);

                uploadModelButton.transform.parent.gameObject.SetActive(false);
                break;
            case CustomLoader.DataType.WebLink:
                inputTextButton.transform.parent.gameObject.SetActive(true);

                uploadImageButton.transform.parent.gameObject.SetActive(false);

                uploadModelButton.transform.parent.gameObject.SetActive(false);
                break;
            case CustomLoader.DataType.TaskFlag:
                inputTextButton.transform.parent.gameObject.SetActive(true);

                uploadImageButton.transform.parent.gameObject.SetActive(false);

                uploadModelButton.transform.parent.gameObject.SetActive(false);
                break;
            case CustomLoader.DataType.Null:
                inputTextButton.transform.parent.gameObject.SetActive(false);

                uploadImageButton.transform.parent.gameObject.SetActive(false);

                uploadModelButton.transform.parent.gameObject.SetActive(false);
                break;
            case CustomLoader.DataType.ChatBoard:
                inputTextButton.transform.parent.gameObject.SetActive(true);

                uploadImageButton.transform.parent.gameObject.SetActive(false);

                uploadModelButton.transform.parent.gameObject.SetActive(false);
                break;

          
        }






        return true;
    }

    bool OnObjectUnSelected(GameObject go)
    {
        Debug.Log(go.name + " un-selected!");
        go.transform.GetComponent<CustomLoader>().UnSelected.Invoke();
        if (targetObject == go)
        {
            canvas.TurnOff();
            targetObject = null;

        }




        return true;
    }

    public static ModifyGameObject instance
    {
        get
        {
            return FindObjectOfType<ModifyGameObject>();
        }
    }

    public void InitializeObject(GameObject obj)
    {
        targetObject = obj;
    }
    private string[] Image_extensions = { "png", "jpg", "jpeg" };
    private string[] Model3D_extensions = { "glb", "GLB" };
    public void OnpenImageFile()
    {
        if (targetObject != null)
        {
            string path = Crosstales.FB. FileBrowser.Instance.OpenSingleFile("Open file", "", "", Image_extensions);
            Debug.Log($"OpenSingleFile: '{path}'", this);
            string fullpath = "file:///" + path;
            targetObject.GetComponent<CustomLoader>().LoadImage(fullpath);
            ServerConfig.UploadFile(path, onFinished);
        }

    }


    public void OnpenModel3DFile()
    {
        if (targetObject != null)
        {
            string path = Crosstales.FB.FileBrowser.Instance.OpenSingleFile("Open file", "", "", Model3D_extensions);
            string fullpath = "file:///" + path;
            targetObject.GetComponent<CustomLoader>().LoadModel3D(fullpath);
            ServerConfig.UploadFile(path, onFinished);
        }

    }
    void onFinished(HTTPRequest originalRequest, HTTPResponse response)
    {
        Debug.Log(response.DataAsText);
        JSONObject jd = new JSONObject(response.DataAsText);
        if (jd["url"] != null)
        {
            targetObject.GetComponent<CustomLoader>().data = jd["url"].stringValue;
        }

    }

    public void SetModifyToggle(bool b)
    {
        targetObject.GetComponent<CustomLoader>().DynamicEditing = b;
    }

    public void DeleteObjest()
    {

        if (targetObject != null)
        {
            canvas.TurnOff();
            Destroy(targetObject.gameObject);
        }

    }

    public void DuplicateObject()
    {

        if (targetObject != null)
        {
            canvas.TurnOff();
            targetObject.GetComponent<HighlightEffect>().highlighted = false;
            GameObject g = Instantiate(targetObject);
            g.transform.position = new Vector3(targetObject.transform.position.x + 2, targetObject.transform.position.y, targetObject.transform.position.z + 2);
        }
    }
    public void SendComment(string comment)
    {
        if (targetObject != null)
        {

            if (!ES3FormUser.KeyExists("avatar_pic"))
            {
                ES3FormUser.Save("avatar_pic", "http://galaxycao.asuscomm.com:7771/upload/avatar.png");
            }

            JSONObject jd = new JSONObject();
            jd.AddField("content", comment);
            jd.AddField("name", (string)ES3FormUser.Load("username"));
            jd.AddField("avatar", (string)ES3FormUser.Load("avatar_pic"));
            Debug.Log(jd.ToString());
            var request = new HTTPRequest(new Uri(ServerConfig.host + "/comment/comments/create"), HTTPMethods.Post, onFinished);
            request.SetHeader("Content-Type", "application/json; charset=UTF-8");
            request.RawData = System.Text.Encoding.UTF8.GetBytes(jd.ToString());
            request.Send();

            void onFinished(HTTPRequest originalRequest, HTTPResponse response)
            {
                Debug.Log(response.DataAsText);
                JSONObject jd = new JSONObject(ServerConfig.ParseUnicode(response.DataAsText));

                targetObject.GetComponent<CustomLoader>().data = "" + jd["id"].intValue;
                targetObject.GetComponent<CustomLoader>().LoadChatBoard(jd["id"].intValue.ToString());
            }
        }
    }

    public void SetModelInformation(string s)
    {
        if (targetObject != null)
        {
            targetObject.GetComponent<CustomLoader>().InfoData = (s);

        }
    }
    public void SetTaskFlag(string s)
    {
        if (targetObject != null)
        {
            targetObject.GetComponent<CustomLoader>().LoadTaskFlag(s);

        }
    }

    public void setInputText(string s)
    {
        switch (targetObject.transform.GetComponent<CustomLoader>().dataType)
        {
            
            case CustomLoader.DataType.WebLink:
                SetWebLink(s);
                break;
            case CustomLoader.DataType.TaskFlag:
                SetTaskFlag(s);
                break;
           
            case CustomLoader.DataType.ChatBoard:
              SendComment(s);
                break;
        }
    }

    public void SetWebLink(string s)
    {
        if (targetObject != null)
        {
            targetObject.GetComponent<CustomLoader>().LoadWebLink(s);

        }
    }
    public void MoveObject()
    {

        if (targetObject != null)
        {
            targetObject.transform.position = position + m_position;
        }
    }

    public void MoveObject_x(float xposition)
    {
        m_position = new Vector3(xposition, m_position.y, m_position.z);
        MoveObject();
    }

    public void MoveObject_y(float yposition)
    {
        m_position = new Vector3(m_position.x, yposition, m_position.z);
        MoveObject();
    }

    public void MoveObject_z(float zposition)
    {
        m_position = new Vector3(m_position.x, m_position.y, zposition);
        MoveObject();
    }

    public void RotateObject()
    {
        if (targetObject != null)
        {
            targetObject.transform.localEulerAngles = localEulerAngles + m_localEulerAngles;
        }
    }
    public void RotateObject_x(float xRotation)
    {
        m_localEulerAngles = new Vector3(xRotation, m_localEulerAngles.y, m_localEulerAngles.z);
        RotateObject();

    }

    public void RotateObject_y(float yRotation)
    {
        m_localEulerAngles = new Vector3(m_localEulerAngles.x, yRotation, m_localEulerAngles.z);
        RotateObject();
    }

    public void RotateObject_z(float zRotation)
    {
        m_localEulerAngles = new Vector3(m_localEulerAngles.x, m_localEulerAngles.y, zRotation);
        RotateObject();
    }



    public void ResizeObject()
    {
        if (targetObject != null)
        {
            targetObject.transform.localScale = localScale + m_localScale;
        }
    }


    public void ResizeObject_x(float xScale)
    {
        m_localScale = new Vector3(xScale, m_localScale.y, m_localScale.z);
        ResizeObject();

    }

    public void ResizeObject_y(float yScale)
    {
        m_localScale = new Vector3(m_localScale.x, yScale, m_localScale.z);
        ResizeObject();
    }

    public void ResizeObject_z(float zScale)
    {
        m_localScale = new Vector3(m_localScale.x, m_localScale.y, zScale);
        ResizeObject();
    }

}