using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleGroupHandler : MonoBehaviour
{
    public ToggleGroup toggleGroup; // 拖拽Toggle Group到此处
    public TMP_InputField tmpInputField; // 拖拽TMP输入框到此处
    public TTSSender tTSSender;
    void Start()
    {
        // 获取Toggle Group中的所有Toggle
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();

        // 为每个Toggle添加监听事件
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate {
                OnToggleValueChanged(toggle);
            });
        }
    }

    void OnToggleValueChanged(Toggle changedToggle)
    {
        if (changedToggle.isOn)
        {
            // 根据Toggle的不同状态触发不同函数
            switch (changedToggle.transform.GetSiblingIndex())
            {
                case 0:
                    FunctionForToggle1();
                    break;
                case 1:
                    FunctionForToggle2();
                    break;
                case 2:
                    FunctionForToggle3();
                    break;
            }
            tmpInputField.gameObject.SetActive (true );
            // 激活TMP输入框
            //tmpInputField.ActivateInputField();
        }
    }

    void FunctionForToggle1()
    {
        tTSSender.Text_lang = "yue";
        Debug.Log("Toggle 1 selected");
        // 您可以在此处添加具体的功能代码
    }

    void FunctionForToggle2()
    {
        tTSSender.Text_lang = "zh";
        Debug.Log("Toggle 2 selected");
        // 您可以在此处添加具体的功能代码
    }

    void FunctionForToggle3()
    {
        tTSSender.Text_lang = "en";
        Debug.Log("Toggle 3 selected");
        // 您可以在此处添加具体的功能代码
    }
}
