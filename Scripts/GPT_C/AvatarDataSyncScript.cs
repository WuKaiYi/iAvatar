using BestHTTP;
using Unity.Cinemachine;
using Defective.JSON;
using AvatarGroup;
using ReadyPlayerMe.Core;
using SickscoreGames.HUDNavigationSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.BossRoom.Gameplay.Configuration;
using Unity.BossRoom.Gameplay.GameplayObjects.Character;
using Unity.BossRoom.Gameplay.UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static IMBX.ImageLoader;

public class AvatarDataSyncScript : NetworkBehaviour
{  // 自定义空间ID的网络变量
    public NetworkVariable<int> AvatarID = new NetworkVariable<int>();
    public NetworkVariable<int> GroupID = new NetworkVariable<int>();
    public Animator RootAnimator;
    public GameObject[] GameObjectAvatars;
    public string PlayerName;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        Debug.Log("avatar-" + gameObject.name + "  " + AvatarID.Value);
        if (IsHost)
        {
            if (IsOwner)
            {
                AvatarID.Value = ES3.Load<int>("user_id");


            }

            //LoadAvatar(AvatarID.Value);
        }
        if (IsClient && IsOwner && !IsHost)
        {
            SetDataServerRpc(ES3.Load<int>("user_id"));


        }

        AvatarID.OnValueChanged += LoadAvatar;
        LoadAvatar(AvatarID.Value);

        GroupID.OnValueChanged += SetGroup;

        if (IsOwner)
        {
           if (GameObject.FindFirstObjectByType<PathRenderer>() != null)
            {
                GameObject.FindFirstObjectByType<PathRenderer>().playerPoint = this.transform ;
            }


        }
    }
    HUDNavigationSystem navigationSystem;
    /// <summary>
    /// 初始化小地圖
    /// </summary>
    /// <param name="pingCount"></param>
    public void InitializeMiniMap(Transform avatar)
    {
        // TODO: 实现初始化小地图的逻辑
        navigationSystem = GameObject.FindAnyObjectByType<HUDNavigationSystem>();
        navigationSystem.PlayerController = avatar;
    }




    [ServerRpc]
    public void PingServerRpc(int pingCount)
    {

    }
    private void LoadAvatar(int previousValue, int newValue)
    {

        LoadAvatar(newValue);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetDataServerRpc(int id)
    {
        AvatarID.Value = id;
    }
    void LoadAvatarFormWeb(GameObject avatar)
    {

        //avatar.name = "Avatar" + jd["avatar_url"].stringValue;
        avatar.transform.parent = this.transform.GetChild(0);
        avatar.transform.localPosition = Vector3.zero;
        avatar.transform.localEulerAngles = Vector3.zero;

        RootAnimator.avatar = avatar.GetComponent<Animator>().avatar;
        avatar.GetComponent<Animator>().enabled = false;
        if (IsOwner)
        {
            InitializeMiniMap(avatar.transform);
            CinemachineFreeLook cinemachineFreeLook = GameObject.FindAnyObjectByType<CinemachineFreeLook>();
            cinemachineFreeLook.LookAt = (avatar.transform);
            cinemachineFreeLook.Follow = (avatar.transform);
            InitializeRemix(avatar);
            InitializeButton(avatar);
        }

    }

    void InitializeButton(GameObject avatar)
    {
        if (GameObject.Find("ButtonController") == null)
        {
            return;
        }

        GameObject ButtonController = GameObject.Find("ButtonController");
        ButtonController.GetComponent<ButtonController>().Animator = RootAnimator;
    }

    void InitializeRemix(GameObject avatar)
    {
        if (GameObject.Find("Remix") == null)
        {
            return;
        }
        GameObject remix = GameObject.Find("Remix");

        remix.GetComponent<RemixGetData>().animator = RootAnimator;
        remix.GetComponent<RemixPosture>().animator = RootAnimator;
        remix.GetComponent<RemixEotion>().animator = RootAnimator;


        //remix.GetComponent<RemixGetData>().enabled = true ;
        //remix.GetComponent<RemixPosture>().enabled = true;
        //remix.GetComponent<RemixEotion>().enabled = true;
    }
    /// <summary>
    /// 初始化用戶頭像
    /// </summary>
    public void InitializeUserAvatar(string url)
    {
        try
        {
            RawImage rawImage = GameObject.Find("FigmaUI/LE main/user/Group 44753/Group 44814_1").GetComponent<RawImage>();
            IMBX.ImageLoader imageLoader = IMBX.ImageLoader.Create();
            imageLoader.Load(0, url, "", "", CacheMode.NoCache, (texture, index) =>
            {
                if (texture != null)
                {
                    rawImage.texture = texture;
                }
            }, 0, 10);
        }
        catch
        {

        }
     

    }
    [SerializeField]
    ServerCharacter m_ServerCharacter;
    //public void AvatarMovement(Vector3 pos)
    //{
    //    m_ServerCharacter.SendCharacterInputServerRpc(pos);

    //}

    void SetGroup(int previousValue, int newValue)
    {
        this.GetComponent<LearningverseUIStateDisplayHandler>().m_UIState.ChangeColor(newValue);
    }




    GameObject avatar;
    public  AvatarConfig inGameConfig;

    void LoadAvatar(int id)
    {
        Debug.Log("avatar-" + id);
        if (id == 0)
        {
            return;
        }
        var request = new HTTPRequest(new Uri(ServerConfig.host + "/api/get_user/" + id), HTTPMethods.Get, onFinished);
        Debug.Log(request.Uri);
        request.Send();

        void onFinished(HTTPRequest originalRequest, HTTPResponse response)
        {
            Debug.Log(response.DataAsText);
            JSONObject jd = new JSONObject(response.DataAsText);


            PlayerName = jd["custom_name"].stringValue;
            //if (IsOwner)
            //{
            //    GameObject.FindFirstObjectByType<ChatManager>().LoginToVivox(PlayerName);
            //}
            this.GetComponent<LearningverseUIStateDisplayHandler>().DisplayUIName(PlayerName);
            this.GetComponent<LearningverseUIStateDisplayHandler>().m_UIState.ChangeColor(jd["group_id"].intValue);
            if (IsHost)
            {
                int count = 1;
                var newItems = new MyListItemModel[count];

                for (int i = 0; i < count; ++i)
                {
                    var model = new MyListItemModel()
                    {
                        name = PlayerName,
                        group = GroupID.Value
                    };
                    newItems[i] = model;
                }
                //GameObject.FindAnyObjectByType<BasicListAdapterGroup>().AddItemsAt(0, newItems);
            }

            if (jd["avatar_url"].stringValue.Substring(0, 3) == "int")
            {
                avatar = Instantiate(GameObjectAvatars[int.Parse(Regex.Split(jd["avatar_url"].stringValue, "int", RegexOptions.IgnoreCase)[1])]);
                LoadAvatarFormWeb(avatar);
                if (IsOwner)
                {
                    InitializeUserAvatar(jd["avatar_pic"].stringValue);
                }

            }
            else
            {
                if (IsOwner)
                {
                    InitializeUserAvatar(jd["avatar_pic"].stringValue);
                }
                var avatarObjectLoader = new AvatarObjectLoader();
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
                    //avatar.gameObject.transform.Find("Renderer_Avatar").name = "Renderer_Head";
                    AvatarAnimationHelper.SetupAnimator(args.Metadata, avatar);
               
                    LoadAvatarFormWeb(avatar);
                };

                avatarObjectLoader.LoadAvatar(jd["avatar_url"].stringValue);

            }
        }


    }
}
