using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Unity.Netcode;
using Unity.Collections;

public class GrabObjectNet : NetworkBehaviour
{
    public NetworkVariable<int> TargetId  = new NetworkVariable<int>(-1);

    public NetworkObject networkObject;

   /* public NetworkVariable<bool> State = new NetworkVariable<bool>();*/

    public GameObject Arror;
    public LineRenderer lineRenderer;

    GrabObjectNetCilent onwer;

    public override void OnNetworkSpawn()
    {
        
      /*  State.OnValueChanged += OnStateChanged;*/

        TargetId.OnValueChanged += OnTargetIdChanged;

        

        if (IsHost)
            TargetId.Value = -1;

       
    }

    public override void OnNetworkDespawn()
    {
      /*  State.OnValueChanged -= OnStateChanged;*/

        TargetId.OnValueChanged -= OnTargetIdChanged;
    }

    public void OnTargetIdChanged(int previous, int current)
    {
        if (TargetId.Value == -1)
        {
            Arror.SetActive(false);
            onwer = null;
            lineRenderer.enabled = false;
            return;
        }
           


        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue((ulong)TargetId.Value, out NetworkObject networkObject))
        {
            Debug.Log(networkObject.name);
            Arror.SetActive(true);
            onwer = networkObject.gameObject.GetComponent<GrabObjectNetCilent>();
            lineRenderer.enabled = true;
        }
    }




    public void SendID(int id)
    {
        TargetIdServerRpc(id);
    }


    [ServerRpc(RequireOwnership = false)]
    public void TargetIdServerRpc(int id)
    {
        // this will cause a replication over the network
        // and ultimately invoke `OnValueChanged` on receivers
        TargetId.Value =id;
       

    }

    private void Update()
    {


        if (onwer != null)
        {

            lineRenderer.SetPosition(1, transform.InverseTransformPoint(onwer.transform.position));

            if (IsHost)
            {
                transform.position = Vector3.SmoothDamp(transform.position, onwer.GradPoint.position, ref velocity, 1f, 15);
            }

           
        }
        else
        {
            if (TargetId.Value != -1)
            {
                if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue((ulong)TargetId.Value, out NetworkObject networkObject))
                {
                    Debug.Log(networkObject.name);
                    Arror.SetActive(true);
                    onwer = networkObject.gameObject.GetComponent<GrabObjectNetCilent>();
                    lineRenderer.enabled = true;
                }
            }

        }


    }
    private Vector3 velocity;
}
