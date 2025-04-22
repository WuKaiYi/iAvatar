using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DoorInteractionNet : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsHost)
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
            {
                Debug.DrawLine(hit.point, hit.normal);
                Debug.Log(hit.collider.gameObject.name);
                // 检查是否点击到名为"DoorTrigger"的物体
                if (hit.collider.gameObject.GetComponentInParent<NetworkDoor>() != null)
                {
                    hit.collider.gameObject.GetComponentInParent<NetworkDoor>().ToggleCanOpen();
                }
            }
        }
    }
}
