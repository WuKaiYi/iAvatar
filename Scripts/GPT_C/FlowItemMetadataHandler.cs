using DungeonArchitect;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FlowItemMetadataHandler : DungeonItemSpawnListener
{
    
    public override void SetMetadata(GameObject dungeonItem, DungeonNodeSpawnData spawnData)
    {
        //if (SceneManager.GetActiveScene().name == "custon_tool_top_camera")
        //{
        //  //  dungeonItem.GetComponent<ES3.>();
        //}


        if (dungeonItem != null)
        {
            //Debug.Log("spawnData.socket.Id  " + spawnData.socket.Id);
            //Debug.Log("spawnData.socket.SocketType  " + spawnData.socket.SocketType);

            if (spawnData.socket.SocketType == "Door")
            {
                dungeonItem.name = "Door_" + spawnData.socket.Id;
            }
            
        }
    }
}