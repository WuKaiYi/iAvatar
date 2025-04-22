using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadyPlayerMe;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.Events;
using ReadyPlayerMe.Core;

public class ES3AvatarData : MonoBehaviour
{
    public GameObject EasySaveAvatar;
    public Material material;
    [SerializeField]
    private string AvatarURL = "https://d1a370nemizbjq.cloudfront.net/209a1bc2-efed-46c5-9dfd-edc8a1d9cbe4.glb";

    public Texture2D texture;

    public  UnityEvent<GameObject> Loaded;


    // Start is called before the first frame update
    void Start()
    {
      


        if (AvatarURL != "")
        {
            ES3.Save<string>("AvatarUrl", AvatarURL);
        }
        // Cursor.SetCursor(texture, new Vector2(0.5f, 0.5f), CursorMode.ForceSoftware);
#if UNITY_EDITOR
        //var tex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/tex.png");
        //var tex = Application.cu

#endif

        LoadUserName();
        load();
        /*   if (GameObject.Find("Avatar") != null)
           {
               Destroy(GameObject.Find("Avatar"));
           }*/

  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public InputField InputField;
    public TMPro.TMP_InputField TMP_InputField;
    public void LoadUserName()
    {
        if (ES3.KeyExists("Username"))
        {
            if (TMP_InputField != null)
            {
                TMP_InputField.text = (string)ES3FormUser.Load("custom_name");
            }
            if (InputField != null)
            {
                InputField.text = (string)ES3FormUser.Load("custom_name");
            }
        }

           
    }
    public void SaveName(string name)
    {
        ES3.Save("Username", name);
    }
    GameObject avatar;
    public void DontDestroyAvatar()
    {
       /* avatar.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightHand).gameObject.AddComponent<BoxCollider>();
        avatar.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.LeftHand ).gameObject.AddComponent<BoxCollider>();
*/

      //  avatar.name = "Avatar";

            DontDestroyOnLoad(avatar);
    }

    public bool isDontDestroyOnLoad = false;

    public GameObject[] GameObjectAvatars;

    void load()
    {

        if (ES3FormUser.Load<string>("AvatarUrl").Substring(0, 3) == "int")
        {
            GameObject avatar = Instantiate(GameObjectAvatars[int.Parse(Regex.Split(ES3FormUser.Load<string>("AvatarUrl"), "int", RegexOptions.IgnoreCase)[1])]);
            avatar.name = "Avatar" + ES3FormUser.Load<string>("AvatarUrl");
            //avatar.AddComponent<EyeAnimationHandler>();
            //args. Avatar.AddComponent<VoiceHandler>();
            Loaded.Invoke(avatar);
            //avatar.AddComponent<VoiceHandler>();
            return;
        }

        var avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += (sender, args) =>
        {
            Debug.Log($"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]");
            avatar = args.Avatar;
            avatar.AddComponent<EyeAnimationHandler>();
           // avatar.AddComponent<VoiceHandler>();
            Loaded.Invoke(avatar);
        };

        avatarLoader.OnFailed += (sender, args) =>
        {
            Debug.Log(args.Type);
        };

        avatarLoader.LoadAvatar(ES3.Load<string>("AvatarUrl"));
    }
}
