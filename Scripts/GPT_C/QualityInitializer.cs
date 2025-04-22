using UnityEngine;
using UnityEngine.SceneManagement;

public class QualityInitializer : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR
        return;
#endif

#if UNITY_IOS || UNITY_ANDROID
        QualitySettings.SetQualityLevel(0);
        return;
#endif

        // 读取画质配置文件的路径
        string filePath = Application.streamingAssetsPath + "/Quality.txt";

        // 使用StreamReader读取文件内容
        System.IO.StreamReader reader = new System.IO.StreamReader(filePath);
        string qualitySetting = reader.ReadLine(); // 仅读取第一行数字内容
        reader.Close();

        // 将读取到的内容转换为整数
        int qualityLevel;
        if (int.TryParse(qualitySetting, out qualityLevel))
        {
            // 设置画质等级
            QualitySettings.SetQualityLevel(qualityLevel);
        }
        else
        {
            Debug.LogError("无法解析画质配置文件中的内容为整数！");
        }
    }
}