using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Netcode;

public class LogData : NetworkBehaviour
{
    private string fileName;
    private string filePath;
    private StreamWriter writer;

    private string userName; // 用户名
    private WaitForSeconds waitTime; // 等待时间

    void Start()
    {
//#if UNITY_EDITOR
//        return;
//#endif 
        if (IsOwner)
        {
            // 设置文件名为Unity开始运行的时间
            fileName = "log_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            // 设置文件保存路径为Unity程序的运行位置
            filePath = Application.streamingAssetsPath + "/Logs/" + ES3FormUser.Load("log_data") + "/" + userName + "_" + fileName;
            Debug.Log("filePath   " + filePath);

            // 创建文件
            writer = new StreamWriter(filePath, true);
            writer.WriteLine("Start logging...");

            waitTime = new WaitForSeconds(0.5f); // 每0.5秒记录一次

            StartCoroutine(LogCoroutine());
        }
        
    }

    IEnumerator LogCoroutine()
    {
        while (true)
        {string json = " {\"position\": \"" + this.transform.position.ToString() + "\", \"datetime\": \"" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + "\"}";
            // 记录物体的移动
            // 假设我们要记录这个物体的位置
            writer.WriteLine(json);
            yield return waitTime;
        }
    }

    void OnDestroy()
    {
        // 在物体被销毁时，关闭文件
        writer.WriteLine("Stop logging...");
        writer.Close();
    }
}