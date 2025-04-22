using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Events;

using Defective.JSON;
using BestHTTP;
using System;
using ReadyPlayerMe.Core;
using frame8.Logic.Misc.Other.Extensions;
using UnityEngine.SceneManagement;
using I2.Loc;

public class AvatarUI : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public GameObject[] Avatars;
    public Text text;
    public InputField username, custom_name, email;
    int i = 0;
    

    /*    {
      "id": 3,
      "username": "st1",
      "custom_name": "stuuu",
      "email": "333333",
      "avatar_url": "aasdddd"
    }*/
    public UnityEvent<string> Error;

    void Change_api(JSONObject json)
    {
        var request = new HTTPRequest(new Uri(Host + "/api/change_user"), HTTPMethods.Post, callback: OnRequestFinished);

        request.SetHeader("Content-Type", "application/json; charset=UTF-8");
        request.RawData = System.Text.Encoding.UTF8.GetBytes(json.ToString());
        request.Send();

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

                        ES3FormUser.Save("username", js["username"].stringValue);
                        ES3FormUser.Save("custom_name", js["custom_name"].stringValue);
                        ES3FormUser.Save("email", js["email"].stringValue);
                        ES3FormUser.Save("avatar_url", js["avatar_url"].stringValue);
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

            }
        }
    }
   public RawImage avatar_pic;
    public void save_avatar()
    {
        var request = new HTTPRequest(new Uri(Host + "/api/change_avatar_pic"), HTTPMethods.Post, callback: OnRequestFinished);

        request.AddField("user_id", ES3.Load("user_id").ToString());

        byte[] bytes = ServerConfig.ConvertToTexture2D(avatar_pic.texture).EncodeToJPG();
        request.AddBinaryData("file", bytes, "avatar.jpeg", "image/jpeg");
        request.Send();

        void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            Debug.Log(resp.DataAsText);
            //bl_SceneLoaderManager.LoadScene("select_scene");
            bl_SceneLoaderManager.LoadScene("ModelSelect");
        }
    }
    public void Chabge_username()
    {
        JSONObject json = new JSONObject();
        json.AddField("id",""+ ES3.Load("user_id"));
        json.AddField("username", username.text);
        
        Change_api(json );
    }
    public void Chabge_custom_name()
    {
        JSONObject json = new JSONObject();
        json.AddField("id", "" + ES3.Load("user_id"));
        json.AddField("custom_name", custom_name.text);
        Change_api(json);
    }
    public void Chabge_email()
    {
        JSONObject json = new JSONObject();
        json.AddField("id", "" + ES3.Load("user_id"));
        json.AddField("email", email.text);
        Change_api(json);
    }
    public void Chabge_avatar_url(string avatar_url)
    {
        JSONObject json = new JSONObject();
        json.AddField("id", "" + ES3.Load("user_id"));
        json.AddField("avatar_url", avatar_url);
        Change_api(json);
    }

    public void Left()
    {
        i--;
        if (i < 0)
        {
            i = Avatars.Length - 1;
        }
   
        move();
    }
    public void right()
    {
        i++;
        if (i == Avatars.Length)
        {

            i = 0;
        }
        move();
    }

    public void move()
    {
        if (i == 6)
        {

            text.text = LocalizationManager.GetTranslation("Customize");
        }
        else
        {
            text.text = "Avatar-" + Avatars[i].name;
        }

        string url = "";
        if (i <Avatars.Length-1)
        {
            //text.text = "Customize Avatar";
            ES3FormUser.Save<string>("AvatarUrl", "int" + i);
            url = "int" + i;
        }
        if (i == Avatars.Length - 1)
        {
            //text.text = "Customize Avatar";
            ES3FormUser.Save<string>("AvatarUrl", ES3FormUser.Load<string>("CustomizeAvatarUrl"));
            url = ES3FormUser.Load<string>("CustomizeAvatarUrl");
        }
      

        //text.text = "Avatar-" + Avatars[i].name;
        virtualCamera.Follow = Avatars[i].transform.GetComponentAtPath<Transform>("Armature/Hips/Spine/Spine1/Spine2/Neck");
        virtualCamera.LookAt = virtualCamera.Follow;

        Chabge_avatar_url(url);


    }
    // Start is called before the first frame update
    void Start()
    {
        if (!ES3FormUser.KeyExists("Username"))
        {
            ES3FormUser.Save<string>("Username", "student");
        }
        if (!ES3FormUser.KeyExists("AvatarUrl"))
        {
            ES3FormUser.Save<string>("AvatarUrl", "int0");
        }
        //if (!ES3FormUser.KeyExists("CustomizeAvatarUrl"))
        //{
        //    ES3FormUser.Save<string>("CustomizeAvatarUrl", "https://models.readyplayer.me/6418cfb004207164c853134f.glb");
        //}
        //i = 0;
       
        // text.text = "Avatar-" + Avatars[i].name;
        

        if (ES3FormUser.KeyExists("Username"))
        {
            if (username != null)
            {
                username.text = (string)ES3FormUser.Load("Username");
            }
            //InputField username, custom_name, email;
            if (custom_name != null)
            {
                custom_name.text = (string)ES3FormUser.Load("custom_name");
            }
            if (email != null)
            {
                email.text = (string)ES3FormUser.Load("email");
            }
        }
        load();
    }
    public void Set_name(string n)
    {
        ES3FormUser.Save("Username", n);
    }
    public UnityEvent Loaded;
    public  GameObject avatar;

    public RuntimeAnimatorController animator;
    public string Host="";
    public Transform CustonAvatar;

    //void LoadAvatarFormWeb(object  sender, CompletionEventArgs args)
    //{
    //    GameObject game=null ;
    //    if (avatar != null)
    //    {
    //        Destroy(avatar);

    //    }

    //    Debug.Log($"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]");
    //    avatar = args.Avatar;
    //    avatar.name = "Customize Avatar";
    //    avatar.AddComponent<EyeAnimationHandler>();

    //    AvatarAnimationHelper.SetupAnimator(args.Metadata, avatar);
    //    avatar.GetComponent<Animator>().runtimeAnimatorController = animator;


    //    avatar.transform.parent = CustonAvatar;
    //    avatar.transform.localEulerAngles = Vector3.zero;
    //    avatar.transform.localPosition = Vector3.zero;
    //    Avatars[6] = avatar;

    //    move();

    //    //avatar.AddComponent<VoiceHandler>();
    //    Loaded.Invoke();
    //    //if (game != null)
    //    //{
    //    //    Destroy(game);
    //    //}

    //}


    //public void CustomizeAvatarLoad(string url)
    //{
       
          
        
    //        var avatarLoader = new AvatarObjectLoader();
    //    avatarLoader.OnCompleted += LoadAvatarFormWeb;

    //    //avatarLoader.OnCompleted += (sender, args) =>
    //    //{
    //    //    Debug.Log($"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]");
    //    //    avatar = args.Avatar;
    //    //    avatar.name = "Customize Avatar";
    //    //    avatar.AddComponent<EyeAnimationHandler>();

    //    //    AvatarAnimatorHelper.SetupAnimator(args.Metadata.BodyType, avatar);
    //    //    avatar.GetComponent<Animator>().runtimeAnimatorController = animator;
    //    //    i = Avatars.Length - 1;
    //    //    move();
    //    //    Avatars[6]=avatar;
    //    //    avatar.transform.parent = Avatars[i].transform;
    //    //    avatar.transform.localEulerAngles = Vector3.zero;
    //    //    avatar.transform.localPosition = Vector3.zero;
    //    //    //avatar.AddComponent<VoiceHandler>();
    //    //    Loaded.Invoke();
    //    //};

    //    avatarLoader.OnFailed += (sender, args) =>
    //    {
    //        Debug.Log(args.Type);
    //    };

    //    avatarLoader.LoadAvatar(url);
    //    Chabge_avatar_url(url);
    //    ES3FormUser.Save<string>("CustomizeAvatarUrl", url);
    //    i = 6;
    //    move();
    //}

    public  void load()
    {
        if (ES3FormUser.Load<string>("AvatarUrl") != null)
        {
            Debug.Log(ES3FormUser.Load<string>("AvatarUrl"));
            if (ES3FormUser.Load<string>("AvatarUrl").Substring(0, 3) == "int")
            {
                i = int.Parse(Regex.Split(ES3FormUser.Load<string>("AvatarUrl"), "int", RegexOptions.IgnoreCase)[1]);
                move();
                Loaded.Invoke();
                //avatar.AddComponent<VoiceHandler>();
            
            }
            else
            {
                //ES3FormUser.Save<string>("CustomizeAvatarUrl", ES3FormUser.Load<string>("AvatarUrl"));
                i = Avatars.Length - 1;
                move();
                Loaded.Invoke();
            }
        }


        if (ES3FormUser.KeyExists("CustomizeAvatarUrl"))
        {
            var startTime = Time.time;
            avatarObjectLoader = new AvatarObjectLoader();
            avatarObjectLoader.AvatarConfig = inGameConfig;
            avatarObjectLoader.OnCompleted += (sender, args) =>
            {
              
                if (avatar != null)
                {
                    Destroy(avatar);

                }

                Debug.Log($"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]");
                avatar = args.Avatar;
                avatar.name = "Customize Avatar";
                avatar.AddComponent<EyeAnimationHandler>();

                AvatarAnimationHelper.SetupAnimator(args.Metadata, avatar);
                avatar.GetComponent<Animator>().runtimeAnimatorController = animator;


                avatar.transform.parent = CustonAvatar;
                avatar.transform.localEulerAngles = Vector3.zero;
                avatar.transform.localPosition = Vector3.zero;
                Avatars[6] = avatar;

                i = Avatars.Length - 1;
                move();
                Loaded.Invoke();
            };

            avatarObjectLoader.LoadAvatar(ES3FormUser.Load<string>("CustomizeAvatarUrl"));
        }
    }
    private AvatarObjectLoader avatarObjectLoader;
    [SerializeField] private AvatarConfig inGameConfig;

    public  void createSceneForAvatar()
    {
        ResourceCleaner.CleanUp();
        SceneManager.LoadScene("AvatarCreatorWizard");
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        //Debug.Log("ES3FormUser.Load<string>" + ES3FormUser.Load<string >("AvatarUrl"));
#endif

    }
}
