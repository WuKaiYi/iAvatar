using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkDoor : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<bool> m_CanOpen = new NetworkVariable<bool>();
    public NetworkVariable<bool> CanOpen => m_CanOpen;


    public override void OnNetworkSpawn()
    {
        m_CanOpen.OnValueChanged += OnCanOpenChanged;
        if (IsHost )
        {
            //CanOpen.Value = true;
            // Assin the current value based on the current message index value
            //m_Data.Value = m_Messages[m_MessageIndex];
        }
        else
        {
            this.GetComponentInChildren<RoomDoorTrigger>().SetOpen(CanOpen.Value);
            // Subscribe to the OnValueChanged event
            //m_CanOpen.OnValueChanged += OnCanOpenChanged;
            // Log the current value of the text string when the client connected

        }
    }
    private void OnDestroy()
    {
        // 注销状态变化的回调方法
        CanOpen.OnValueChanged -= OnCanOpenChanged;
    }

    public void ToggleCanOpen()
    {
      
        // 通过ServerRpc发送命令，让服务器更新门是否能够开关的状态
        ToggleCanOpenServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ToggleCanOpenServerRpc()
    {
        // 更新门是否能够开关的状态
        CanOpen.Value = !CanOpen.Value;
    }

    private void OnCanOpenChanged(bool previous, bool current)
    {
        this.GetComponentInChildren<RoomDoorTrigger>().SetOpen(current);
        // 处理门是否能够开关的状态变化
        //if (current)
        //{
        //    EnableDoorOpen();
        //}
        //else
        //{
        //    DisableDoorOpen();
        //}
    }

    //private void EnableDoorOpen()
    //{
    //    // 实现启用门开关的逻辑
    //    // ...
    //    Debug.Log("Door open enabled");
    //}

    //private void DisableDoorOpen()
    //{
    //    // 实现禁用门开关的逻辑
    //    // ...
    //    Debug.Log("Door open disabled");
    //}
}
