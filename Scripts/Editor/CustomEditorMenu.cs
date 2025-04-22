using UnityEditor;
using UnityEngine;

public class CustomEditorMenu : MonoBehaviour
{
    [MenuItem("GameObject/執行初始化", false, 0)]
    static void InitializeComponent()
    {
        // 获取当前选中的物体
        GameObject selectedObject = Selection.activeGameObject;

        // 检查是否有选中物体
        if (selectedObject == null)
        {
            Debug.LogWarning("No object selected!");
            return;
        }

        // 获取选中物体上的组件
        Component component = selectedObject.GetComponent<Component>();

        // 检查是否有组件
        if (component == null)
        {
            Debug.LogWarning("No component found on the selected object!");
            return;
        }

        // 执行组件的初始化函数
        component.SendMessage("Start", SendMessageOptions.RequireReceiver);
    }
}