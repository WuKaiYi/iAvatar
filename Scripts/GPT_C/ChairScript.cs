using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ChairScript : MonoBehaviour
{
    public Transform target; // 椅子的位置
    public Animator animator; // 角色的动画控制器
    private NavMeshAgent agent; // 寻路代理
    public float rotationSpeed = 5f; // 旋转速度
    public NetworkAnimator animator_net; // 角色的动画控制器
    private void Start()
    {

        if (SceneManager.GetActiveScene().name == "custon_tool_top_camera" || SceneManager.GetActiveScene().name == "DungeonLayout")
        {
            agent = GameObject.FindFirstObjectByType<NavMeshAgent>();
            animator = agent.GetComponent<Animator>();
        }
        else
        {
         
        }
    }

    public void MoveToChair()
    {
        if (SceneManager.GetActiveScene().name == "custon_tool_top_camera" || SceneManager.GetActiveScene().name == "DungeonLayout")
        {
            StartCoroutine(MoveToChairCoroutine());
        }
        else
        {
            StartCoroutine(MoveToChairCoroutineNet());
        }
           
    }
    GameObject avatar;
    AvatarDataSyncScript dataSyncScript;
    private IEnumerator MoveToChairCoroutineNet()
    {
        Debug.Log("MoveToChairCoroutineNet");
        foreach (AvatarDataSyncScript gameObject in GameObject.FindObjectsByType<AvatarDataSyncScript>( FindObjectsSortMode.None))
        {
            if (gameObject.GetComponent<AvatarDataSyncScript>().IsOwner)
            {
                dataSyncScript = gameObject;
                Debug.Log(dataSyncScript.gameObject.name );
                agent = dataSyncScript.GetComponent<NavMeshAgent>();
                animator = agent.GetComponentInChildren<Animator>();
            }
        }
        //target.position = target.position + Vector3.up * 0.5f;
        agent.GetComponent<PlayerControllerNet>().SetTargetPosition(target.position);
        //dataSyncScript.AvatarMovement(target.position);

        // 等待直到角色到达目标点
        while (Vector3.Distance(agent.transform.position, target.position) > 0.5f)
        {
            yield return null;
        }
      
        Vector3 targetDirection = target.TransformDirection(Vector3.forward);
        targetDirection.y = 0; // 忽略Y轴方向
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        float elapsedTime = 0f;
        Debug.Log("targetRotation:  " + targetRotation + "   " + agent.transform.rotation);

        agent.transform.DORotateQuaternion(targetRotation, 0.5f);
        yield return new WaitForSeconds(0.5f);
        //while (elapsedTime < 0.5f)
        //{
        //    agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, 5 * Time.deltaTime);
        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}
        this.GetComponent<CustomLoader>().UnHover.Invoke();
        // 触发坐下动画
        animator.SetBool("Sit", true);
        agent.transform.position = target.position;
    }
    private IEnumerator MoveToChairCoroutine()
    {
        Debug.Log("targetRotation:  " + agent.transform.position + "   " + target.position);
        agent.GetComponent<PlayerController>().SetTargetPosition(target.position);

        // 等待直到角色到达目标点
        while (Vector3.Distance(agent.transform.position, target.position) > 0.1f)
        {
            yield return null;
        }
        // 平滑旋转角色朝向椅子物体自身的Z轴方向
        Vector3 targetDirection = target.TransformDirection(Vector3.forward);
        targetDirection.y = 0; // 忽略Y轴方向
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        float elapsedTime = 0f;
        Debug.Log("targetRotation:  " + targetRotation + "   " + agent.transform.rotation);
        while (elapsedTime < 0.5f)
        {
            agent.transform .rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, 5 * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.GetComponent<CustomLoader>().UnHover.Invoke();
        // 触发坐下动画
        animator.SetBool("Sit", true);
    }
}