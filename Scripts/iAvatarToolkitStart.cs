//using Cinemachine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//public class iAvatarToolkitStart : MonoBehaviour
//{
//    GameObject avatar,iavatar;
//    Material material;
//    Texture texture;

//    public GameObject AIModel;
//    public GameObject iAvatarF, iAvatarM;

//    public Cinemachine.CinemachineVirtualCamera CinemachineVirtualCamera;

//  //  public VNectBarracudaRunner VNectBarracudaRunner;
//   // public

//   public  Transform iAvatarFT, iAvatarMT;

//    public Vector3 stagepos;
//    public Vector3 stageroat;

//    public void onStage()
//    {
//        this.transform.position = stagepos;
//        this.transform.localEulerAngles = stageroat;

//        var CinemachineTransposer = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
//        CinemachineTransposer.m_FollowOffset = new Vector3(0.0f, 1.71f, -1.66f);
//        CinemachineVirtualCamera.transform.localEulerAngles = Vector3.right * 20;
//    }

//    public void BackSit()
//    {
//        this.transform.position = Vector3.zero;
//        this.transform.localEulerAngles = Vector3.zero;

//        var CinemachineTransposer = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
//        CinemachineTransposer.m_FollowOffset = new Vector3(0.0f, 1.71f, -0.66f);
//        CinemachineVirtualCamera.transform.localEulerAngles = Vector3.right * 20;
//    }
//    // Start is called before the first frame update
//    void Start()
//    {
//        if (GameObject.Find("Avatar") != null)
//        {
//            transform.GetChild(0).gameObject.SetActive(true);
//            avatar = GameObject.Find("Avatar");

          
//        }
//        else
//        {
//            return;
//        }


//       // DontDestroyOnLoad(this.gameObject);



//        if (avatar.GetComponent<Animator>().avatar.name == "FeminineAnimationAvatar")
//        {
//            CinemachineVirtualCamera.LookAt = iAvatarFT;
//            iavatar = iAvatarF;
//            AIModel.GetComponent<VNectBarracudaRunner>().VNectModel = iAvatarF.GetComponent<VNectModel>();
//        }
//        else
//        {
//            CinemachineVirtualCamera.LookAt = iAvatarMT;
//            iavatar = iAvatarM;
//            AIModel.GetComponent<VNectBarracudaRunner>().VNectModel = iAvatarM.GetComponent<VNectModel>();

//        }
//        iavatar.SetActive(true);



//        Texture texture = avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.mainTexture;
//        iavatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>(). material.mainTexture = texture;

//        iavatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh = 
//            avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh;

//        if (avatar.transform.childCount > 2)
//        {
//            iavatar.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material.mainTexture =
//                avatar.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material.mainTexture;

//            iavatar.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMesh =
//                avatar.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMesh;

//        }
//        else
//        {
//            iavatar.transform.GetChild(1).gameObject.SetActive(false);
//        }

//        avatar.SetActive(false);

//        AIModel.SetActive(true);

//    }

//    void ChangeCamera()
//    {

//        if (SceneManager.GetActiveScene().name != "HomePage")
//        {
//            CinemachineVirtualCamera.LookAt = null;
//            var CinemachineTransposer = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
//            CinemachineTransposer.m_FollowOffset = new Vector3(0.6f,1.43f,-1.11f);
//            CinemachineVirtualCamera.transform.localEulerAngles = Vector3.zero;
//        }
//        else
//        {

//            if (iAvatarF.activeSelf)
//            {
//                CinemachineVirtualCamera.LookAt = iAvatarFT;
//            }
//            else
//            {
//                CinemachineVirtualCamera.LookAt = iAvatarMT;
//            }

//            var CinemachineTransposer = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
//            CinemachineTransposer.m_FollowOffset = new Vector3(0.0f, 1.71f, -0.66f);
//            CinemachineVirtualCamera.transform.localEulerAngles = Vector3.right*20;
//        }


//    }

//    public void NeedDestroy()
//    {
//        AIModel.GetComponent<WebCamInput>().ChangeScence();
//        Destroy(this.gameObject);
//    }
//    public void NeedRestAvatar()
//    {
//        avatar.gameObject.SetActive(true);
//    }


//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
