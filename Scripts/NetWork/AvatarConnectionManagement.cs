using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ReadyPlayerMe;
using Unity.Multiplayer.Samples.BossRoom.Client;
using Unity.Multiplayer.Samples.BossRoom;
using Unity.Netcode;
using System;
using ReadyPlayerMe.Core;

public class AvatarConnectionManagement : MonoBehaviour
{
    //public CharSelectData charSelectData;
    public Material material;
    AvatarObjectLoader avatarLoader;

    public Transform[] Sets;
    public bool[] SetSwitchs;
    // Start is called before the first frame update
    void Start()
    {



    }
    public void Setsets()
    {
        Sets = GameObject.Find("AvatarSets").transform.GetComponentsInChildren<Transform>();
    }

    //IEnumerator avatarload(CharSelectData charSelectData)
    //{
    //    string url;
    //    bool done = false;
    //    for (int i = 0; i < charSelectData.LobbyPlayers.Count; i++)
    //    {
    //        done = false;
    //        Debug.Log(charSelectData.LobbyPlayers[i].PlayerName);
    //        url = charSelectData.LobbyPlayers[i].AvatarUrl;
    //        //avatarLoader.LoadAvatar(charSelectData.LobbyPlayers[i].AvatarUrl, OnAvatarImported, OnAvatarLoaded);


    //        var avatarLoader = new AvatarObjectLoader();
    //        avatarLoader.OnCompleted += (sender, args) =>
    //        {
    //            SetSwitchs[i] = true;
    //            Debug.Log($"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]");
    //            Texture texture = args.Avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.mainTexture;


    //            Material material_t = args.Avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
    //            material_t.shader = material.shader;
    //            material_t.CopyPropertiesFromMaterial(material);
    //            material_t.mainTexture = texture;

    //            args.Avatar.layer = LayerMask.NameToLayer("Avatar");
    //            foreach (Transform child in args.Avatar.GetComponentsInChildren<Transform>())
    //            {
    //                child.gameObject.layer = LayerMask.NameToLayer("Avatar");
    //            }


    //            // args.Avatar.name = "Avatar" + i;

    //            args.Avatar.transform.position = Sets[i].position;

    //            args.Avatar.transform.GetChild(0).name = url;
    //            done = true;


    //        };

    //        avatarLoader.OnFailed += (sender, args) =>
    //        {
    //            Debug.Log(args.Type);
    //        };
    //        if (SetSwitchs[i])
    //        {
    //            yield return 0;
    //        }
    //        else
    //        {
    //            avatarLoader.LoadAvatar(charSelectData.LobbyPlayers[i].AvatarUrl);
    //            yield return SetSwitchs[i];
    //        }



    //    }

    //}

    //public void OnListChanged(CharSelectData charSelectData)
    //{

    //    avatarLoader = new AvatarObjectLoader();
    //    int localPlayerIdx = -1;
    //    for (int i = 0; i < charSelectData.LobbyPlayers.Count; ++i)
    //    {
    //        if (charSelectData.LobbyPlayers[i].ClientId == NetworkManager.Singleton.LocalClientId)
    //        {
    //            localPlayerIdx = i;
    //            break;
    //        }
    //    }

    //    for (int i = 0; i < charSelectData.LobbyPlayers.Count; i++)
    //    {
    //        GameObject al = new GameObject();
    //        al.name = "al " + i;
    //        AvatarLoadNetwork avatarLoadNetwork = al.AddComponent<AvatarLoadNetwork>();
    //        avatarLoadNetwork.LoadAvatar(charSelectData, i, SetSwitchs, material, Sets);



    //    }
    //    //  StartCoroutine(avatarload(charSelectData));

    //}



    //public void onSetNewAvatar(ulong clientId)
    //{
    //    string url;
    //    avatarLoader = new AvatarObjectLoader();
    //    for (int i = 0; i < charSelectData.LobbyPlayers.Count; i++)
    //    {
    //        if (charSelectData.LobbyPlayers[i].ClientId == clientId)
    //        {
    //            Debug.Log(charSelectData.LobbyPlayers[i].PlayerName);
    //            url = charSelectData.LobbyPlayers[i].AvatarUrl;
    //            //avatarLoader.LoadAvatar(charSelectData.LobbyPlayers[i].AvatarUrl, OnAvatarImported, OnAvatarLoaded);

    //            var avatarLoader = new AvatarObjectLoader();
    //            avatarLoader.OnCompleted += (sender, args) =>
    //            {
    //                Debug.Log($"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]");
    //                Texture texture = args.Avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.mainTexture;


    //                Material material_t = args.Avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
    //                material_t.shader = material.shader;
    //                material_t.CopyPropertiesFromMaterial(material);
    //                material_t.mainTexture = texture;

    //                args.Avatar.layer = LayerMask.NameToLayer("Avatar");
    //                foreach (Transform child in args.Avatar.GetComponentsInChildren<Transform>())
    //                {
    //                    child.gameObject.layer = LayerMask.NameToLayer("Avatar");
    //                }


    //                //  args.Avatar.name = "Avatar" + clientId;

    //                args.Avatar.transform.position = Sets[clientId].position;

    //                args.Avatar.transform.GetChild(0).name = url;
    //            };

    //            avatarLoader.OnFailed += (sender, args) =>
    //            {
    //                Debug.Log(args.Type);
    //            };

    //            avatarLoader.LoadAvatar(charSelectData.LobbyPlayers[i].AvatarUrl);
    //        }
    //    }
    //}

    // Update is called once per frame
    void Update()
    {


    }
    private void OnAvatarImported(GameObject avatar)
    {
        Debug.Log($"Avatar imported. [{Time.timeSinceLevelLoad:F2}]");
    }
    private void OnAvatarLoaded(GameObject avatar, AvatarMetadata metaData)
    {
        Debug.Log($"Avatar loaded. [{Time.timeSinceLevelLoad:F2}]\n\n{metaData}");

        Texture texture = avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.mainTexture;
        avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = material;
        material.mainTexture = texture;

        //AvatarInfo avatarInfo = avatar.AddComponent<AvatarInfo>();
        //avatarInfo.Url = PlayerPrefs.GetString("avatarlink", "");



    }
}
