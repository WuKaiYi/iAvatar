using UnityEngine;
using UnityEngine.UI;

public static class UIInitializer
{
    public static T FindComponent<T>(string path)
    {
        // 在指定的路径中查找组件
        // 返回找到的组件，如果找不到则返回默认值
        return GameObject.Find(path).GetComponent<T>();
    }
}