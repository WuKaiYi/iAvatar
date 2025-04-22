using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CMFreelookOnlyWhenRightMouseDown : MonoBehaviour
{


    private CinemachineFreeLook cinemachineFreeLook;
    private CinemachinePOV cinemachineCinemachinePOV;
    [SerializeField] private float rotationSpeed = 2f;

    private void Start()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        cinemachineCinemachinePOV = cinemachineFreeLook.GetRig(2).GetCinemachineComponent<CinemachinePOV>();
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }
    public float v;
    private void Update()
    {
        //cinemachineCinemachinePOV.m_VerticalAxis.Value = cinemachineFreeLook.m_YAxis.Value;

    }
    private float GetAxisCustom(string axisName)
    {
        if (cinemachineFreeLook.m_YAxis.Value < 0.5f && UnityEngine.Input.GetAxis("Mouse ScrollWheel") < 0 && cinemachineFreeLook.m_YAxis.Value == 0)
        {
            //cinemachineCinemachinePOV.m_HorizontalAxis.Value = cinemachineFreeLook.m_XAxis.Value + 180;
            cinemachineFreeLook.m_XAxis.Value = cinemachineCinemachinePOV.m_HorizontalAxis.Value;
            cinemachineFreeLook.m_YAxis.Value = 0.5f;

            return 0;
        }

        if (cinemachineFreeLook.m_YAxis.Value < 0.5f && UnityEngine.Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cinemachineCinemachinePOV.m_HorizontalAxis.Value = cinemachineFreeLook.m_XAxis.Value;
            cinemachineFreeLook.m_YAxis.Value = 0;
            return 0;
        }

        if (cinemachineFreeLook.m_YAxis.Value == 0)
        {
            //cinemachineCinemachinePOV.m_HorizontalAxis.Value = cinemachineFreeLook.m_XAxis.Value + 180;
            if (axisName == "POV X")
            {
                if (Input.GetMouseButton(1))
                {
                    return UnityEngine.Input.GetAxis("Mouse X");
                }
                else
                {
                    return 0;
                }
            }
            else if (axisName == "POV Y")
            {
                if (Input.GetMouseButton(1))
                {
                    return UnityEngine.Input.GetAxis("Mouse Y");
                }
                else
                {
                    return 0;
                }
            }
        }
        else
        {
            if (cinemachineFreeLook.m_YAxis.Value < 0.7f && cinemachineFreeLook.m_YAxis.Value >= 0.5f)
            {
                if (Input.GetMouseButton(1))
                {
                    // 获取鼠标上下移动的值
                    float mouseY = Input.GetAxis("Mouse Y");

                    //// 根据鼠标移动值计算新的lookAt物体位置
                    //Vector3 newPosition = cinemachineFreeLook.LookAt.localPosition + new Vector3(0f, mouseY * 0.1f, 0f);
                    //// 限制lookAt物体在一定范围内移动
                    //newPosition.y = Mathf.Clamp(newPosition.y, 0.5f, 4f);

                    //// 更新lookAt物体的本地位置
                    //cinemachineFreeLook.LookAt.localPosition = newPosition;

                }
            }
            else
            {
                //cinemachineFreeLook.LookAt.localPosition =Vector3.up* 1.8f;
            }





            if (axisName == "Mouse X")
            {
                if (Input.GetMouseButton(1))
                {
                    return UnityEngine.Input.GetAxis("Mouse X") * rotationSpeed;
                }


                else if (Input.GetKey(KeyCode.Q))
                {
                    return 0.5f * rotationSpeed;
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    return -0.5f * rotationSpeed;
                }

                else if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);

                    // 检查触控开始
                    if (touch.phase == TouchPhase.Began)
                    {
                        touchStartTime = Time.time;
                        touchLongPressed = false;
                    }

                    // 检测触控长按
                    if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                    {
                        if (!touchLongPressed && Time.time - touchStartTime > longPressDuration)
                        {
                            touchLongPressed = true;
                        }
                    }

                    // 检测长按且移动
                    if (touchLongPressed && touch.phase == TouchPhase.Moved)
                    {
                        // 计算触控的方向
                        Vector2 touchDelta = touch.deltaPosition;

                        // 根据屏幕尺寸进行归一化
                        horizontal = -touchDelta.x;
                        vertical = touchDelta.y;

                        // 根据需求进一步处理horizontal和vertical
                        // 比如，可以用它们来调整相机的视角
                        return horizontal;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else if (axisName == "Mouse Y")
            {
                if (Input.GetMouseButton(1))
                {
                    return UnityEngine.Input.GetAxis("Mouse Y") * rotationSpeed;
                }
                else
                {
                    return 0;
                }
            }
            else if (axisName == "POV X")
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    return -1f * rotationSpeed;
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    return 1f * rotationSpeed;
                }
                else
                {
                    return 0;
                }
            }
            else if (axisName == "POV Y")
            {
                return 0;
            }
        }

        return UnityEngine.Input.GetAxis(axisName);
    }
    // 长按时间限制
    private float longPressDuration = 0.2f;

    // 记录触控开始时间
    private float touchStartTime = 0;

    // 用于指示是否已检测到长按
    private bool touchLongPressed = false;

    // 水平和竖直方向
    private float horizontal = 0;
    private float vertical = 0;
}