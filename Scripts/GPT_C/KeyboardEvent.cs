using UnityEngine;
using UnityEngine.Events;

public class KeyboardEvent : MonoBehaviour
{
    public UnityEvent myEvent;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // 这里以按下空格键为例
        {
            myEvent.Invoke();
        }
    }
}