using UnityEngine;
using Unity.Netcode;

public class CustomObjectController : NetworkBehaviour
{
    [SerializeField]
    private GameObject customLoaderPrefab;

    [SerializeField]
    private GameObject DoorPrefab;
    /// <summary>
    /// 初始化數據
    /// </summary>
    public void InitializeData()
    {
        // TODO: 初始化數據的具體邏輯
        if (IsHost)
        {
            CustomLoader[] customLoaders = FindObjectsOfType<CustomLoader>();

            foreach (CustomLoader customLoader in customLoaders)
            {
                if (customLoader.DynamicEditing)
                {
                    GameObject networkCustomLoader = Instantiate(customLoaderPrefab, customLoader.transform.position, customLoader.transform.rotation);
                    NetworkObject networkObject = networkCustomLoader.GetComponent<NetworkObject>();


                    CustomLoaderNetworkBehaviour networkBehaviour = networkCustomLoader.GetComponent<CustomLoaderNetworkBehaviour>();
                    networkBehaviour.DynamicEditing.Value = customLoader.DynamicEditing;
                    networkBehaviour.DataType.Value = customLoader.dataType;
                    networkBehaviour.Data.Value = customLoader.data;
                    networkBehaviour.InfoData.Value = customLoader.InfoData;
                    networkObject.Spawn();
                    Destroy(customLoader.gameObject);
                }
            }


            RoomDoorTrigger[] roomDoorTriggers = FindObjectsOfType<RoomDoorTrigger>();

            foreach (RoomDoorTrigger roomDoorTrigger in roomDoorTriggers)
            {

                GameObject networkRoomDoorTrigger = Instantiate(DoorPrefab, roomDoorTrigger.transform.position, roomDoorTrigger.transform.rotation);
                //networkRoomDoorTrigger.transform.localScale= roomDoorTrigger.transform.localScale;
                NetworkObject networkObject = networkRoomDoorTrigger.GetComponent<NetworkObject>();
                NetworkDoor networkDoor  = networkRoomDoorTrigger.GetComponent<NetworkDoor>();
                networkDoor.CanOpen.Value = true;
                networkObject.Spawn();
                Destroy(roomDoorTrigger.gameObject);

            }
        }
        else
        {
            RoomDoorTrigger[] roomDoorTriggers = FindObjectsOfType<RoomDoorTrigger>();

            foreach (RoomDoorTrigger roomDoorTrigger in roomDoorTriggers)
            {

              if (!roomDoorTrigger.GetComponentInParent<NetworkDoor>())
                {
                    roomDoorTrigger.transform.parent.gameObject.SetActive(false);
                }
             

            }
        }
    }

    private void Start()
    {
        Invoke("InitializeData", 3);
    }
}