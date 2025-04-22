using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
   

    private void Start()
    {
        // 获取主摄像机
      
    }

    private void Update()
    {
        if (GameObject.FindAnyObjectByType<CustomObjectController>() != null)
        {
            return;
        }

            // 检测鼠标点击事件
            if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 发射射线并检测是否击中物体
            if (Physics.Raycast(ray, out hit))
            {Debug.DrawLine(hit.point, hit.normal);
                Debug.Log(hit.collider.gameObject.name);
                // 检查是否点击到名为"DoorTrigger"的物体
                if (hit.collider.gameObject.GetComponent<RoomDoorTrigger>()!=null )
                {
                    hit.collider.gameObject.GetComponent<RoomDoorTrigger>().ToggleDoor();
                }
            }
        }
    }


}