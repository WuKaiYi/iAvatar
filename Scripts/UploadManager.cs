using BestHTTP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UploadManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string file_path;
    public void Upload()
    {
       
        var request = new HTTPRequest(new Uri("http://galaxycao.asuscomm.com:7771/upload/fileUpload"), HTTPMethods.Post);

        byte[] data = File.ReadAllBytes(file_path);
       

        request.AddBinaryData("file", data, Path.GetFileName(file_path));
        request.OnUploadProgress = OnUploadProgress;
        request.Send();

        void OnUploadProgress(HTTPRequest request, long uploaded, long length)
        {
            float progressPercent = (uploaded / (float)length) * 100.0f;
            Debug.Log("Uploaded: " + progressPercent.ToString("F2") + "%");
        }
    }
}
