using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class RoomDoorTrigger : MonoBehaviour
{
    [SerializeField]
    private RoomDoor[] Doors;
    [SerializeField]
    private Transform[] DoorsTR;
    [SerializeField]
    private Transform[] DoorsTL;
    private int AgentsInRange = 0;

    public bool Open;
    public GameObject[] Lights;
    //public bool CanOpen;

    private void Awake()
    {
        SetOpen(Open);
    }
    /// <summary>
    /// 用bool切換門是否能被打開
    /// </summary>
    public void ToggleDoor()
    {
        Open = !Open;
        SetOpen(Open);
        //  UpdateDoorUI(Open);
    }
    /// <summary>
    /// 更新門的UI

    /// </summary>
    public void UpdateDoorUI(bool open)
    {
        if (Open)
        {
            Lights[0].SetActive(true);
            Lights[1].SetActive(false);

        }
        else
        {
            Lights[1].SetActive(true);
            Lights[0].SetActive(false);
        }
        // TODO: 实现更新门的UI的代码
    }



    private void OnTriggerEnter(Collider other)
    {
        if (!Open)
        {

            return;
        }
        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {

            AgentsInRange++;

            foreach (RoomDoor Door in Doors)
            {
                if (!Door.IsOpen)
                {
                    Door.Open(other.transform.position);
                }
            }

            foreach (Transform Door in DoorsTR)
            {
               
                    Door.transform.DOLocalMoveZ(0.91f, 2f);
            }
            foreach (Transform Door in DoorsTL)
            {
              
                    Door.transform.DOLocalMoveZ(-0.91f, 2f);
            }
        }
    }

    public void SetOpen(bool v)
    {


        Open = v;
        Lights[0].SetActive(v);
        Lights[1].SetActive(!v);


        //  this.GetComponent<BoxCollider>().enabled = v;
        foreach (RoomDoor Door in Doors)
        {
            Door.GetComponent<NavMeshObstacle>().enabled = !v;
            Door.GetComponent<NavMeshObstacle>().carving = !v;
            //Door.GetComponent<NavMeshObstacle>().carving = v;
            //if (!Door.IsOpen)
            //{
            //    Door.GetComponent<NavMeshObstacle>().carving = v;
            //}
            if (!v)
            {
                //Door.Close();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!Open)
        {

            return;
        }
        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {

            // if you do not want to automatically close doors, do not implement this method

            /*如果您不想自动关闭门，请不要实现此方法。*/
            AgentsInRange--;
            foreach (RoomDoor Door in Doors)
            {
                if (Door.IsOpen && AgentsInRange == 0)
                {
                    Door.Close();
                }
            }

            foreach (Transform Door in DoorsTR)
            {
                if (AgentsInRange == 0)
                    Door.transform.DOLocalMoveZ(0f, 2f);
            }
            foreach (Transform Door in DoorsTL)
            {
                if (AgentsInRange == 0)
                    Door.transform.DOLocalMoveZ(0f, 2f);
            }
            Debug.Log("AgentsInRange  "+ AgentsInRange);
        }
    }
}