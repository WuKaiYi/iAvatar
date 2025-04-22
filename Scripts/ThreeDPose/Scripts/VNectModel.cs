//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public enum PositionIndex : int
//{
//    rShldrBend ,
//    rForearmBend,
//    rHand,
//    rThumb2,
//    rMid1,

//    lShldrBend,
//    lForearmBend,
//    lHand,
//    lThumb2,
//    lMid1,

//    lEar,
//    lEye,
//    rEar,
//    rEye,
//    Nose,

//    rThighBend,
//    rShin,
//    rFoot,
//    rToe,

//    lThighBend,
//    lShin,
//    lFoot,
//    lToe,

//    abdomenUpper,

//    //Calculated coordinates
//    hip,
//    head,
//    neck,
//    spine,

   
//    LeftRingDistal,
//    LeftThumbProximal,
//    LeftThumbIntermediate,
//    LeftThumbDistal,
//    LeftIndexProximal,
//    LeftIndexIntermediate,
//    LeftIndexDistal,
//    LeftMiddleProximal,
//    LeftMiddleIntermediate,
//    LeftMiddleDistal,
//    LeftRingProximal,
//    LeftRingIntermediate,

   
//    LeftLittleProximal,
//    LeftLittleIntermediate,
//    LeftLittleDistal,
//    RightThumbIntermediate,
//    RightThumbProximal,
//    RightThumbDistal,
//    RightIndexProximal,
//    RightIndexIntermediate,
//    RightIndexDistal,
//    RightMiddleProximal,
//    RightMiddleIntermediate,
//    RightMiddleDistal,
//    RightRingProximal,
//    RightRingIntermediate,
//    RightRingDistal,
//    RightLittleProximal,
//    RightLittleIntermediate,
//    RightLittleDistal,


    

//    RightThumbDistalTip,
//    RightIndexDistalTip,
//    RightMiddleDistalTip,
//    RightRingDistalTip,
//    RightLittleDistalTip,

//    LeftIndexDistalTip,
//    LeftMiddleDistalTip,
//    LeftRingDistalTip,
//    LeftLittleDistalTip,
//    LeftThumbDistalTip,

//    Count,
//    None,
   
//}

//public static partial class EnumExtend
//{
//    public static int Int(this PositionIndex i)
//    {
//        return (int)i;
//    }
//}

//public class VNectModel : MonoBehaviour
//{
//    public RootMotion.FinalIK.VRIK VRIK;
//    public class JointPoint
//    {
//        public PositionIndex Index;
//        public Vector2 Pos2D = new Vector2();
//        public float Score2D;

//        public Vector3 Pos3D = new Vector3();
//        public Vector3 Now3D = new Vector3();
//        //public Vector3 PPos3D = new Vector3();
//        //public Vector3 PPPos3D = new Vector3();
//        public Vector3[] PrevPos3D = new Vector3[10];
//        //public Vector3 PrevNow3D = new Vector3();
//        //public Vector3 PPrevNow3D = new Vector3();
//        //public Vector3 Predicted3D = new Vector3();
//        //public Vector3 VecNow3D = new Vector3();
//        //public Vector3 PPredicted3D = new Vector3();
//        //public Vector3 AccNow3D = new Vector3();
//        //public Vector3 VelNow3D = new Vector3();
//        //public float VecAngle;
//        public float Score3D;
//        public bool Visibled;
//        public int Error;
//        //public float VecNow3DMagnitude;
//        //public bool RattlingCheck;
//        //public float RattlingCheckFrame;
//        //public float Threshold;
//        //public float Smooth;
//        //public float Ratio;
//        public bool UpperBody;
//        public bool Lock;
//        public int maxXIndex;
//        public int maxYIndex;
//        public int maxZIndex;

//        // Bones
//        public Transform Transform = null;
//        public Quaternion InitRotation;
//        public Quaternion Inverse;
//        public Quaternion InverseRotation;

//        public JointPoint Child = null;
//        public JointPoint Parent = null;
//        public float VecFlag = 1f;

//        public static float Q = 0.001f;
//        public static float R = 0.0015f;
//        public Vector3 P = new Vector3();
//        public Vector3 X = new Vector3();
//        public Vector3 K = new Vector3();

//        public bool hand=false;
//    }

//    public class Skeleton
//    {
//        public GameObject LineObject;
//        public LineRenderer Line;

//        public JointPoint start = null;
//        public JointPoint end = null;
//        public bool upperBody = false;
//    }

//    private List<Skeleton> Skeletons = new List<Skeleton>();
//    public Material SkeletonMaterial;

//    public float SkeletonX;
//    public float SkeletonY;
//    public float SkeletonZ;
//    public float SkeletonScale;

//    // Joint position and bone
//    private JointPoint[] jointPoints;
//    public JointPoint[] JointPoints { get { return jointPoints; } }

//    private Vector3 initPosition; // Initial center position

//    private Quaternion InitGazeRotation;
//    private Quaternion gazeInverse;

//    // UnityChan
//    public GameObject ModelObject;
//    public GameObject Nose;
//    private Animator anim;

//    private float movementScale = 0.01f;
//    private float centerTall = 224 * 0.75f;
//    private float tall = 224 * 0.75f;
//    private float prevTall = 224 * 0.75f;
//    public float ZScale = 0.8f;

//    public bool LockFoot = false;
//    public bool LockLegs = false;
//    public bool LockHand = false;
//    private float FootIKY = 0f;
//    private float ToeIKY = 0f;

//    private bool UpperBodyMode = false;
//    private float UpperBodyF = 1f;

//    /**** Foot IK ****/
//    [SerializeField]
//    private bool useIK = true;
//    //　IKで角度を有効にするかどうか
//    [SerializeField]
//    private bool useIKRot = true;
//    //　右足のウエイト
//    private float rightFootWeight = 0f;
//    //　左足のウエイト
//    private float leftFootWeight = 0f;
//    //　右足の位置
//    private Vector3 rightFootPos;
//    //　左足の位置
//    private Vector3 leftFootPos;
//    //　右足の角度
//    private Quaternion rightFootRot;
//    //　左足の角度
//    private Quaternion leftFootRot;
//    //　右足と左足の距離
//    private float distance;
//    //　足を付く位置のオフセット値
//    [SerializeField]
//    private float offset = 0.1f;
//    //　コライダの中心位置
//    private Vector3 defaultCenter;
//    //　レイを飛ばす距離
//    [SerializeField]
//    private float rayRange = 1f;

//    //　コライダの位置を調整する時のスピード
//    [SerializeField]
//    private float smoothing = 100f;

//    //　レイを飛ばす位置の調整値
//    [SerializeField]
//    private Vector3 rayPositionOffset = Vector3.up * 0.3f;

//    public Transform[] VRIKTransforms;
//    public Vector3 ofv;

//    public bool showSkeletons = false;

//    public JointPoint[] Init(int inputImageSize, ConfigurationSetting config)
//    {
//        movementScale = 0.01f * 224f / inputImageSize;
//        centerTall = inputImageSize * 0.75f;
//        tall = inputImageSize * 0.75f;
//        prevTall = inputImageSize * 0.75f;

//        jointPoints = new JointPoint[PositionIndex.Count.Int()];

//        VRIKTransforms = new Transform[PositionIndex.Count.Int()];

//        for (var i = 0; i < PositionIndex.Count.Int(); i++)
//        {
//            jointPoints[i] = new JointPoint();
//            jointPoints[i].Index = (PositionIndex)i;
//            jointPoints[i].Score3D = 1;
//            //jointPoints[i].RattlingCheck = false;
//            //jointPoints[i].VecNow3DMagnitude = 0;
//            jointPoints[i].UpperBody = false;
//            jointPoints[i].Lock = false;
//            jointPoints[i].Error = 0;


//        }

//        anim = ModelObject.GetComponent<Animator>();
//        jointPoints[PositionIndex.hip.Int()].Transform = transform;

//        // Right Arm
//        jointPoints[PositionIndex.rShldrBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
//        jointPoints[PositionIndex.rForearmBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
//        jointPoints[PositionIndex.rHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightHand);
//        jointPoints[PositionIndex.rThumb2.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLittleProximal);
//        jointPoints[PositionIndex.rMid1.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightIndexProximal);
//        // Left Arm
//        jointPoints[PositionIndex.lShldrBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
//        jointPoints[PositionIndex.lForearmBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
//        jointPoints[PositionIndex.lHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftHand);
//        jointPoints[PositionIndex.lThumb2.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLittleProximal);
//        jointPoints[PositionIndex.lMid1.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftIndexProximal);
//        // Face
//        jointPoints[PositionIndex.lEar.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
//        jointPoints[PositionIndex.lEye.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftEye);
//        jointPoints[PositionIndex.rEar.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
//        jointPoints[PositionIndex.rEye.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightEye);
//        jointPoints[PositionIndex.Nose.Int()].Transform = Nose.transform;

//        // Right Leg
//        jointPoints[PositionIndex.rThighBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
//        jointPoints[PositionIndex.rShin.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
//        jointPoints[PositionIndex.rFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightFoot);
//        jointPoints[PositionIndex.rToe.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightToes);

//        // Left Leg
//        jointPoints[PositionIndex.lThighBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
//        jointPoints[PositionIndex.lShin.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
//        jointPoints[PositionIndex.lFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
//        jointPoints[PositionIndex.lToe.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftToes);

//        // etc
//        jointPoints[PositionIndex.abdomenUpper.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Spine);
//        jointPoints[PositionIndex.head.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
//        jointPoints[PositionIndex.hip.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Hips);
//        jointPoints[PositionIndex.neck.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Neck);
//        jointPoints[PositionIndex.spine.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Spine);


//        jointPoints[PositionIndex.LeftThumbProximal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftThumbProximal);
//        jointPoints[PositionIndex.LeftThumbIntermediate.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate);
//        jointPoints[PositionIndex.LeftThumbDistal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftThumbDistal);
//        jointPoints[PositionIndex.LeftIndexProximal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftIndexProximal);
//        jointPoints[PositionIndex.LeftIndexIntermediate.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftIndexIntermediate);
//        jointPoints[PositionIndex.LeftIndexDistal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftIndexDistal);
//        jointPoints[PositionIndex.LeftMiddleProximal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftMiddleProximal);
//        jointPoints[PositionIndex.LeftMiddleIntermediate.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftMiddleIntermediate);
//        jointPoints[PositionIndex.LeftMiddleDistal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftMiddleDistal);
//        jointPoints[PositionIndex.LeftRingProximal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftRingProximal);
//        jointPoints[PositionIndex.LeftRingIntermediate.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftRingIntermediate);
//        jointPoints[PositionIndex.LeftRingDistal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftRingDistal);
//        jointPoints[PositionIndex.LeftLittleProximal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLittleProximal);
//        jointPoints[PositionIndex.LeftLittleIntermediate.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLittleIntermediate);
//        jointPoints[PositionIndex.LeftLittleDistal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLittleDistal);


//        jointPoints[PositionIndex.RightThumbProximal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightThumbProximal);
//        jointPoints[PositionIndex.RightThumbIntermediate.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightThumbIntermediate);
//        jointPoints[PositionIndex.RightThumbDistal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightThumbDistal);
//        jointPoints[PositionIndex.RightIndexProximal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightIndexProximal);
//        jointPoints[PositionIndex.RightIndexIntermediate.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightIndexIntermediate);
//        jointPoints[PositionIndex.RightIndexDistal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightIndexDistal);
//        jointPoints[PositionIndex.RightMiddleProximal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightMiddleProximal);
//        jointPoints[PositionIndex.RightMiddleIntermediate.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate);
//        jointPoints[PositionIndex.RightMiddleDistal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightMiddleDistal);
//        jointPoints[PositionIndex.RightRingProximal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightRingProximal);
//        jointPoints[PositionIndex.RightRingIntermediate.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightRingIntermediate);
//        jointPoints[PositionIndex.RightRingDistal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightRingDistal);
//        jointPoints[PositionIndex.RightLittleProximal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLittleProximal);
//        jointPoints[PositionIndex.RightLittleIntermediate.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLittleIntermediate);
//        jointPoints[PositionIndex.RightLittleDistal.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLittleDistal);

//        jointPoints[PositionIndex.RightThumbDistalTip.Int()].Transform = jointPoints[PositionIndex.RightThumbDistal.Int()].Transform.GetChild(0);
//        jointPoints[PositionIndex.RightIndexDistalTip.Int()].Transform = jointPoints[PositionIndex.RightIndexDistal.Int()].Transform.GetChild(0);
//        jointPoints[PositionIndex.RightMiddleDistalTip.Int()].Transform = jointPoints[PositionIndex.RightMiddleDistal.Int()].Transform.GetChild(0);
//        jointPoints[PositionIndex.RightRingDistalTip.Int()].Transform = jointPoints[PositionIndex.RightRingDistal.Int()].Transform.GetChild(0);
//        jointPoints[PositionIndex.RightLittleDistalTip.Int()].Transform = jointPoints[PositionIndex.RightLittleDistal.Int()].Transform.GetChild(0);

//        jointPoints[PositionIndex.LeftThumbDistalTip.Int()].Transform = jointPoints[PositionIndex.LeftThumbDistal.Int()].Transform.GetChild(0);
//        jointPoints[PositionIndex.LeftIndexDistalTip.Int()].Transform = jointPoints[PositionIndex.LeftIndexDistal.Int()].Transform.GetChild(0);
//        jointPoints[PositionIndex.LeftMiddleDistalTip.Int()].Transform = jointPoints[PositionIndex.LeftMiddleDistal.Int()].Transform.GetChild(0);
//        jointPoints[PositionIndex.LeftRingDistalTip.Int()].Transform = jointPoints[PositionIndex.LeftRingDistal.Int()].Transform.GetChild(0);
//        jointPoints[PositionIndex.LeftLittleDistalTip.Int()].Transform = jointPoints[PositionIndex.LeftLittleDistal.Int()].Transform.GetChild(0);


//        jointPoints[PositionIndex.LeftThumbProximal.Int()].hand = true;
//        jointPoints[PositionIndex.LeftThumbIntermediate.Int()].hand = true;
//        jointPoints[PositionIndex.LeftThumbDistal.Int()].hand = true;
//        jointPoints[PositionIndex.LeftIndexProximal.Int()].hand = true;
//        jointPoints[PositionIndex.LeftIndexIntermediate.Int()].hand = true;
//        jointPoints[PositionIndex.LeftIndexDistal.Int()].hand = true;
//        jointPoints[PositionIndex.LeftMiddleProximal.Int()].hand = true;
//        jointPoints[PositionIndex.LeftMiddleIntermediate.Int()].hand = true;
//        jointPoints[PositionIndex.LeftMiddleDistal.Int()].hand = true;
//        jointPoints[PositionIndex.LeftRingProximal.Int()].hand = true;
//        jointPoints[PositionIndex.LeftRingIntermediate.Int()].hand = true;
//        jointPoints[PositionIndex.LeftRingDistal.Int()].hand = true;
//        jointPoints[PositionIndex.LeftLittleProximal.Int()].hand = true;
//        jointPoints[PositionIndex.LeftLittleIntermediate.Int()].hand = true;
//        jointPoints[PositionIndex.LeftLittleDistal.Int()].hand = true;


//        jointPoints[PositionIndex.RightThumbProximal.Int()].hand = true;
//        jointPoints[PositionIndex.RightThumbIntermediate.Int()].hand = true;
//        jointPoints[PositionIndex.RightThumbDistal.Int()].hand = true;
//        jointPoints[PositionIndex.RightIndexProximal.Int()].hand = true;
//        jointPoints[PositionIndex.RightIndexIntermediate.Int()].hand = true;
//        jointPoints[PositionIndex.RightIndexDistal.Int()].hand = true;
//        jointPoints[PositionIndex.RightMiddleProximal.Int()].hand = true;
//        jointPoints[PositionIndex.RightMiddleIntermediate.Int()].hand = true;
//        jointPoints[PositionIndex.RightMiddleDistal.Int()].hand = true;
//        jointPoints[PositionIndex.RightRingProximal.Int()].hand = true;
//        jointPoints[PositionIndex.RightRingIntermediate.Int()].hand = true;
//        jointPoints[PositionIndex.RightRingDistal.Int()].hand = true;
//        jointPoints[PositionIndex.RightLittleProximal.Int()].hand = true;
//        jointPoints[PositionIndex.RightLittleIntermediate.Int()].hand = true;
//        jointPoints[PositionIndex.RightLittleDistal.Int()].hand = true;

//        jointPoints[PositionIndex.RightThumbDistalTip.Int()].hand = true;
//        jointPoints[PositionIndex.RightIndexDistalTip.Int()].hand = true;
//        jointPoints[PositionIndex.RightMiddleDistalTip.Int()].hand = true;
//        jointPoints[PositionIndex.RightRingDistalTip.Int()].hand = true;
//        jointPoints[PositionIndex.RightLittleDistalTip.Int()].hand = true;

//        // UpperBody Settings
//        jointPoints[PositionIndex.hip.Int()].UpperBody = true;
//        // Right Arm
//        jointPoints[PositionIndex.rShldrBend.Int()].UpperBody = true;
//        jointPoints[PositionIndex.rForearmBend.Int()].UpperBody = true;
//        jointPoints[PositionIndex.rHand.Int()].UpperBody = true;
//        jointPoints[PositionIndex.rThumb2.Int()].UpperBody = true;
//        jointPoints[PositionIndex.rMid1.Int()].UpperBody = true;
//        // Left Arm
//        jointPoints[PositionIndex.lShldrBend.Int()].UpperBody = true;
//        jointPoints[PositionIndex.lForearmBend.Int()].UpperBody = true;
//        jointPoints[PositionIndex.lHand.Int()].UpperBody = true;
//        jointPoints[PositionIndex.lThumb2.Int()].UpperBody = true;
//        jointPoints[PositionIndex.lMid1.Int()].UpperBody = true;
//        // Face
//        jointPoints[PositionIndex.lEar.Int()].UpperBody = true;
//        jointPoints[PositionIndex.lEye.Int()].UpperBody = true;
//        jointPoints[PositionIndex.rEar.Int()].UpperBody = true;
//        jointPoints[PositionIndex.rEye.Int()].UpperBody = true;
//        jointPoints[PositionIndex.Nose.Int()].UpperBody = true;
//        // etc
//        jointPoints[PositionIndex.spine.Int()].UpperBody = true;
//        jointPoints[PositionIndex.neck.Int()].UpperBody = true;

//        // Parent and Child Settings
//        // Right Arm
//        jointPoints[PositionIndex.rShldrBend.Int()].Child = jointPoints[PositionIndex.rForearmBend.Int()];
//        jointPoints[PositionIndex.rForearmBend.Int()].Child = jointPoints[PositionIndex.rHand.Int()];
//        if (config.ElbowAxisTop == 0)
//        {
//            jointPoints[PositionIndex.rForearmBend.Int()].Parent = jointPoints[PositionIndex.rShldrBend.Int()];
//        }
//        //jointPoints[PositionIndex.rHand.Int()].Parent = jointPoints[PositionIndex.rForearmBend.Int()];

//        // Left Arm
//        jointPoints[PositionIndex.lShldrBend.Int()].Child = jointPoints[PositionIndex.lForearmBend.Int()];
//        jointPoints[PositionIndex.lForearmBend.Int()].Child = jointPoints[PositionIndex.lHand.Int()];
//        if (config.ElbowAxisTop == 0)
//        {
//            jointPoints[PositionIndex.lForearmBend.Int()].Parent = jointPoints[PositionIndex.lShldrBend.Int()];
//        }
//        //jointPoints[PositionIndex.lHand.Int()].Parent = jointPoints[PositionIndex.lForearmBend.Int()];

//        // Fase

//        // Right Leg
//        jointPoints[PositionIndex.rThighBend.Int()].Child = jointPoints[PositionIndex.rShin.Int()];
//        jointPoints[PositionIndex.rShin.Int()].Child = jointPoints[PositionIndex.rFoot.Int()];
//        jointPoints[PositionIndex.rFoot.Int()].Child = jointPoints[PositionIndex.rToe.Int()];
//        jointPoints[PositionIndex.rFoot.Int()].Parent = jointPoints[PositionIndex.rShin.Int()];
         
//        // Left Leg
//        jointPoints[PositionIndex.lThighBend.Int()].Child = jointPoints[PositionIndex.lShin.Int()];
//        jointPoints[PositionIndex.lShin.Int()].Child = jointPoints[PositionIndex.lFoot.Int()];
//        jointPoints[PositionIndex.lFoot.Int()].Child = jointPoints[PositionIndex.lToe.Int()];
//        jointPoints[PositionIndex.lFoot.Int()].Parent = jointPoints[PositionIndex.lShin.Int()];

//        // etc
//        jointPoints[PositionIndex.spine.Int()].Child = jointPoints[PositionIndex.neck.Int()];
//        jointPoints[PositionIndex.neck.Int()].Child = jointPoints[PositionIndex.head.Int()];
//        jointPoints[PositionIndex.head.Int()].Child = jointPoints[PositionIndex.Nose.Int()];
//        //jointPoints[PositionIndex.hip.Int()].Child = jointPoints[PositionIndex.spine.Int()];

//      //  jointPoints[PositionIndex.rHand.Int()].Parent = jointPoints[PositionIndex.rForearmBend.Int()];
//        jointPoints[PositionIndex.rHand.Int()].Child = jointPoints[PositionIndex.RightThumbProximal.Int()];

//       // jointPoints[PositionIndex.RightThumbProximal.Int()].Parent = jointPoints[PositionIndex.rHand.Int()];
//        jointPoints[PositionIndex.RightThumbProximal.Int()].Child = jointPoints[PositionIndex.RightThumbIntermediate.Int()];
//        jointPoints[PositionIndex.RightThumbIntermediate.Int()].Child = jointPoints[PositionIndex.RightThumbDistal.Int()];

//       // jointPoints[PositionIndex.RightIndexProximal.Int()].Parent = jointPoints[PositionIndex.rHand.Int()];
//        jointPoints[PositionIndex.RightIndexProximal.Int()].Child = jointPoints[PositionIndex.RightIndexIntermediate.Int()];
//        jointPoints[PositionIndex.RightIndexIntermediate.Int()].Child = jointPoints[PositionIndex.RightIndexDistal.Int()];

//        //jointPoints[PositionIndex.RightMiddleProximal.Int()].Parent = jointPoints[PositionIndex.rHand.Int()];
//        jointPoints[PositionIndex.RightMiddleProximal.Int()].Child = jointPoints[PositionIndex.RightMiddleIntermediate.Int()];
//        jointPoints[PositionIndex.RightMiddleIntermediate.Int()].Child = jointPoints[PositionIndex.RightMiddleDistal.Int()];

//       // jointPoints[PositionIndex.RightRingProximal.Int()].Parent = jointPoints[PositionIndex.rHand.Int()];
//        jointPoints[PositionIndex.RightRingProximal.Int()].Child = jointPoints[PositionIndex.RightRingIntermediate.Int()];
//        jointPoints[PositionIndex.RightRingIntermediate.Int()].Child = jointPoints[PositionIndex.RightRingDistal.Int()];

//      //  jointPoints[PositionIndex.RightLittleProximal.Int()].Parent = jointPoints[PositionIndex.rHand.Int()];
//        jointPoints[PositionIndex.RightLittleProximal.Int()].Child = jointPoints[PositionIndex.RightLittleIntermediate.Int()];
//        jointPoints[PositionIndex.RightLittleIntermediate.Int()].Child = jointPoints[PositionIndex.RightLittleDistal.Int()];

//        jointPoints[PositionIndex.lHand.Int()].Parent = jointPoints[PositionIndex.lForearmBend.Int()];
//        jointPoints[PositionIndex.lHand.Int()].Child = jointPoints[PositionIndex.LeftThumbProximal.Int()];

//       // jointPoints[PositionIndex.LeftThumbProximal.Int()].Parent = jointPoints[PositionIndex.lHand.Int()];
//        jointPoints[PositionIndex.LeftThumbProximal.Int()].Child = jointPoints[PositionIndex.LeftThumbIntermediate.Int()];
//        jointPoints[PositionIndex.LeftThumbIntermediate.Int()].Child = jointPoints[PositionIndex.LeftThumbDistal.Int()];

//    //    jointPoints[PositionIndex.LeftIndexProximal.Int()].Parent = jointPoints[PositionIndex.lHand.Int()];
//        jointPoints[PositionIndex.LeftIndexProximal.Int()].Child = jointPoints[PositionIndex.LeftIndexIntermediate.Int()];
//        jointPoints[PositionIndex.LeftIndexIntermediate.Int()].Child = jointPoints[PositionIndex.LeftIndexDistal.Int()];

//      //  jointPoints[PositionIndex.LeftMiddleProximal.Int()].Parent = jointPoints[PositionIndex.lHand.Int()];
//        jointPoints[PositionIndex.LeftMiddleProximal.Int()].Child = jointPoints[PositionIndex.LeftMiddleIntermediate.Int()];
//        jointPoints[PositionIndex.LeftMiddleIntermediate.Int()].Child = jointPoints[PositionIndex.LeftMiddleDistal.Int()];

//      //  jointPoints[PositionIndex.LeftRingProximal.Int()].Parent = jointPoints[PositionIndex.lHand.Int()];
//        jointPoints[PositionIndex.LeftRingProximal.Int()].Child = jointPoints[PositionIndex.LeftRingIntermediate.Int()];
//        jointPoints[PositionIndex.LeftRingIntermediate.Int()].Child = jointPoints[PositionIndex.LeftRingDistal.Int()];

//      //  jointPoints[PositionIndex.LeftLittleProximal.Int()].Parent = jointPoints[PositionIndex.lHand.Int()];
//        jointPoints[PositionIndex.LeftLittleProximal.Int()].Child = jointPoints[PositionIndex.LeftLittleIntermediate.Int()];
//        jointPoints[PositionIndex.LeftLittleIntermediate.Int()].Child = jointPoints[PositionIndex.LeftLittleDistal.Int()];




//        if (showSkeletons)
//        {
//            // Line Child Settings
//            // Right Arm
//            AddSkeleton(PositionIndex.rShldrBend, PositionIndex.rForearmBend, true);
//            AddSkeleton(PositionIndex.rForearmBend, PositionIndex.rHand, true);
//            AddSkeleton(PositionIndex.rHand, PositionIndex.rThumb2, true);
//            AddSkeleton(PositionIndex.rHand, PositionIndex.rMid1, true);

//            // Left Arm
//            AddSkeleton(PositionIndex.lShldrBend, PositionIndex.lForearmBend, true);
//            AddSkeleton(PositionIndex.lForearmBend, PositionIndex.lHand, true);
//            AddSkeleton(PositionIndex.lHand, PositionIndex.lThumb2, true);
//            AddSkeleton(PositionIndex.lHand, PositionIndex.lMid1, true);

//            // Face
//            //AddSkeleton(PositionIndex.lEar, PositionIndex.lEye);
//            //AddSkeleton(PositionIndex.lEye, PositionIndex.Nose);
//            //AddSkeleton(PositionIndex.rEar, PositionIndex.rEye);
//            //AddSkeleton(PositionIndex.rEye, PositionIndex.Nose);
//            AddSkeleton(PositionIndex.lEar, PositionIndex.Nose, true);
//            AddSkeleton(PositionIndex.rEar, PositionIndex.Nose, true);

//            // Right Leg
//            AddSkeleton(PositionIndex.rThighBend, PositionIndex.rShin, false);
//            AddSkeleton(PositionIndex.rShin, PositionIndex.rFoot, false);
//            AddSkeleton(PositionIndex.rFoot, PositionIndex.rToe, false);

//            // Left Leg
//            AddSkeleton(PositionIndex.lThighBend, PositionIndex.lShin, false);
//            AddSkeleton(PositionIndex.lShin, PositionIndex.lFoot, false);
//            AddSkeleton(PositionIndex.lFoot, PositionIndex.lToe, false);

//            // etc
//            AddSkeleton(PositionIndex.spine, PositionIndex.neck, true);
//            AddSkeleton(PositionIndex.neck, PositionIndex.head, true);
//            AddSkeleton(PositionIndex.head, PositionIndex.Nose, true);
//            AddSkeleton(PositionIndex.neck, PositionIndex.rShldrBend, true);
//            AddSkeleton(PositionIndex.neck, PositionIndex.lShldrBend, true);
//            AddSkeleton(PositionIndex.rThighBend, PositionIndex.rShldrBend, true);
//            AddSkeleton(PositionIndex.lThighBend, PositionIndex.lShldrBend, true);
//            AddSkeleton(PositionIndex.rShldrBend, PositionIndex.abdomenUpper, true);
//            AddSkeleton(PositionIndex.lShldrBend, PositionIndex.abdomenUpper, true);
//            AddSkeleton(PositionIndex.rThighBend, PositionIndex.abdomenUpper, true);
//            AddSkeleton(PositionIndex.lThighBend, PositionIndex.abdomenUpper, true);
//            AddSkeleton(PositionIndex.lThighBend, PositionIndex.rThighBend, true);
//        }
//        // Set Inverse
//        var forward = TriangleNormal(jointPoints[PositionIndex.spine.Int()].Transform.position, jointPoints[PositionIndex.lThighBend.Int()].Transform.position, jointPoints[PositionIndex.rThighBend.Int()].Transform.position);

//        foreach (var jointPoint in jointPoints)
//        {
//            if (jointPoint.Transform != null)
//            {
//                jointPoint.InitRotation = jointPoint.Transform.rotation;
//            }

//            if (jointPoint.Child != null)
//            {
//                jointPoint.Inverse = GetInverse(jointPoint, jointPoint.Child, forward);
//                jointPoint.InverseRotation = jointPoint.Inverse * jointPoint.InitRotation;
//            }
//        }
//        var hip = jointPoints[PositionIndex.hip.Int()];
//        initPosition = transform.position;
//        //initPosition = jointPoints[PositionIndex.hip.Int()].Transform.position;
//        hip.Inverse = Quaternion.Inverse(Quaternion.LookRotation(forward));
//        hip.InverseRotation = hip.Inverse * hip.InitRotation;

//        // For Head Rotation
//        var head = jointPoints[PositionIndex.head.Int()];
//        head.InitRotation = jointPoints[PositionIndex.head.Int()].Transform.rotation;
//        var gaze = jointPoints[PositionIndex.Nose.Int()].Transform.position - jointPoints[PositionIndex.head.Int()].Transform.position;
//        head.Inverse = Quaternion.Inverse(Quaternion.LookRotation(gaze));
//        head.InverseRotation = head.Inverse * head.InitRotation;

//        var lHand = jointPoints[PositionIndex.lHand.Int()];
//      //  var lf = TriangleNormal(lHand.Pos3D, jointPoints[PositionIndex.lMid1.Int()].Pos3D, jointPoints[PositionIndex.lThumb2.Int()].Pos3D);
//        lHand.InitRotation = lHand.Transform.rotation;
//        lHand.Inverse = Quaternion.Inverse(Quaternion.LookRotation(jointPoints[PositionIndex.LeftMiddleProximal.Int()].Transform.position - jointPoints[PositionIndex.lHand.Int()].Transform.position));
//        lHand.InverseRotation = lHand.Inverse * lHand.InitRotation;

//        var rHand = jointPoints[PositionIndex.rHand.Int()];
//       // var rf = TriangleNormal(rHand.Pos3D, jointPoints[PositionIndex.RightIndexProximal.Int()].Pos3D, jointPoints[PositionIndex.RightLittleProximal.Int()].Pos3D);
//        rHand.InitRotation = jointPoints[PositionIndex.rHand.Int()].Transform.rotation;
//        rHand.Inverse = Quaternion.Inverse(Quaternion.LookRotation(jointPoints[PositionIndex.RightMiddleProximal.Int()].Transform.position - jointPoints[PositionIndex.rHand.Int()].Transform.position));
//        rHand.InverseRotation = rHand.Inverse * rHand.InitRotation;

       

//        jointPoints[PositionIndex.hip.Int()].Score3D = 1f;
//        jointPoints[PositionIndex.neck.Int()].Score3D = 1f;
//        jointPoints[PositionIndex.Nose.Int()].Score3D = 1f;
//        jointPoints[PositionIndex.head.Int()].Score3D = 1f;
//        jointPoints[PositionIndex.spine.Int()].Score3D = 1f;
//        /*
//        jointPoints[PositionIndex.rForearmBend.Int()].RattlingCheck = true;
//        jointPoints[PositionIndex.rHand.Int()].RattlingCheck = true;
//        jointPoints[PositionIndex.rThumb2.Int()].RattlingCheck = true;
//        jointPoints[PositionIndex.rMid1.Int()].RattlingCheck = true;
//        jointPoints[PositionIndex.lForearmBend.Int()].RattlingCheck = true;
//        jointPoints[PositionIndex.lHand.Int()].RattlingCheck = true;
//        jointPoints[PositionIndex.lThumb2.Int()].RattlingCheck = true;
//        jointPoints[PositionIndex.lMid1.Int()].RattlingCheck = true;
//        jointPoints[PositionIndex.rShin.Int()].RattlingCheck = true;
//        jointPoints[PositionIndex.rFoot.Int()].RattlingCheck = true;
//        jointPoints[PositionIndex.rToe.Int()].RattlingCheck = true;
//        jointPoints[PositionIndex.lShin.Int()].RattlingCheck = true;
//        jointPoints[PositionIndex.lFoot.Int()].RattlingCheck = true;
//        jointPoints[PositionIndex.lToe.Int()].RattlingCheck = true;
//        */
//        SetPredictSetting(config);

//        defaultCenter = new Vector3(transform.position.x, (jointPoints[PositionIndex.rToe.Int()].Transform.position.y + jointPoints[PositionIndex.lToe.Int()].Transform.position.y) /2f, transform.position.z);
//        FootIKY = (jointPoints[PositionIndex.rFoot.Int()].Transform.position.y + jointPoints[PositionIndex.lFoot.Int()].Transform.position.y) / 2f + 0.1f;
//        ToeIKY = (jointPoints[PositionIndex.rToe.Int()].Transform.position.y + jointPoints[PositionIndex.lToe.Int()].Transform.position.y) / 2f;

//      //  handoffx = Vector3.Distance(jointPoints[PositionIndex.RightIndexProximal.Int()].Transform.position, jointPoints[PositionIndex.RightLittleProximal.Int()].Transform.position);


//        return JointPoints;
//    }
//   public float handoffx;
//    public void SetNose(float x, float y, float z)
//    {
//        if (this.Nose == null)
//        {
//            this.Nose = new GameObject(this.name + "_Nose");
//        }
//        var ani = ModelObject.GetComponent<Animator>();
//        var t = ani.GetBoneTransform(HumanBodyBones.Head);
//        this.Nose.transform.position = new Vector3(t.position.x + x, t.position.y + y, t.position.z + z);

//    }

//    public void SetSettings(AvatarSetting setting)
//    {
//        this.name = setting.AvatarName;

//        ResetPosition(setting.PosX, setting.PosY, setting.PosZ);
//        SetNose(setting.FaceOriX, setting.FaceOriY, setting.FaceOriZ);
//        SetScale(setting.Scale);
//        SetZScale(setting.DepthScale);

//        SetSkeleton(setting.SkeletonVisible == 1);

//        SkeletonX = setting.SkeletonPosX;
//        SkeletonY = setting.SkeletonPosY;
//        SkeletonZ = setting.SkeletonPosZ;
//        SkeletonScale = setting.SkeletonScale;
//    }

//    public void SetScale(float scale)
//    {
//        transform.localScale = new Vector3(scale, scale, scale);
//    }

//    public void SetZScale(float zScale)
//    {
//        ZScale = zScale;
//    }

//    public void SetSkeleton(bool flag)
//    {
//        foreach (var sk in Skeletons)
//        {
//            sk.LineObject.SetActive(flag);
//        }
//    }

//    public void ResetPosition(float x, float y, float z)
//    {
//        transform.position = new Vector3(x, y, z);
//        initPosition = transform.position;
//    }

//    public Vector3 GetHeadPosition()
//    {
//        return anim.GetBoneTransform(HumanBodyBones.Head).position;
//    }

//    public void Show()
//    {
//    }

//    public void Hide()
//    {
//        SetSkeleton(false);
//    }

//    public void SetUpperBodyMode(bool upper)
//    {
//        UpperBodyMode = upper;
//        UpperBodyF = upper ? 0f : 1f;
//    }

//    public void SetPredictSetting(ConfigurationSetting config)
//    {
//        if(jointPoints == null)
//        {
//            return;
//        }
//        /*
//        for (var i = 0; i < PositionIndex.Count.Int(); i++)
//        {
//            jointPoints[i].RattlingCheckFrame = 5;
//            jointPoints[i].VecNow3DMagnitude = 0;
//            jointPoints[i].Threshold = config.OtherThreshold;
//            jointPoints[i].Smooth = config.OtherSmooth;
//            jointPoints[i].Ratio = config.OtherRatio;
//        }
   
//        jointPoints[PositionIndex.lShldrBend.Int()].RattlingCheckFrame = config.ShoulderRattlingCheckFrame;
//        jointPoints[PositionIndex.rShldrBend.Int()].RattlingCheckFrame = config.ShoulderRattlingCheckFrame;
//        jointPoints[PositionIndex.lThighBend.Int()].RattlingCheckFrame = config.ThighRattlingCheckFrame;
//        jointPoints[PositionIndex.rThighBend.Int()].RattlingCheckFrame = config.ThighRattlingCheckFrame;
//        jointPoints[PositionIndex.lShin.Int()].RattlingCheckFrame = config.FootRattlingCheckFrame;
//        jointPoints[PositionIndex.rShin.Int()].RattlingCheckFrame = config.FootRattlingCheckFrame;
//        jointPoints[PositionIndex.lFoot.Int()].RattlingCheckFrame = config.FootRattlingCheckFrame;
//        jointPoints[PositionIndex.rFoot.Int()].RattlingCheckFrame = config.FootRattlingCheckFrame;
//        jointPoints[PositionIndex.lToe.Int()].RattlingCheckFrame = config.FootRattlingCheckFrame;
//        jointPoints[PositionIndex.rToe.Int()].RattlingCheckFrame = config.FootRattlingCheckFrame;
//        jointPoints[PositionIndex.lForearmBend.Int()].RattlingCheckFrame = config.ArmRattlingCheckFrame;
//        jointPoints[PositionIndex.lHand.Int()].RattlingCheckFrame = config.ArmRattlingCheckFrame;
//        jointPoints[PositionIndex.lThumb2.Int()].RattlingCheckFrame = config.ArmRattlingCheckFrame;
//        jointPoints[PositionIndex.lMid1.Int()].RattlingCheckFrame = config.ArmRattlingCheckFrame;
//        jointPoints[PositionIndex.rForearmBend.Int()].RattlingCheckFrame = config.ArmRattlingCheckFrame;
//        jointPoints[PositionIndex.rHand.Int()].RattlingCheckFrame = config.ArmRattlingCheckFrame;
//        jointPoints[PositionIndex.rThumb2.Int()].RattlingCheckFrame = config.ArmRattlingCheckFrame;
//        jointPoints[PositionIndex.rMid1.Int()].RattlingCheckFrame = config.ArmRattlingCheckFrame;

//        jointPoints[PositionIndex.lShin.Int()].Threshold = config.ShinThreshold;
//        jointPoints[PositionIndex.rShin.Int()].Threshold = config.ShinThreshold;
//        jointPoints[PositionIndex.lShin.Int()].Smooth = config.ShinSmooth;
//        jointPoints[PositionIndex.rShin.Int()].Smooth = config.ShinSmooth;
//        jointPoints[PositionIndex.lShin.Int()].Ratio = config.ShinRatio;
//        jointPoints[PositionIndex.rShin.Int()].Ratio = config.ShinRatio;

//        jointPoints[PositionIndex.lHand.Int()].Threshold = config.ArmThreshold;
//        jointPoints[PositionIndex.lThumb2.Int()].Threshold = config.ArmThreshold;
//        jointPoints[PositionIndex.lMid1.Int()].Threshold = config.ArmThreshold;
//        jointPoints[PositionIndex.rHand.Int()].Threshold = config.ArmThreshold;
//        jointPoints[PositionIndex.rThumb2.Int()].Threshold = config.ArmThreshold;
//        jointPoints[PositionIndex.rMid1.Int()].Threshold = config.ArmThreshold;

//        jointPoints[PositionIndex.lHand.Int()].Smooth = config.ArmSmooth;
//        jointPoints[PositionIndex.lThumb2.Int()].Smooth = config.ArmSmooth;
//        jointPoints[PositionIndex.lMid1.Int()].Smooth = config.ArmSmooth;
//        jointPoints[PositionIndex.rHand.Int()].Smooth = config.ArmSmooth;
//        jointPoints[PositionIndex.rThumb2.Int()].Smooth = config.ArmSmooth;
//        jointPoints[PositionIndex.rMid1.Int()].Smooth = config.ArmSmooth;

//        jointPoints[PositionIndex.lHand.Int()].Ratio = config.ArmRatio;
//        jointPoints[PositionIndex.lThumb2.Int()].Ratio = config.ArmRatio;
//        jointPoints[PositionIndex.lMid1.Int()].Ratio = config.ArmRatio;
//        jointPoints[PositionIndex.rHand.Int()].Ratio = config.ArmRatio;
//        jointPoints[PositionIndex.rThumb2.Int()].Ratio = config.ArmRatio;
//        jointPoints[PositionIndex.rMid1.Int()].Ratio = config.ArmRatio;
//*/
//        //  LockFoot = config.LockFoot == 1;
//        //  LockLegs = config.LockLegs == 1;
//        //  LockHand = config.LockHand == 1;rThighBend
//        jointPoints[PositionIndex.rThighBend.Int()].Lock = LockFoot || LockLegs;
//        jointPoints[PositionIndex.lThighBend.Int()].Lock = LockFoot || LockLegs;

//        jointPoints[PositionIndex.lToe.Int()].Lock = LockFoot || LockLegs;
//        jointPoints[PositionIndex.rToe.Int()].Lock = LockFoot || LockLegs;
//        jointPoints[PositionIndex.lFoot.Int()].Lock = LockFoot || LockLegs;
//        jointPoints[PositionIndex.rFoot.Int()].Lock = LockFoot || LockLegs;
//        jointPoints[PositionIndex.lShin.Int()].Lock = LockLegs;
//        jointPoints[PositionIndex.rShin.Int()].Lock = LockLegs;
//        jointPoints[PositionIndex.lHand.Int()].Lock = LockHand;
//        jointPoints[PositionIndex.rHand.Int()].Lock = LockHand;

//        if (config.ElbowAxisTop == 0)
//        {
//            jointPoints[PositionIndex.rForearmBend.Int()].Parent = jointPoints[PositionIndex.rShldrBend.Int()];
//            jointPoints[PositionIndex.lForearmBend.Int()].Parent = jointPoints[PositionIndex.lShldrBend.Int()];
//        }
//    else
//        {
//            jointPoints[PositionIndex.rForearmBend.Int()].Parent = null;
//            jointPoints[PositionIndex.lForearmBend.Int()].Parent = null;
//        }
//    }

//    private float tallHeadNeck;
//    private float tallNeckSpine;
//    private float tallSpineCrotch;
//    private float tallThigh;
//    private float tallShin;
//    public float EstimatedScore;
//    private float VisibleThreshold = 0.05f;

//    public Vector3[] jointPointsoffex;

//    public void PoseUpdate()
//    {
//        // 身長からｚの移動量を算出
//        tallHeadNeck = Vector3.Distance(jointPoints[PositionIndex.head.Int()].Pos3D, jointPoints[PositionIndex.neck.Int()].Pos3D);
//        tallNeckSpine = Vector3.Distance(jointPoints[PositionIndex.neck.Int()].Pos3D, jointPoints[PositionIndex.spine.Int()].Pos3D);

//        jointPoints[PositionIndex.lToe.Int()].Visibled = jointPoints[PositionIndex.lToe.Int()].Score3D < VisibleThreshold ? false : true;
//        jointPoints[PositionIndex.rToe.Int()].Visibled = jointPoints[PositionIndex.rToe.Int()].Score3D < VisibleThreshold ? false : true;
//        jointPoints[PositionIndex.lFoot.Int()].Visibled = jointPoints[PositionIndex.lFoot.Int()].Score3D < VisibleThreshold ? false : true;
//        jointPoints[PositionIndex.rFoot.Int()].Visibled = jointPoints[PositionIndex.rFoot.Int()].Score3D < VisibleThreshold ? false : true;
//        jointPoints[PositionIndex.lShin.Int()].Visibled = jointPoints[PositionIndex.lShin.Int()].Score3D < VisibleThreshold ? false : true;
//        jointPoints[PositionIndex.rShin.Int()].Visibled = jointPoints[PositionIndex.rShin.Int()].Score3D < VisibleThreshold ? false : true;
//        jointPoints[PositionIndex.lThighBend.Int()].Visibled = jointPoints[PositionIndex.lThighBend.Int()].Score3D < VisibleThreshold ? false : true;
//        jointPoints[PositionIndex.rThighBend.Int()].Visibled = jointPoints[PositionIndex.rThighBend.Int()].Score3D < VisibleThreshold ? false : true;


//        jointPoints[PositionIndex.lHand.Int()].Visibled = jointPoints[PositionIndex.lHand.Int()].Score3D < VisibleThreshold ? false : true;
//        jointPoints[PositionIndex.rHand.Int()].Visibled = jointPoints[PositionIndex.rHand.Int()].Score3D < VisibleThreshold ? false : true;
//        LeftHand.SetActive(jointPoints[PositionIndex.lHand.Int()].Visibled);
//        RightHand.SetActive(jointPoints[PositionIndex.rHand.Int()].Visibled);



//        var leftShin = 0f;
//        var rightShin = 0f;
//        var shinCnt = 0;
//        if (jointPoints[PositionIndex.lShin.Int()].Visibled && jointPoints[PositionIndex.lFoot.Int()].Visibled)
//        {
//            leftShin = Vector3.Distance(jointPoints[PositionIndex.lShin.Int()].Pos3D, jointPoints[PositionIndex.lFoot.Int()].Pos3D);
//            shinCnt++;
//        }
//        if (jointPoints[PositionIndex.rShin.Int()].Visibled && jointPoints[PositionIndex.rFoot.Int()].Visibled)
//        {
//            rightShin = Vector3.Distance(jointPoints[PositionIndex.rShin.Int()].Pos3D, jointPoints[PositionIndex.rFoot.Int()].Pos3D);
//            shinCnt++;
//        }
//        if (shinCnt != 0)
//        {
//            tallShin = (rightShin + leftShin) / shinCnt;
//        }

//        var rightThigh = 0f;
//        var leftThigh = 0f;
//        var thighCnt = 0;
//        if (jointPoints[PositionIndex.rThighBend.Int()].Visibled && jointPoints[PositionIndex.rShin.Int()].Visibled)
//        {
//            rightThigh = Vector3.Distance(jointPoints[PositionIndex.rThighBend.Int()].Pos3D, jointPoints[PositionIndex.rShin.Int()].Pos3D);
//            thighCnt++;
//        }
//        if (jointPoints[PositionIndex.lThighBend.Int()].Visibled && jointPoints[PositionIndex.lShin.Int()].Visibled)
//        {
//            leftThigh = Vector3.Distance(jointPoints[PositionIndex.lThighBend.Int()].Pos3D, jointPoints[PositionIndex.lShin.Int()].Pos3D);
//            thighCnt++;
//        }
//        if (thighCnt != 0)
//        {
//            tallThigh = (rightThigh + leftThigh) / 2f;
//        }

//        var crotch = (jointPoints[PositionIndex.rThighBend.Int()].Pos3D + jointPoints[PositionIndex.lThighBend.Int()].Pos3D) / 2f;
//        tallSpineCrotch = Vector3.Distance(jointPoints[PositionIndex.spine.Int()].Pos3D, crotch);

//        if(tallThigh <= 0.01f && tallShin <= 0.01f)
//        {
//            tallThigh = tallNeckSpine;
//            tallShin = tallNeckSpine;
//        }
//        else if (tallShin <= 0.01f)
//        {
//            tallShin = tallThigh;
//        }
//        else if (tallThigh <= 0.01f)
//        {
//            tallThigh = tallShin;
//        }

//        var t = tallHeadNeck + tallNeckSpine + tallSpineCrotch + (tallThigh + tallShin) * UpperBodyF;

//        tall = t * 0.7f + prevTall * 0.3f;
//        prevTall = tall;

//        //var dz = (centerTall - tall) / centerTall * ZScale;
//        var dz = (tall / centerTall - 1f);

//        var score = 0f;
//        var scoreCnt = 0;
//        for (var i = 0; i < 24; i++)
//        {
//            if (!jointPoints[i].Visibled)
//            {
//                continue;
//            }

//            if (jointPoints[i].Child != null)
//            {
//                score += jointPoints[i].Score3D;
//                scoreCnt++;
//            }
//        }

//        if (scoreCnt > 0)
//        {
//            EstimatedScore = score / scoreCnt;
//        }
//        else
//        {
//            EstimatedScore = 0f;
//        }

//        if(EstimatedScore < 0.03f)
//        {
//            return;
//        }
//        // センターの移動と回転
//        var forward = TriangleNormal(jointPoints[PositionIndex.spine.Int()].Pos3D, jointPoints[PositionIndex.lThighBend.Int()].Pos3D, jointPoints[PositionIndex.rThighBend.Int()].Pos3D);

//        transform.localPosition = jointPoints[PositionIndex.hip.Int()].Pos3D * movementScale + new Vector3(initPosition.x, initPosition.y, initPosition.z - dz * ZScale);
       
//        jointPoints[PositionIndex.hip.Int()].Transform.localRotation = Quaternion.LookRotation(forward) * jointPoints[PositionIndex.hip.Int()].InverseRotation;

//        // 各ボーンの回転
//        foreach (var jointPoint in jointPoints)
//        {
//            if (this.UpperBodyMode && !jointPoint.UpperBody)
//            {
//                continue;
//            }
//            if (jointPoint.Lock)
//            {
//                if(LockLegs)
//                {
//                    if(jointPoint.Index == PositionIndex.lShin || jointPoint.Index == PositionIndex.rShin)
//                    {
//                        jointPoint.Transform.rotation = Quaternion.LookRotation(Vector3.up, forward) * jointPoint.InverseRotation;
//                    }
//                }
//                continue;
//            }
//            if (!jointPoint.Visibled)
//            {
//                continue;
//            }

//            if (jointPoint.Parent != null)
//            {
//                var fv = jointPoint.Parent.Pos3D - jointPoint.Pos3D;
//                jointPoint.Transform.rotation = Quaternion.LookRotation(jointPoint.Pos3D - jointPoint.Child.Pos3D, fv) * jointPoint.InverseRotation;
//            }
//            else if (jointPoint.Child != null)
//            {
//                if (!jointPoint.Child.Visibled)
//                {
//                    continue;
//                }
//             if(jointPoint.hand==false)
//                jointPoint.Transform.rotation = Quaternion.LookRotation(jointPoint.Pos3D - jointPoint.Child.Pos3D, forward) * jointPoint.InverseRotation;
//           //  else
//                  //  jointPoint.Transform.rotation = Quaternion.LookRotation(jointPoint.Pos3D - jointPoint.Child.Pos3D) * jointPoint.InverseRotation;


//            }
           
//        }


//            // Head Rotation
//            var gaze = jointPoints[PositionIndex.Nose.Int()].Pos3D - jointPoints[PositionIndex.head.Int()].Pos3D;
//        var f = TriangleNormal(jointPoints[PositionIndex.Nose.Int()].Pos3D, jointPoints[PositionIndex.rEar.Int()].Pos3D, jointPoints[PositionIndex.lEar.Int()].Pos3D);
//        var head = jointPoints[PositionIndex.head.Int()];
//        head.Transform.rotation = Quaternion.LookRotation(gaze, f) * head.InverseRotation;

//        // Wrist rotation (Test code)
//        var lHand = jointPoints[PositionIndex.lHand.Int()];
//        if (!lHand.Lock && lHand.Visibled)
//        {
//            var lf = TriangleNormal(lHand.Pos3D, jointPoints[PositionIndex.LeftLittleProximal .Int()].Pos3D, jointPoints[PositionIndex.LeftIndexProximal.Int()].Pos3D);
//            lHand.Transform.rotation = Quaternion.LookRotation(jointPoints[PositionIndex.LeftMiddleProximal.Int()].Pos3D - jointPoints[PositionIndex.lHand.Int()].Pos3D, lf) * lHand.InverseRotation;
//        }
//        var rHand = jointPoints[PositionIndex.rHand.Int()];

//        if (!rHand.Lock && rHand.Visibled)
//        {
//            var rf = TriangleNormal(rHand.Pos3D, jointPoints[PositionIndex.RightIndexProximal.Int()].Pos3D, jointPoints[PositionIndex.RightLittleProximal.Int()].Pos3D);
//            rHand.Transform.rotation = Quaternion.LookRotation(jointPoints[PositionIndex.RightMiddleProximal.Int()].Pos3D - jointPoints[PositionIndex.rHand.Int()].Pos3D, rf) * rHand.InverseRotation;
//        }
//        float d = Vector3.Distance(jointPoints[PositionIndex.RightIndexProximal.Int()].Pos3D, jointPoints[PositionIndex.rHand.Int()].Pos3D);
//        float dl = Vector3.Distance(jointPoints[PositionIndex.LeftIndexProximal.Int()].Pos3D, jointPoints[PositionIndex.lHand.Int()].Pos3D);

//        if (d == 0 || dl == 0)
//        {
//            d = dl = 0.1f;
//        }
//        // float d = Vector3.Distance(jointPoints[PositionIndex.RightIndexProximal.Int()].Pos3D, jointPoints[PositionIndex.RightLittleProximal.Int()].Pos3D);
//        RightThumbDistalTip.transform.position = jointPoints[PositionIndex.rHand.Int()].Transform.position + (jointPoints[PositionIndex.RightThumbDistalTip.Int()].Pos3D * handoffx / d)
//            + jointPoints[PositionIndex.rHand.Int()].Transform.TransformDirection(Vector3.forward*0.05f);
//        RightIndexDistalTip.transform.position = jointPoints[PositionIndex.rHand.Int()].Transform.position + (jointPoints[PositionIndex.RightIndexDistalTip.Int()].Pos3D * handoffx / d)
//            + jointPoints[PositionIndex.rHand.Int()].Transform.TransformDirection(Vector3.forward * 0.05f); ;

//        RightMiddleDistalTip.transform.position = jointPoints[PositionIndex.rHand.Int()].Transform.position + (jointPoints[PositionIndex.RightMiddleDistalTip.Int()].Pos3D * handoffx / d)
//            + jointPoints[PositionIndex.rHand.Int()].Transform.TransformDirection(Vector3.forward * 0.05f); ;

//        RightRingDistalTip.transform.position = jointPoints[PositionIndex.rHand.Int()].Transform.position + (jointPoints[PositionIndex.RightRingDistalTip.Int()].Pos3D * handoffx / d)
//            + jointPoints[PositionIndex.rHand.Int()].Transform.TransformDirection(Vector3.forward * 0.05f); ;

//        RightLittleDistalTip.transform.position = jointPoints[PositionIndex.rHand.Int()].Transform.position + (jointPoints[PositionIndex.RightLittleDistalTip.Int()].Pos3D * handoffx / d)
//            + jointPoints[PositionIndex.rHand.Int()].Transform.TransformDirection(Vector3.forward * 0.05f); ;


//        LeftThumbDistalTip.transform.position = jointPoints[PositionIndex.lHand.Int()].Transform.position + (jointPoints[PositionIndex.LeftThumbDistalTip.Int()].Pos3D * handoffx / dl) 
//            + jointPoints[PositionIndex.lHand.Int()].Transform.TransformDirection(Vector3.forward * 0.05f); 
//        LeftIndexDistalTip.transform.position = jointPoints[PositionIndex.lHand.Int()].Transform.position + (jointPoints[PositionIndex.LeftIndexDistalTip.Int()].Pos3D * handoffx / dl) 
//            + jointPoints[PositionIndex.lHand.Int()].Transform.TransformDirection(Vector3.forward * 0.05f);

//        LeftMiddleDistalTip.transform.position = jointPoints[PositionIndex.lHand.Int()].Transform.position + (jointPoints[PositionIndex.LeftMiddleDistalTip.Int()].Pos3D * handoffx / dl)
//             + jointPoints[PositionIndex.lHand.Int()].Transform.TransformDirection(Vector3.forward * 0.05f);

//        LeftRingDistalTip.transform.position = jointPoints[PositionIndex.lHand.Int()].Transform.position + (jointPoints[PositionIndex.LeftRingDistalTip.Int()].Pos3D * handoffx / dl)
//             + jointPoints[PositionIndex.lHand.Int()].Transform.TransformDirection(Vector3.forward * 0.05f);

//        LeftLittleDistalTip.transform.position = jointPoints[PositionIndex.lHand.Int()].Transform.position + (jointPoints[PositionIndex.LeftLittleDistalTip.Int()].Pos3D * handoffx / dl)
//             + jointPoints[PositionIndex.lHand.Int()].Transform.TransformDirection(Vector3.forward * 0.05f);

//        Root.transform.eulerAngles = jointPoints[PositionIndex.head.Int()].Transform.eulerAngles;

///*        Head.transform.position = jointPoints[PositionIndex.head.Int()].Transform.position;
//        LeftHand.transform.position = jointPoints[PositionIndex.lHand.Int()].Transform.position;
//        RightHand.transform.position = jointPoints[PositionIndex.rHand.Int()].Transform.position;

//        Head.transform.eulerAngles = jointPoints[PositionIndex.head.Int()].Transform.eulerAngles;
//        LeftHand.transform.eulerAngles = jointPoints[PositionIndex.lHand.Int()].Transform.eulerAngles;
//        RightHand.transform.eulerAngles = jointPoints[PositionIndex.rHand.Int()].Transform.eulerAngles;*/

//        foreach (var sk in Skeletons)
//        {
//            if (this.UpperBodyMode && !sk.upperBody)
//            {
//                continue;
//            }

//            var s = sk.start;
//            var e = sk.end;

//            sk.Line.SetPosition(0, new Vector3(s.Pos3D.x * SkeletonScale + SkeletonX, s.Pos3D.y * SkeletonScale + SkeletonY, s.Pos3D.z * SkeletonScale + SkeletonZ));
//            sk.Line.SetPosition(1, new Vector3(e.Pos3D.x * SkeletonScale + SkeletonX, e.Pos3D.y * SkeletonScale + SkeletonY, e.Pos3D.z * SkeletonScale + SkeletonZ));
//        }
//    }
//    public GameObject RightThumbDistalTip;
//    private GameObject RightIndexDistalTip;
//    private GameObject RightMiddleDistalTip;
//    private GameObject RightRingDistalTip;
//    private GameObject RightLittleDistalTip;

//    public GameObject LeftThumbDistalTip;
//    private GameObject LeftIndexDistalTip;
//    private GameObject LeftMiddleDistalTip;
//    private GameObject LeftRingDistalTip;
//    private GameObject LeftLittleDistalTip;

//    public GameObject Root;
//    public GameObject Head;
//    public GameObject LeftHand;
//    public GameObject RightHand;


//    private void Awake()
//    {
//        RightThumbDistalTip = GameObject.Find("RightThumbDistalTip");
//        RightIndexDistalTip = GameObject.Find("RightIndexDistalTip");
//        RightMiddleDistalTip = GameObject.Find("RightMiddleDistalTip");
//        RightRingDistalTip = GameObject.Find("RightRingDistalTip");
//        RightLittleDistalTip = GameObject.Find("RightLittleDistalTip");

//        LeftThumbDistalTip = GameObject.Find("LeftThumbDistalTip");
//        LeftIndexDistalTip = GameObject.Find("LeftIndexDistalTip");
//        LeftMiddleDistalTip = GameObject.Find("LeftMiddleDistalTip");
//        LeftRingDistalTip = GameObject.Find("LeftRingDistalTip");
//        LeftLittleDistalTip = GameObject.Find("LeftLittleDistalTip");


//        Root = GameObject.Find("Root");
///*        Head = GameObject.Find("Head_net");
//        LeftHand = GameObject.Find("LeftHand_net");
//        RightHand = GameObject.Find("RightHand_net");*/
//    }

//    private void OnDrawGizmos()
//    {/*
//        Gizmos.color = Color.red;
//        if (jointPoints == null )
//            return;

//        float d= Vector3.Distance(jointPoints[PositionIndex.RightIndexProximal.Int()].Pos3D, jointPoints[PositionIndex.RightLittleProximal.Int()].Pos3D);

//        for (int i = 0; i < jointPoints.Length; i++)
//        {
//            if (jointPoints[i].hand)
//            {
//                Gizmos.color = Color.green;
//                Gizmos.DrawSphere(jointPoints[PositionIndex.rHand .Int()].Transform.position+(jointPoints[i].Pos3D*handoffx/d), 0.01f);
//            }
//            else
//            {
//                Gizmos.DrawSphere(jointPoints[i].Pos3D, 0.02f);

//            }



//        }

//        */
//    }
//    Vector3 TriangleNormal(Vector3 a, Vector3 b, Vector3 c)
//    {
//        Vector3 d1 = a - b;
//        Vector3 d2 = a - c;

//        Vector3 dd = Vector3.Cross(d1, d2);
//        dd.Normalize();

//        return dd;
//    }

//    private Quaternion GetInverse(JointPoint p1, JointPoint p2, Vector3 forward)
//    {
//      //  if(p1.hand==false )
//         return Quaternion.Inverse(Quaternion.LookRotation(p1.Transform.position - p2.Transform.position, forward));
//      //  else
//      //      return Quaternion.Inverse(Quaternion.LookRotation(p2.Transform.position - p1.Transform.position));
//    }

//    private void AddSkeleton(PositionIndex s, PositionIndex e, bool upperBody)
//    {
//        var sk = new Skeleton()
//        {
//            LineObject = new GameObject(this.name + "_Skeleton" +  (Skeletons.Count + 1).ToString("00")),
//            start = jointPoints[s.Int()],
//            end = jointPoints[e.Int()],
//            upperBody = upperBody,
//        };

//        sk.Line = sk.LineObject.AddComponent<LineRenderer>();
//        sk.Line.startWidth = 0.04f;
//        sk.Line.endWidth = 0.01f;
//        //頂点の数を決める
//        sk.Line.positionCount = 2;
//        sk.Line.material = SkeletonMaterial;

//        Skeletons.Add(sk);
//    }

//    public static bool IsPoseUpdate = false;

//    private void Update()
//    {
//        if (jointPoints != null)
//        {
//           // if (IsPoseUpdate)
//            {
//                if(VRIK==null)
//                PoseUpdate();

//                if (VRIK != null)
//                    PoseUpdateVRIK();
//            }
//          //  IsPoseUpdate = false;
//        }
//    }


//    void PoseUpdateVRIK()
//    { 
    
//    }

//    void OnAnimatorIK()
//    {
//        //　IKを使わない場合はこれ以降なにもしない
//        if (!useIK)
//        {
//            return;
//        }

//        //　アニメーションパラメータからIKのウエイトを取得
//        rightFootWeight = 1f;
//        leftFootWeight = 1f;
//        //rightFootWeight = anim.GetFloat("RightFootWeight");
//        //leftFootWeight = anim.GetFloat("LeftFootWeight");

//        //　右足用のレイの視覚化
//        Debug.DrawRay(anim.GetIKPosition(AvatarIKGoal.RightFoot) + rayPositionOffset, -transform.up * rayRange, Color.red);
//        //　右足用のレイを飛ばす処理
//        var ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + rayPositionOffset, -transform.up);

//        RaycastHit hit;

//        if (Physics.Raycast(ray, out hit, rayRange))
//        {
//            rightFootPos = hit.point;
            
//            //　右足IKの設定
//            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
//            anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos + new Vector3(0f, offset, 0f));
//            if (useIKRot)
//            {
//                rightFootRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
//                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeight);
//                anim.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
//            }
//        }

//        //　左足用のレイを飛ばす処理
//        ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + rayPositionOffset, -transform.up);
//        //　左足用のレイの視覚化
//        Debug.DrawRay(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + rayPositionOffset, -transform.up * rayRange, Color.red);

//        if (Physics.Raycast(ray, out hit, rayRange))
//        {
//            leftFootPos = hit.point;

//            //　左足IKの設定
//            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
//            anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos + new Vector3(0f, offset, 0f));

//            if (useIKRot)
//            {
//                leftFootRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
//                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
//                anim.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRot);
//            }
//        }
//    }
//}
