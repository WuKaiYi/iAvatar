using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class runtimeModelDownload : MonoBehaviour
{
    ES3Settings settings;
    groupModelUpload[] uploads;
    // Start is called before the first frame update
    void Start()
    {
    uploads = FindObjectsOfType<groupModelUpload>();
        settings = new ES3Settings();

        settings.path = ES3.Load("user_id").ToString()+"/StartClub";
        Debug.Log(settings.FullPath + " ES3AutoSaveMgr.Current.settings.path ");

      if (ES3.KeyExists("runtimeModel",settings))
        {
            groupModelUpload[] online_uploads = ES3.Load<groupModelUpload[]>("runtimeModel", settings);

            for (int i = 0; i < online_uploads.Length; i++)
            {
                Debug.Log("dowloading" + uploads[i].GlbUrl);
                uploads[i].GlbUrl= online_uploads[i].GlbUrl;
                if (!uploads[i].GlbUrl.IsNullOrEmpty())
                {
                   
                    uploads[i].LoadModel3D(uploads[i].GlbUrl);
                }
            }

        }


    }

    public void ExitView()
    {
        for (int i = 0; i < uploads.Length; i++)
        {
            uploads[i].CameraPivot.SetActive(false);
        }
    }

    public void UploadModels()
    {
        ES3.Save<groupModelUpload[]>("runtimeModel", uploads, settings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
