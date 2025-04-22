using ReadyPlayerMe;
using ReadyPlayerMe.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.BossRoom;
using UnityEngine;

public class AvatarLoadNetwork : MonoBehaviour
{
    public Material material;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void LoadAvatar(string url)
    {


        //avatarLoader.LoadAvatar(charSelectData.LobbyPlayers[i].AvatarUrl, OnAvatarImported, OnAvatarLoaded);


        var avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += (sender, args) =>
        {

            Debug.Log($"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]");

        };

        avatarLoader.OnFailed += (sender, args) =>
        {
            Debug.Log(args.Type);
        };
        avatarLoader.LoadAvatar(url);
    }

    //public void LoadAvatar(CharSelectData charSelectData, int i, bool[] SetSwitchs, Material material, Transform[] Sets)
    //{
    //    if (SetSwitchs[i])
    //        return;

    //    Debug.Log(charSelectData.LobbyPlayers[i].PlayerName);

    //    //avatarLoader.LoadAvatar(charSelectData.LobbyPlayers[i].AvatarUrl, OnAvatarImported, OnAvatarLoaded);


    //    var avatarLoader = new AvatarObjectLoader();
    //    avatarLoader.OnCompleted += (sender, args) =>
    //    {
    //        SetSwitchs[i] = true;
    //        Debug.Log($"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]");
    //        Texture texture = args.Avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.mainTexture;


    //        Material material_t = args.Avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
    //        material_t.shader = material.shader;
    //        material_t.CopyPropertiesFromMaterial(material);
    //        material_t.mainTexture = texture;

    //        args.Avatar.layer = LayerMask.NameToLayer("Avatar");
    //        foreach (Transform child in args.Avatar.GetComponentsInChildren<Transform>())
    //        {
    //            child.gameObject.layer = LayerMask.NameToLayer("Avatar");
    //        }


    //        //  args.Avatar.name = "Avatar" + i;

    //        args.Avatar.transform.position = Sets[i].position;

    //        args.Avatar.transform.GetChild(0).name = charSelectData.LobbyPlayers[i].AvatarUrl;

    //        Destroy(this.gameObject);

    //    };

    //    avatarLoader.OnFailed += (sender, args) =>
    //    {
    //        Debug.Log(args.Type);
    //    };
    //    avatarLoader.LoadAvatar(charSelectData.LobbyPlayers[i].AvatarUrl);
    //}

    // Update is called once per frame
    void Update()
    {

    }
}
