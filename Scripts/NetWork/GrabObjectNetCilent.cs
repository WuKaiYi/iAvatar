using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GrabObjectNetCilent : NetworkBehaviour
{
    public Transform GradPoint;

    bool Graded = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Send(GrabObjectNet grabObjectNet)
    {
        if (GetComponent<NetworkObject>() != null)
        {
            if (grabObjectNet.TargetId.Value != -1&& grabObjectNet.TargetId.Value != (int)GetComponent<NetworkObject>().NetworkObjectId)
                return;


            if (grabObjectNet.TargetId.Value == (int)GetComponent<NetworkObject>().NetworkObjectId)
            {
                Graded = false;
                grabObjectNet.SendID(-1);
            }
            else
            {
                if (!Graded)
                {
                    Graded = true;
                    grabObjectNet.SendID((int)GetComponent<NetworkObject>().NetworkObjectId);
                }             
            }

               
        }
    
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
            return;

        var forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;

        GradPoint.position = this.transform.position + forward*1.5f;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit,20,layerMask))
            {
                GameObject go = hit.collider.gameObject;    //获得选中物体
                string goName = go.name;    //获得选中物体的名字，使用hit.transform.name也可以
                Debug.Log (goName);
                if (go.GetComponent<GrabObjectNet>() != null)
                {
                    Send(go.GetComponent<GrabObjectNet>());
                }

            }
        }
    }
   public  LayerMask layerMask;
}
