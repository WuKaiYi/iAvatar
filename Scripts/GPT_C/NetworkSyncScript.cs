
using Unity.Netcode;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class NetworkSyncScript : NetworkBehaviour
{
    // 自定义空间ID的网络变量
    public NetworkVariable<int> customSpaceID = new NetworkVariable<int>();

    private void Start()
    {
        //// 在服务器上设置初始值
        //if (IsServer)
        //{
        //    customSpaceID.Value = ES3FormUser.Load<int>("CustomSpaceId"); ;
        //}
    }
    CustomSpaceLoader  customSpaceLoader;
    public override void OnNetworkSpawn()
    {
        Debug.Log("OnNetworkSpawn");
        customSpaceLoader = this.GetComponent<CustomSpaceLoader>();
       

        //customSpaceID.OnValueChanged += customSpaceLoader.LoadCustomSpace;

        if (IsServer)
        {
            customSpaceID.Value = ES3FormUser.Load<int>("CustomSpaceId"); ;
        }
        if (IsClient)
        {
           
        }
        Debug.Log(customSpaceID.Value);
        customSpaceLoader.LoadCustomSpace(customSpaceID.Value);
    }
    private void Update()
    {
        // 只有在服务器上才能更改值
        if (IsServer)
        {
            // 在这里编写更新customSpaceID的逻辑
            // 例如：customSpaceID.Value = 新的空间ID;
        }
    }
}