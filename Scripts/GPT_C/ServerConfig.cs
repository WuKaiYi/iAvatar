
using BestHTTP;
using Defective.JSON;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class ServerConfig
{
    public static string host = "http://learningverse.hkicai.com:8771";


    public string GetHost()
    {
        return host;
    }

    public static string ParseUnicode(string unicodeString)
    {
        string parsedString = Regex.Unescape(unicodeString);
        parsedString = ConvertChineseComma(parsedString);
        return parsedString;
    }
    /// <summary>
    /// 一個處理中文逗號 轉換為逗號加兩個空格的函數
    /// </summary>
    public static string ConvertChineseComma(string input)
    {
        return input.Replace("，", ",  ");
    }
    //編寫textrue 轉textrue2D 的腳本

    public static Texture2D ConvertToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);
        return texture2D;
    }


    public static void UploadFile(Texture2D texture2D, OnRequestFinishedDelegate finishedDelegate)

    {
        string fileType = "image/png";
        var request = new HTTPRequest(new Uri(ServerConfig.host + "/api/Upload"), HTTPMethods.Post, finishedDelegate);
        request.AddField("user_id", ES3.Load("user_id").ToString());
        request.AddBinaryData("file", texture2D.EncodeToPNG(), "image", fileType);
        //request.AddField("file", ES3AutoSaveMgr.Current.settings.FullPath);
        request.Send();
    }
    public static void UploadFile(string path, OnRequestFinishedDelegate finishedDelegate)

    {
        string fileName = Path.GetFileName(path);
        string fileType = "";
        if (fileName.EndsWith(".png"))
        {
            fileType = "image/png";
        }
        else if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg"))
        {
            fileType = "image/jpeg";
        }
        else if (fileName.EndsWith(".gif"))
        {
            fileType = "image/gif";
        }


        Debug.Log("UploadFile  " + path + "  " + fileName + "  " + fileType);
        var request = new HTTPRequest(new Uri(ServerConfig.host + "/api/Upload"), HTTPMethods.Post, finishedDelegate);
        request.AddField("user_id", ES3.Load("user_id").ToString());

        request.AddBinaryData("file", File.ReadAllBytes(path), fileName, fileType);
        //request.AddField("file", ES3AutoSaveMgr.Current.settings.FullPath);
        request.Send();
    }
    public static void UploadRawfile(string path, OnRequestFinishedDelegate finishedDelegate)

    {
        string fileName = Path.GetFileName(path);
        string fileType = "";
        if (fileName.EndsWith(".png"))
        {
            fileType = "image/png";
        }
        else if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg"))
        {
            fileType = "image/jpeg";
        }
        else if (fileName.EndsWith(".gif"))
        {
            fileType = "image/gif";
        }
        else if (fileName.EndsWith(".txt"))
        {
            fileType = "text/plain";
        }

        Debug.Log("UploadFile  " + path + "  " + fileName + "  " + fileType);
        var request = new HTTPRequest(new Uri(ServerConfig.host + "/api/UploadRawfile"), HTTPMethods.Post, finishedDelegate);
        request.AddField("user_id", ES3.Load("user_id").ToString());

        request.AddBinaryData("file", File.ReadAllBytes(path), fileName, fileType);
        //request.AddField("file", ES3AutoSaveMgr.Current.settings.FullPath);
        request.Send();
    }
    //unity 中編寫一個方法 傳入 文件的路徑( string) 返回 文件名(string)  例如http://galaxycao.asuscomm.com:7771/upload/11/1111.txt 返回1111.txt

    public static string GetFileNameFromPath(string filePath)
    {
        string[] pathParts = filePath.Split('/');
        string fileName = pathParts[pathParts.Length - 1];
        return fileName;
    }
 


}

