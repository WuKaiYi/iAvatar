using BestHTTP;
using Defective.JSON;
using EasyBuildSystem.Features.Runtime.Buildings.Manager;
using EasyBuildSystem.Features.Runtime.Buildings.Placer;
using Lean.Gui;
using ReadyPlayerMe.Core;
using SharpNav.Crowds;
using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator m_Animator;
    NavMeshAgent m_Agent;

    void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //if (SceneManager.GetActiveScene().name == "custon_tool_top_camera"|| SceneManager.GetActiveScene().name == "DungeonLayout")
        //{
          
        //}
        //else
        //{
        //    return;
        //}

        modifyGameObject = ModifyGameObject.instance;

        LoadAvatar(ES3.Load<int>("user_id"));
        //var avatarLoader = new AvatarObjectLoader();
        //avatarLoader.OnCompleted += (sender, args) =>
        //{
        //    Debug.Log($"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]");
        //    avatar = args.Avatar;
        //    avatar.name = "Customize Avatar";
        //    avatar.AddComponent<EyeAnimationHandler>();

        //    // AvatarAnimatorHelper.SetupAnimator(args.Metadata.BodyType, avatar);

        //    avatar.transform.parent = this.transform;
        //    avatar.transform.localPosition = Vector3.zero;
        //    avatar.transform.localEulerAngles = Vector3.zero;
        //    this.GetComponent<Animator>().avatar = avatar.GetComponent<Animator>().avatar;
        //    avatar.GetComponent<Animator>().enabled = false;
        //};

        //avatarLoader.OnFailed += (sender, args) =>
        //{
        //    Debug.Log(args.Type);
        //};

        //avatarLoader.LoadAvatar(ES3FormUser.Load<string>("CustomizeAvatarUrl"));


    }
  public   GameObject[] GameObjectAvatars;
    GameObject avatar;
    void LoadAvatar(int id)
    {
        Debug.Log("avatar-" + id);
        if (id == 0)
        {
            return;
        }
        var request = new HTTPRequest(new Uri(ServerConfig.host + "/api/get_user/" + id), HTTPMethods.Get, onFinished);
        Debug.Log(request.Uri);
        request.Send();

        void onFinished(HTTPRequest originalRequest, HTTPResponse response)
        {
            Debug.Log(response.DataAsText);
            JSONObject jd = new JSONObject(response.DataAsText);
            if (jd["avatar_url"].stringValue.Substring(0, 3) == "int")
            {
                avatar = Instantiate(GameObjectAvatars[int.Parse(Regex.Split(jd["avatar_url"].stringValue, "int", RegexOptions.IgnoreCase)[1])]);

                avatar.transform.parent = this.transform;
                avatar.transform.localPosition = Vector3.zero;
                avatar.transform.localEulerAngles = Vector3.zero;
                this.GetComponent<Animator>().avatar = avatar.GetComponent<Animator>().avatar;
                avatar.GetComponent<Animator>().enabled = false;
            }
            else
            {
            
                var avatarLoader = new AvatarObjectLoader();
                avatarLoader.OnCompleted += (sender, args) =>
                {
                    Debug.Log($"Loaded avatar. [{Time.timeSinceLevelLoad:F2}]");
                    avatar = args.Avatar;
                    avatar.name = "Customize Avatar";
                    avatar.AddComponent<EyeAnimationHandler>();

                    avatar.transform.parent = this.transform;
                    avatar.transform.localPosition = Vector3.zero;
                    avatar.transform.localEulerAngles = Vector3.zero;
                    this.GetComponent<Animator>().avatar = avatar.GetComponent<Animator>().avatar;
                    avatar.GetComponent<Animator>().enabled = false;
                };

                avatarLoader.OnFailed += (sender, args) =>
                {
                    Debug.Log(args.Type);
                };
                avatarLoader.LoadAvatar(jd["avatar_url"].stringValue);
            }
        
        }


    }

ModifyGameObject modifyGameObject = null;
  public  BuildingPlacer buildingPlacer   = null;
   

    public bool isMoving = false;
    public Vector3 moveDirection = Vector3.zero;
    private float walkSpeed = 3f;

    private float smoothSpeed = 10f;
    private float moveTimer = 0f;
    public bool isRunning = false;
  public   Vector3 targetPosition;

    public void  SetTargetPosition(Vector3 vector3)
    {
        isMoving = true;
        targetPosition = vector3;
        //moveDirection = (targetPosition - transform.position);
       
    }

    void Update()
    {
        //if (SceneManager.GetActiveScene().name == "custon_tool_top_camera" || SceneManager.GetActiveScene().name == "DungeonLayout")
        //{

        //}
        //else
        //{
          
        //    return;
        //}
        if (modifyGameObject!=null)
          if (modifyGameObject.canvas.GetComponent<LeanWindow >().On)
             return;

           // 获取水平和垂直移动输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 计算相对于相机的运动方向
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        if (Input.GetMouseButtonDown(0))
        {
            if (buildingPlacer != null)
            {
                if (buildingPlacer.GetBuildMode == BuildingPlacer.BuildMode.PLACE)
                    return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                // 鼠标在UI上
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.transform.tag);
                if (hit.transform.tag != "CustomTool")
                {        // 設定目標位置 
                    targetPosition = hit.point;
                    isMoving = true;
                    moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;
                }
            }
        }



        float moveSpeed;
        m_Animator.SetFloat("Speed", m_Agent.velocity.magnitude);
    
     
        // 判断是否按下 Shift 键
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);


        moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;
        //Debug.Log("moveDirection  "+moveDirection);
        if (moveDirection != Vector3.zero)
        { 
            targetPosition = this.transform.position;
            m_Animator.SetBool("Sit", false);
            //targetPosition =this.transform.position;

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
            //Debug.Log("moveDirection  " + movement);
        }
        else
        {
           
            //return;

            NavMeshHit hit;
            if (Vector3.Distance(targetPosition,this.transform.position )>0.5f&& NavMesh.SamplePosition(targetPosition, out hit, 0.1f, NavMesh.AllAreas)&&isMoving)
            {
                m_Animator.SetBool("Sit", false);
                moveDirection = (targetPosition - transform.position).normalized;
                // 設定角色朝向移動方向 
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
                m_Agent.velocity = moveDirection * walkSpeed * 2; // 使用 m_Agent.velocity 進行移動

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
            float currentSpeed = Mathf.Lerp(m_Agent.velocity.magnitude, 0f, smoothSpeed *3* Time.deltaTime);
           
            // 设置移动速度
            Vector3 movement = m_Agent.velocity.normalized * currentSpeed;
            m_Agent.velocity = movement;
            //Debug.Log("moveDirection222  " + movement);
            isMoving = false;
        }
    }

}
