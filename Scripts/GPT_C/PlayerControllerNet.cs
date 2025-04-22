
using DA_Assets.Shared.Extensions;
using EasyBuildSystem.Features.Runtime.Buildings.Manager;
using EasyBuildSystem.Features.Runtime.Buildings.Placer;
using Lean.Gui;
using ReadyPlayerMe.Core;
using SharpNav.Crowds;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterController))]
public class PlayerControllerNet : NetworkBehaviour
{
    [SerializeField] Animator m_Animator;
    NavMeshAgent m_Agent;

    void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
       
    }

    ModifyGameObject modifyGameObject = null;
    public BuildingPlacer buildingPlacer = null;
    private GameObject avatar;

    private bool isMoving = false;
    private Vector3 moveDirection = Vector3.zero;
    private float walkSpeed = 3f;

    private float smoothSpeed = 10f;
    private float moveTimer = 0f;
    private bool isRunning = false;
    Vector3 targetPosition;

    public void SetTargetPosition(Vector3 vector3)
    {
        //Debug.Log("SetTargetPosition");
        targetPosition = vector3;
        isMoving = true;
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {

            GetComponent<NavMeshAgent>().enabled = true;
        }
        else
        {
            gameObject.TryAddComponent<NavMeshObstacle>(out NavMeshObstacle navMeshObstacle);
            navMeshObstacle.shape= NavMeshObstacleShape.Box;
            navMeshObstacle.carving = true ;
            navMeshObstacle.size = Vector3.one;
        }
        base.OnNetworkSpawn();
    }
    void HandleClick(Vector3 clickPosition)
    {
        if (buildingPlacer != null && buildingPlacer.GetBuildMode == BuildingPlacer.BuildMode.PLACE)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            // 鼠标或触摸在UI上
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(clickPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Debug.Log(hit.transform.tag);
            if (hit.transform.tag != "CustomTool")
            {
                // 设置目标位置
                targetPosition = hit.point;
                isMoving = true;
            }
        }
    }
    // 长按时间限制
    private float longPressDuration = 0.5f;

    // 记录触控开始时间
    private float touchStartTime = 0;

    // 用于指示是否已检测到长按
    private bool touchLongPressed = false;

 
    void Update()
    {
       if (!IsOwner)
        {
            return;
        }


    
        // 处理鼠标点击
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick(Input.mousePosition);
        }

        // 处理触摸点击
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            HandleClick(Input.GetTouch(0).position);
        }


        float moveSpeed;

        float horizontal = 0f;
        float vertical = 0f;

        // 判断是否按下 Shift 键
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // 处理鼠标和键盘输入
    
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        

        //if (Input.touchCount == 1)
        //{
        //    Touch touch = Input.GetTouch(0);
         
        //    // 检查触控开始
        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        touchStartTime = Time.time;
        //        touchLongPressed = false;
        //    }

        //    // 检测触控长按
        //    if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
        //    {
        //        if (!touchLongPressed && Time.time - touchStartTime > longPressDuration)
        //        {
        //            touchLongPressed = true;
        //        }
        //    }

        //    // 检测长按且移动
        //    if (touchLongPressed && touch.phase == TouchPhase.Moved)
        //    {
        //        // 计算触控的方向
        //        Vector2 touchDelta = touch.deltaPosition;
        //        Debug.Log("touchLongPressed   " + touchDelta);
        //        // 根据屏幕尺寸进行归一化
        //        horizontal = touchDelta.x ;
        //        vertical = touchDelta.y;

            
        //    }
        //}

        // 计算相对于相机的运动方向
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;
        //Debug.Log("moveDirection  "+moveDirection);
        if (moveDirection != Vector3.zero)
        {
            m_Animator.SetBool("Sit", false);
            targetPosition = this.transform.position;

            // 设置角色朝向移动方向
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            // 更新计时器
            moveTimer += Time.deltaTime;
            moveSpeed = walkSpeed;
            // 切换为跑步状态
            if (moveTimer >= 2f)
            {
                moveSpeed = walkSpeed * 2;
            }
            if (shiftPressed)
            {
                moveSpeed = walkSpeed * 4;
            }

            // 设置目标速度
            float targetSpeed = moveSpeed;

            // 平滑处理速度
            float currentSpeed = Mathf.Lerp(m_Agent.velocity.magnitude, targetSpeed, smoothSpeed * Time.deltaTime);

            // 设置移动速度
            Vector3 movement = moveDirection * currentSpeed;
            m_Agent.velocity = movement;
            isMoving = true;
            m_Animator.SetFloat("Speed", m_Agent.velocity.magnitude);
            //Debug.Log("moveDirection  " + movement);
        }
        else
        {

            //return;

            NavMeshHit hit;
            if (Vector3.Distance(targetPosition, this.transform.position) > 0.5f && NavMesh.SamplePosition(targetPosition, out hit, 0.1f, NavMesh.AllAreas) && isMoving)
            {
                m_Animator.SetBool("Sit", false);
                moveDirection = (targetPosition - transform.position).normalized;
                // 設定角色朝向移動方向 
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
                m_Agent.velocity = moveDirection * walkSpeed * 2; // 使用 m_Agent.velocity 進行移動
                m_Animator.SetFloat("Speed", m_Agent.velocity.magnitude);
                //Debug.Log("moveDirection333  " + m_Agent.velocity);
                return;

            }
            //targetPosition = this.transform.position;
            // 重置计时器
            moveTimer = 0f;

            // 切换为走路状态
            if (isRunning)
            {
                isRunning = false;
            }

            // 平滑处理速度至零
            float currentSpeed = Mathf.Lerp(m_Agent.velocity.magnitude, 0f, smoothSpeed * 3 * Time.deltaTime);
         
            // 设置移动速度
            Vector3 movement = m_Agent.velocity.normalized * currentSpeed;
            m_Agent.velocity = movement;
            m_Animator.SetFloat("Speed", m_Agent.velocity.magnitude);
            //Debug.Log("moveDirection222  " + movement);
            isMoving = false;
        }
    }

}
