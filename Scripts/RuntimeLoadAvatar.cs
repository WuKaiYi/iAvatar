using ReadyPlayerMe;
using ReadyPlayerMe.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeLoadAvatar : MonoBehaviour
{
    public bool debug = false;
    public Material material;

    public bool isDontDestroyOnLoad = false;

    public bool EasySave;
    public GameObject EasySaveAvatar;

    [SerializeField]
    private string AvatarURL = "https://d1a370nemizbjq.cloudfront.net/209a1bc2-efed-46c5-9dfd-edc8a1d9cbe4.glb";


    private void Start()
    {
        if (GameObject.Find("Avatar") != null)
        {
            Destroy(GameObject.Find("Avatar"));
        }

        Debug.Log($"Started loading avatar. [{Time.timeSinceLevelLoad:F2}]");
        AvatarObjectLoader avatarLoader = new AvatarObjectLoader();

        if (debug)
        {

          //  avatarLoader.LoadAvatar(AvatarURL, OnAvatarImported, OnAvatarLoaded);
        }
        else
        {
          //  avatarLoader.LoadAvatar(PlayerPrefs.GetString("avatarlink", ""), OnAvatarImported, OnAvatarLoaded);
        }

        if (EasySave)
        {
            load();
        }
    }

    void load()
    {
        var avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += (sender, args) =>
        {
            Debug.Log($"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]");
            Texture texture = args.Avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.mainTexture;


            Material material_t = args.Avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
            material_t.shader = material.shader;
            material_t.CopyPropertiesFromMaterial(material);
            material_t.mainTexture = texture;

            args.Avatar.layer = LayerMask.NameToLayer("Avatar");
            foreach (Transform child in args.Avatar.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer("Avatar");
            }



            args.Avatar.transform.GetChild(0).name = AvatarURL;

            EasySaveAvatar.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = args.Avatar.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        };

        avatarLoader.OnFailed += (sender, args) =>
        {
            Debug.Log(args.Type);
        };

        avatarLoader.LoadAvatar(AvatarURL);
    }

    private void OnAvatarImported(GameObject avatar)
    {
        Debug.Log($"Avatar imported. [{Time.timeSinceLevelLoad:F2}]");
    }

    private void OnAvatarLoaded(GameObject avatar, AvatarMetadata metaData)
    {
        Debug.Log($"Avatar loaded. [{Time.timeSinceLevelLoad:F2}]\n\n{metaData}");

        Texture texture = avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.mainTexture;

        Material material_t = new Material(material);
        //   material_t.shader=material.shader;
        material_t.CopyPropertiesFromMaterial(material);
        material_t.mainTexture = texture;
        avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = material_t;
        AvatarInfo avatarInfo = avatar.AddComponent<AvatarInfo>();
        avatarInfo.Url = PlayerPrefs.GetString("avatarlink", "");
        avatar.name = "Avatar";

        if (isDontDestroyOnLoad)
            DontDestroyOnLoad(avatar);

    }
}
