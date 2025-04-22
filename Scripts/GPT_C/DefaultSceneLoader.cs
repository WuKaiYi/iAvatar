using Unity.Multiplayer.Samples.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultSceneLoader : MonoBehaviour
{
    public string defaultSceneName = "DefaultScene";

    /// <summary>
    /// 加載測試場景
    /// </summary>
    public void LoadTestScene(string scence)
    {
        ResourceCleaner.CleanUp();
        SceneLoaderWrapper.Instance.LoadScene(scence, useNetworkSceneManager: true);
        // TODO: 加载测试场景的代码
    }

    private void Start()
    {

    }

    //void Start()
    //{
    //    LoadDefaultScene();
    //}

    //void LoadDefaultScene()
    //{
    //    SceneManager.LoadScene(defaultSceneName);
    //}
}