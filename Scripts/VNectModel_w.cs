using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Position index of joint points
/// </summary>
public enum PositionIndex_w : int
{
    rShldrBend = 0,
    rForearmBend,
    rHand,
    rThumb2,
    rMid1,

    lShldrBend,
    lForearmBend,
    lHand,
    lThumb2,
    lMid1,

    lEar,
    lEye,
    rEar,
    rEye,
    Nose,

    rThighBend,
    rShin,
    rFoot,
    rToe,

    lThighBend,
    lShin,
    lFoot,
    lToe,

    abdomenUpper,

    //Calculated coordinates
    hip,
    head,
    neck,
    spine,

    Count,
    None,
}

public static partial class EnumExtend
{
    public static int Int(this PositionIndex_w i)
    {
        return (int)i;
    }
}

public class VNectModel_w : MonoBehaviour
{
    public VNectModel_w Original_VNectModel;

    public class JointPoint_w
    {
        public Vector2 Pos2D = new Vector2();
        public float score2D;

        public Vector3 Pos3D = new Vector3();
        public Vector3 Now3D = new Vector3();
        public Vector3[] PrevPos3D = new Vector3[6];
        public float score3D;

        // Bones
        public Transform Transform = null;
        public Quaternion InitRotation;
        public Quaternion Inverse;
        public Quaternion InverseRotation;

        public JointPoint_w Child = null;
        public JointPoint_w Parent = null;

        // For Kalman filter
        public Vector3 P = new Vector3();
        public Vector3 X = new Vector3();
        public Vector3 K = new Vector3();
    }

    public class Skeleton
    {
        public GameObject LineObject;
        public LineRenderer Line;

        public JointPoint_w start = null;
        public JointPoint_w end = null;
    }

    private List<Skeleton> Skeletons = new List<Skeleton>();
    public Material SkeletonMaterial;

    public bool ShowSkeleton;
    private bool useSkeleton;
    public float SkeletonX;
    public float SkeletonY;
    public float SkeletonZ;
    public float SkeletonScale;

    // Joint position and bone
    private JointPoint_w[] JointPoints;
   // public JointPoint_w[] JointPoints { get { return JointPoints; } }

    private Vector3 initPosition; // Initial center position

    private Quaternion InitGazeRotation;
    private Quaternion gazeInverse;

    // UnityChan
    public GameObject ModelObject;
    public GameObject Nose;
    private Animator anim;

    // Move in z direction
    private float centerTall = 224 * 0.75f;
    private float tall = 224 * 0.75f;
    private float prevTall = 224 * 0.75f;
    public float ZScale = 0.8f;

    private void Update()
    {
        if (JointPoints != null)
        {
            PoseUpdate();
        }
    }

    /// <summary>
    /// Initialize joint points
    /// </summary>
    /// <returns></returns>
    public JointPoint_w[] Init()
    {
        JointPoints = new JointPoint_w[PositionIndex_w.Count.Int()];
        for (var i = 0; i < PositionIndex_w.Count.Int(); i++) JointPoints[i] = new JointPoint_w();

        anim = ModelObject.GetComponent<Animator>();

        // Right Arm
        JointPoints[PositionIndex_w.rShldrBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
        JointPoints[PositionIndex_w.rForearmBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
        JointPoints[PositionIndex_w.rHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightHand);
        JointPoints[PositionIndex_w.rThumb2.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightThumbIntermediate);
        JointPoints[PositionIndex_w.rMid1.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightMiddleProximal);
        // Left Arm
        JointPoints[PositionIndex_w.lShldrBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        JointPoints[PositionIndex_w.lForearmBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        JointPoints[PositionIndex_w.lHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftHand);
        JointPoints[PositionIndex_w.lThumb2.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate);
        JointPoints[PositionIndex_w.lMid1.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftMiddleProximal);

        // Face
        JointPoints[PositionIndex_w.lEar.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        JointPoints[PositionIndex_w.lEye.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftEye);
        JointPoints[PositionIndex_w.rEar.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        JointPoints[PositionIndex_w.rEye.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightEye);
        JointPoints[PositionIndex_w.Nose.Int()].Transform = Nose.transform;

        // Right Leg
        JointPoints[PositionIndex_w.rThighBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        JointPoints[PositionIndex_w.rShin.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        JointPoints[PositionIndex_w.rFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightFoot);
        JointPoints[PositionIndex_w.rToe.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightToes);

        // Left Leg
        JointPoints[PositionIndex_w.lThighBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        JointPoints[PositionIndex_w.lShin.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        JointPoints[PositionIndex_w.lFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        JointPoints[PositionIndex_w.lToe.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftToes);

        // etc
        JointPoints[PositionIndex_w.abdomenUpper.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Spine);
        JointPoints[PositionIndex_w.hip.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Hips);
        JointPoints[PositionIndex_w.head.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        JointPoints[PositionIndex_w.neck.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Neck);
        JointPoints[PositionIndex_w.spine.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Spine);

        // Child Settings
        // Right Arm
        JointPoints[PositionIndex_w.rShldrBend.Int()].Child = JointPoints[PositionIndex_w.rForearmBend.Int()];
        JointPoints[PositionIndex_w.rForearmBend.Int()].Child = JointPoints[PositionIndex_w.rHand.Int()];
        JointPoints[PositionIndex_w.rForearmBend.Int()].Parent = JointPoints[PositionIndex_w.rShldrBend.Int()];

        // Left Arm
        JointPoints[PositionIndex_w.lShldrBend.Int()].Child = JointPoints[PositionIndex_w.lForearmBend.Int()];
        JointPoints[PositionIndex_w.lForearmBend.Int()].Child = JointPoints[PositionIndex_w.lHand.Int()];
        JointPoints[PositionIndex_w.lForearmBend.Int()].Parent = JointPoints[PositionIndex_w.lShldrBend.Int()];

        // Fase

        // Right Leg
        JointPoints[PositionIndex_w.rThighBend.Int()].Child = JointPoints[PositionIndex_w.rShin.Int()];
        JointPoints[PositionIndex_w.rShin.Int()].Child = JointPoints[PositionIndex_w.rFoot.Int()];
        JointPoints[PositionIndex_w.rFoot.Int()].Child = JointPoints[PositionIndex_w.rToe.Int()];
        JointPoints[PositionIndex_w.rFoot.Int()].Parent = JointPoints[PositionIndex_w.rShin.Int()];

        // Left Leg
        JointPoints[PositionIndex_w.lThighBend.Int()].Child = JointPoints[PositionIndex_w.lShin.Int()];
        JointPoints[PositionIndex_w.lShin.Int()].Child = JointPoints[PositionIndex_w.lFoot.Int()];
        JointPoints[PositionIndex_w.lFoot.Int()].Child = JointPoints[PositionIndex_w.lToe.Int()];
        JointPoints[PositionIndex_w.lFoot.Int()].Parent = JointPoints[PositionIndex_w.lShin.Int()];

        // etc
        JointPoints[PositionIndex_w.spine.Int()].Child = JointPoints[PositionIndex_w.neck.Int()];
        JointPoints[PositionIndex_w.neck.Int()].Child = JointPoints[PositionIndex_w.head.Int()];
        //JointPoints[PositionIndex_w.head.Int()].Child = JointPoints[PositionIndex_w.Nose.Int()];

        useSkeleton = ShowSkeleton;
        if (useSkeleton)
        {
            // Line Child Settings
            // Right Arm
            AddSkeleton(PositionIndex_w.rShldrBend, PositionIndex_w.rForearmBend);
            AddSkeleton(PositionIndex_w.rForearmBend, PositionIndex_w.rHand);
            AddSkeleton(PositionIndex_w.rHand, PositionIndex_w.rThumb2);
            AddSkeleton(PositionIndex_w.rHand, PositionIndex_w.rMid1);

            // Left Arm
            AddSkeleton(PositionIndex_w.lShldrBend, PositionIndex_w.lForearmBend);
            AddSkeleton(PositionIndex_w.lForearmBend, PositionIndex_w.lHand);
            AddSkeleton(PositionIndex_w.lHand, PositionIndex_w.lThumb2);
            AddSkeleton(PositionIndex_w.lHand, PositionIndex_w.lMid1);

            // Fase
            AddSkeleton(PositionIndex_w.lEar, PositionIndex_w.Nose);
            AddSkeleton(PositionIndex_w.rEar, PositionIndex_w.Nose);

            // Right Leg
            AddSkeleton(PositionIndex_w.rThighBend, PositionIndex_w.rShin);
            AddSkeleton(PositionIndex_w.rShin, PositionIndex_w.rFoot);
            AddSkeleton(PositionIndex_w.rFoot, PositionIndex_w.rToe);

            // Left Leg
            AddSkeleton(PositionIndex_w.lThighBend, PositionIndex_w.lShin);
            AddSkeleton(PositionIndex_w.lShin, PositionIndex_w.lFoot);
            AddSkeleton(PositionIndex_w.lFoot, PositionIndex_w.lToe);

            // etc
            AddSkeleton(PositionIndex_w.spine, PositionIndex_w.neck);
            AddSkeleton(PositionIndex_w.neck, PositionIndex_w.head);
            AddSkeleton(PositionIndex_w.head, PositionIndex_w.Nose);
            AddSkeleton(PositionIndex_w.neck, PositionIndex_w.rShldrBend);
            AddSkeleton(PositionIndex_w.neck, PositionIndex_w.lShldrBend);
            AddSkeleton(PositionIndex_w.rThighBend, PositionIndex_w.rShldrBend);
            AddSkeleton(PositionIndex_w.lThighBend, PositionIndex_w.lShldrBend);
            AddSkeleton(PositionIndex_w.rShldrBend, PositionIndex_w.abdomenUpper);
            AddSkeleton(PositionIndex_w.lShldrBend, PositionIndex_w.abdomenUpper);
            AddSkeleton(PositionIndex_w.rThighBend, PositionIndex_w.abdomenUpper);
            AddSkeleton(PositionIndex_w.lThighBend, PositionIndex_w.abdomenUpper);
            AddSkeleton(PositionIndex_w.lThighBend, PositionIndex_w.rThighBend);
        }

        // Set Inverse
        var forward = TriangleNormal(JointPoints[PositionIndex_w.hip.Int()].Transform.position, JointPoints[PositionIndex_w.lThighBend.Int()].Transform.position, JointPoints[PositionIndex_w.rThighBend.Int()].Transform.position);
        foreach (var JointPoint_w in JointPoints)
        {
            if (JointPoint_w.Transform != null)
            {
                JointPoint_w.InitRotation = JointPoint_w.Transform.localRotation;
            }

            if (JointPoint_w.Child != null)
            {
                JointPoint_w.Inverse = GetInverse(JointPoint_w, JointPoint_w.Child, forward);
                JointPoint_w.InverseRotation = JointPoint_w.Inverse * JointPoint_w.InitRotation;
            }
        }
        var hip = JointPoints[PositionIndex_w.hip.Int()];
        initPosition = JointPoints[PositionIndex_w.hip.Int()].Transform.position;
        hip.Inverse = Quaternion.Inverse(Quaternion.LookRotation(forward));
        hip.InverseRotation = hip.Inverse * hip.InitRotation;

        // For Head Rotation
        var head = JointPoints[PositionIndex_w.head.Int()];
        head.InitRotation = JointPoints[PositionIndex_w.head.Int()].Transform.rotation;
        var gaze = JointPoints[PositionIndex_w.Nose.Int()].Transform.position - JointPoints[PositionIndex_w.head.Int()].Transform.position;
        head.Inverse = Quaternion.Inverse(Quaternion.LookRotation(gaze));
        head.InverseRotation = head.Inverse * head.InitRotation;
        
        var lHand = JointPoints[PositionIndex_w.lHand.Int()];
        var lf = TriangleNormal(lHand.Pos3D, JointPoints[PositionIndex_w.lMid1.Int()].Pos3D, JointPoints[PositionIndex_w.lThumb2.Int()].Pos3D);
        lHand.InitRotation = lHand.Transform.rotation;
        lHand.Inverse = Quaternion.Inverse(Quaternion.LookRotation(JointPoints[PositionIndex_w.lThumb2.Int()].Transform.position - JointPoints[PositionIndex_w.lMid1.Int()].Transform.position, lf));
        lHand.InverseRotation = lHand.Inverse * lHand.InitRotation;

        var rHand = JointPoints[PositionIndex_w.rHand.Int()];
        var rf = TriangleNormal(rHand.Pos3D, JointPoints[PositionIndex_w.rThumb2.Int()].Pos3D, JointPoints[PositionIndex_w.rMid1.Int()].Pos3D);
        rHand.InitRotation = JointPoints[PositionIndex_w.rHand.Int()].Transform.rotation;
        rHand.Inverse = Quaternion.Inverse(Quaternion.LookRotation(JointPoints[PositionIndex_w.rThumb2.Int()].Transform.position - JointPoints[PositionIndex_w.rMid1.Int()].Transform.position, rf));
        rHand.InverseRotation = rHand.Inverse * rHand.InitRotation;

        JointPoints[PositionIndex_w.hip.Int()].score3D = 1f;
        JointPoints[PositionIndex_w.neck.Int()].score3D = 1f;
        JointPoints[PositionIndex_w.Nose.Int()].score3D = 1f;
        JointPoints[PositionIndex_w.head.Int()].score3D = 1f;
        JointPoints[PositionIndex_w.spine.Int()].score3D = 1f;


        return JointPoints;
    }

    public void PoseUpdate()
    {
        // caliculate movement range of z-coordinate from height
        var t1 = Vector3.Distance(JointPoints[PositionIndex_w.head.Int()].Pos3D, JointPoints[PositionIndex_w.neck.Int()].Pos3D);
        var t2 = Vector3.Distance(JointPoints[PositionIndex_w.neck.Int()].Pos3D, JointPoints[PositionIndex_w.spine.Int()].Pos3D);
        var pm = (JointPoints[PositionIndex_w.rThighBend.Int()].Pos3D + JointPoints[PositionIndex_w.lThighBend.Int()].Pos3D) / 2f;
        var t3 = Vector3.Distance(JointPoints[PositionIndex_w.spine.Int()].Pos3D, pm);
        var t4r = Vector3.Distance(JointPoints[PositionIndex_w.rThighBend.Int()].Pos3D, JointPoints[PositionIndex_w.rShin.Int()].Pos3D);
        var t4l = Vector3.Distance(JointPoints[PositionIndex_w.lThighBend.Int()].Pos3D, JointPoints[PositionIndex_w.lShin.Int()].Pos3D);
        var t4 = (t4r + t4l) / 2f;
        var t5r = Vector3.Distance(JointPoints[PositionIndex_w.rShin.Int()].Pos3D, JointPoints[PositionIndex_w.rFoot.Int()].Pos3D);
        var t5l = Vector3.Distance(JointPoints[PositionIndex_w.lShin.Int()].Pos3D, JointPoints[PositionIndex_w.lFoot.Int()].Pos3D);
        var t5 = (t5r + t5l) / 2f;
        var t = t1 + t2 + t3 + t4 + t5;


        // Low pass filter in z direction
        tall = t * 0.7f + prevTall * 0.3f;
        prevTall = tall;

        if (tall == 0)
        {
            tall = centerTall;
        }
        var dz = (centerTall - tall) / centerTall * ZScale;

        // movement and rotatation of center
        var forward = TriangleNormal(JointPoints[PositionIndex_w.hip.Int()].Pos3D, JointPoints[PositionIndex_w.lThighBend.Int()].Pos3D, JointPoints[PositionIndex_w.rThighBend.Int()].Pos3D);
        JointPoints[PositionIndex_w.hip.Int()].Transform.position = JointPoints[PositionIndex_w.hip.Int()].Pos3D * 0.005f + new Vector3(initPosition.x, initPosition.y, initPosition.z + dz);
        JointPoints[PositionIndex_w.hip.Int()].Transform.localRotation = Quaternion.LookRotation(forward) * JointPoints[PositionIndex_w.hip.Int()].InverseRotation;

        // rotate each of bones
        foreach (var JointPoint_w in JointPoints)
        {
            if (JointPoint_w.Parent != null)
            {
                var fv = JointPoint_w.Parent.Pos3D - JointPoint_w.Pos3D;
                JointPoint_w.Transform.localRotation = Quaternion.LookRotation(JointPoint_w.Pos3D - JointPoint_w.Child.Pos3D, fv) * JointPoint_w.InverseRotation;
            }
            else if (JointPoint_w.Child != null)
            {
                JointPoint_w.Transform.localRotation = Quaternion.LookRotation(JointPoint_w.Pos3D - JointPoint_w.Child.Pos3D, forward) * JointPoint_w.InverseRotation;
            }
        }

        // Head Rotation
        var gaze = JointPoints[PositionIndex_w.Nose.Int()].Pos3D - JointPoints[PositionIndex_w.head.Int()].Pos3D;
        var f = TriangleNormal(JointPoints[PositionIndex_w.Nose.Int()].Pos3D, JointPoints[PositionIndex_w.rEar.Int()].Pos3D, JointPoints[PositionIndex_w.lEar.Int()].Pos3D);
        var head = JointPoints[PositionIndex_w.head.Int()];
        head.Transform.localRotation = Quaternion.LookRotation(gaze, f) * head.InverseRotation;
        
        // Wrist rotation (Test code)
        var lHand = JointPoints[PositionIndex_w.lHand.Int()];
        var lf = TriangleNormal(lHand.Pos3D, JointPoints[PositionIndex_w.lMid1.Int()].Pos3D, JointPoints[PositionIndex_w.lThumb2.Int()].Pos3D);
        lHand.Transform.rotation = Quaternion.LookRotation(JointPoints[PositionIndex_w.lThumb2.Int()].Pos3D - JointPoints[PositionIndex_w.lMid1.Int()].Pos3D, lf) * lHand.InverseRotation;

        var rHand = JointPoints[PositionIndex_w.rHand.Int()];
        var rf = TriangleNormal(rHand.Pos3D, JointPoints[PositionIndex_w.rThumb2.Int()].Pos3D, JointPoints[PositionIndex_w.rMid1.Int()].Pos3D);
        //rHand.Transform.rotation = Quaternion.LookRotation(JointPoints[PositionIndex_w.rThumb2.Int()].Pos3D - JointPoints[PositionIndex_w.rMid1.Int()].Pos3D, rf) * rHand.InverseRotation;
        rHand.Transform.rotation = Quaternion.LookRotation(JointPoints[PositionIndex_w.rThumb2.Int()].Pos3D - JointPoints[PositionIndex_w.rMid1.Int()].Pos3D, rf) * rHand.InverseRotation;

        foreach (var sk in Skeletons)
        {
            var s = sk.start;
            var e = sk.end;

            sk.Line.SetPosition(0, new Vector3(s.Pos3D.x * SkeletonScale + SkeletonX, s.Pos3D.y * SkeletonScale + SkeletonY, s.Pos3D.z * SkeletonScale + SkeletonZ));
            sk.Line.SetPosition(1, new Vector3(e.Pos3D.x * SkeletonScale + SkeletonX, e.Pos3D.y * SkeletonScale + SkeletonY, e.Pos3D.z * SkeletonScale + SkeletonZ));
        }
    }

    Vector3 TriangleNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 d1 = a - b;
        Vector3 d2 = a - c;

        Vector3 dd = Vector3.Cross(d1, d2);
        dd.Normalize();

        return dd;
    }

    private Quaternion GetInverse(JointPoint_w p1, JointPoint_w p2, Vector3 forward)
    {
        return Quaternion.Inverse(Quaternion.LookRotation(p1.Transform.position - p2.Transform.position, forward));
    }

    /// <summary>
    /// Add skelton from joint points
    /// </summary>
    /// <param name="s">position index</param>
    /// <param name="e">position index</param>
    private void AddSkeleton(PositionIndex_w s, PositionIndex_w e)
    {
        var sk = new Skeleton()
        {
            LineObject = new GameObject("Line"),
            start = JointPoints[s.Int()],
            end = JointPoints[e.Int()],
        };

        sk.Line = sk.LineObject.AddComponent<LineRenderer>();
        sk.Line.startWidth = 0.04f;
        sk.Line.endWidth = 0.01f;
        
        // define the number of vertex
        sk.Line.positionCount = 2;
        sk.Line.material = SkeletonMaterial;

        Skeletons.Add(sk);
    }
}
