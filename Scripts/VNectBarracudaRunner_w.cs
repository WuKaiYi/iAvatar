//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;
//// using Unity.Barracuda;
//using Unity.Mathematics;

///// <summary>
///// Define Joint points
///// </summary>
//public class VNectBarracudaRunner_w : MonoBehaviour
//{
//    /// <summary>
//    /// Neural network model
//    /// </summary>

//    public bool Verbose = true;

//    public VNectModel_w VNectModel_w;





//    /// <summary>
//    /// Coordinates of joint points
//    /// </summary>
//    private VNectModel_w.JointPoint_w[] JointPoint_ws;
    
//    /// <summary>
//    /// Number of joint points
//    /// </summary>
//    private const int JointNum = 24;

//    /// <summary>
//    /// input image size
//    /// </summary>
//    public int InputImageSize;

//    /// <summary>
//    /// input image size (half)
//    /// </summary>
//    private float InputImageSizeHalf;

//    /// <summary>
//    /// column number of heatmap
//    /// </summary>
//    public int HeatMapCol;
//    private float InputImageSizeF;

//    /// <summary>
//    /// Column number of heatmap in 2D image
//    /// </summary>
//    private int HeatMapCol_Squared;
    
//    /// <summary>
//    /// Column nuber of heatmap in 3D model
//    /// </summary>
//    private int HeatMapCol_Cube;
//    private float ImageScale;

//    /// <summary>
//    /// Buffer memory has 2D heat map
//    /// </summary>
//    private float[] heatMap2D;

//    /// <summary>
//    /// Buffer memory has offset 2D
//    /// </summary>
//    private float[] offset2D;
    
//    /// <summary>
//    /// Buffer memory has 3D heat map
//    /// </summary>
//    private float[] heatMap3D;
    
//    /// <summary>
//    /// Buffer memory hash 3D offset
//    /// </summary>
//    private float[] offset3D;
//    private float unit;
    
//    /// <summary>
//    /// Number of joints in 2D image
//    /// </summary>
//    private int JointNum_Squared = JointNum * 2;
    
//    /// <summary>
//    /// Number of joints in 3D model
//    /// </summary>
//    private int JointNum_Cube = JointNum * 3;

//    /// <summary>
//    /// HeatMapCol * JointNum
//    /// </summary>
//    private int HeatMapCol_JointNum;

//    /// <summary>
//    /// HeatMapCol * JointNum_Squared
//    /// </summary>
//    private int CubeOffsetLinear;

//    /// <summary>
//    /// HeatMapCol * JointNum_Cube
//    /// </summary>
//    private int CubeOffsetSquared;

//    /// <summary>
//    /// For Kalman filter parameter Q
//    /// </summary>
//    public float KalmanParamQ;

//    /// <summary>
//    /// For Kalman filter parameter R
//    /// </summary>
//    public float KalmanParamR;

//    /// <summary>
//    /// Lock to update VNectModel_w
//    /// </summary>
//    private bool Lock = true;

//    /// <summary>
//    /// Use low pass filter flag
//    /// </summary>
//    public bool UseLowPassFilter;

//    /// <summary>
//    /// For low pass filter
//    /// </summary>
//    public float LowPassParam;

//    public Text Msg;
//    public float WaitTimeModelLoad = 10f;
//    private float Countdown = 0;
//    public Texture2D InitImg;

//    public Transform debug;

//    private void Start()
//    {
//        // Initialize 
    

//        heatMap2D = new float[JointNum * HeatMapCol_Squared];
//        offset2D = new float[JointNum * HeatMapCol_Squared * 2];
//        heatMap3D = new float[JointNum * HeatMapCol_Cube];
//        offset3D = new float[JointNum * HeatMapCol_Cube * 3];
//        unit = 1f / (float)HeatMapCol;
//        InputImageSizeF = InputImageSize;
//        InputImageSizeHalf = InputImageSizeF / 2f;
//        ImageScale = InputImageSize / (float)HeatMapCol;// 224f / (float)InputImageSize;

//        // Disabel sleep
//        Screen.sleepTimeout = SleepTimeout.NeverSleep;


//       // JointPoint_ws = VNectModel_w.Init();
//        StartCoroutine("WaitLoad");

//    }

//    private void Update()
//    {
//        if (!Lock)
//        {
         
//        }
//    }

//    private IEnumerator WaitLoad()
//    {


//        // Init VNect model
//        JointPoint_ws = VNectModel_w.Init();


//        yield return new WaitForSeconds(WaitTimeModelLoad);

//        // Init VideoCapture
//      //  videoCapture.Init(InputImageSize, InputImageSize);
//        Lock = false;
       
//    }



//    public float x_offex, z_offex, y_offex=1;


//    VNectModel_w.JointPoint_w JointPoint_w_set(VNectModel_w.JointPoint_w JointPoint_w,float4 f)
//    {
//        JointPoint_w.Now3D.x = f.x*x_offex;
//        JointPoint_w.Now3D.y = f.y*y_offex;
//        JointPoint_w.Now3D.z = f.z*z_offex;
//        JointPoint_w.score3D = f.w;
//        return JointPoint_w;
//    }

//    /// <summary>
//    /// Predict positions of each of joints based on network
//    /// </summary>
//    public void PredictPose(float4[] f)
//    {
//        if (Lock)
//                return;
//        JointPoint_w_set(JointPoint_ws[PositionIndex.rShldrBend.Int()], f[12]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.rForearmBend.Int()], f[14]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.rHand.Int()], f[16]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.rThumb2.Int()], f[22]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.rMid1.Int()], f[20]);

//        JointPoint_w_set(JointPoint_ws[PositionIndex.lShldrBend.Int()], f[11]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.lForearmBend.Int()], f[13]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.lHand.Int()], f[15]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.lThumb2.Int()], f[21]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.lMid1.Int()], f[19]);

//        JointPoint_w_set(JointPoint_ws[PositionIndex.lEar.Int()], f[7]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.lEye.Int()], f[2]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.rEar.Int()], f[8]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.rEye.Int()], f[5]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.Nose.Int()], f[0]);

//        JointPoint_w_set(JointPoint_ws[PositionIndex.rThighBend.Int()], f[24]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.rShin.Int()], f[26]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.rFoot.Int()], f[28]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.rToe.Int()], f[32]);

//        JointPoint_w_set(JointPoint_ws[PositionIndex.lThighBend.Int()], f[23]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.lShin.Int()], f[25]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.lFoot.Int()], f[27]);
//        JointPoint_w_set(JointPoint_ws[PositionIndex.lToe.Int()], f[31]);



//        JointPoint_ws[PositionIndex.abdomenUpper.Int()].Now3D = (new Vector3(f[11].x + f[24].x, f[11].y + f[24].y, f[11].z + f[24].z)/2);
       


//        // Calculate hip location
//        var lc = (JointPoint_ws[PositionIndex.rThighBend.Int()].Now3D + JointPoint_ws[PositionIndex.lThighBend.Int()].Now3D) / 2f;
//        JointPoint_ws[PositionIndex.hip.Int()].Now3D = (JointPoint_ws[PositionIndex.abdomenUpper.Int()].Now3D + lc) / 2f;

//        // Calculate neck location
//        JointPoint_ws[PositionIndex.neck.Int()].Now3D = (JointPoint_ws[PositionIndex.rShldrBend.Int()].Now3D + JointPoint_ws[PositionIndex.lShldrBend.Int()].Now3D) / 2f;

//        // Calculate head location
//        var cEar = (JointPoint_ws[PositionIndex.rEar.Int()].Now3D + JointPoint_ws[PositionIndex.lEar.Int()].Now3D) / 2f;
//        var hv = cEar - JointPoint_ws[PositionIndex.neck.Int()].Now3D;
//        var nhv = Vector3.Normalize(hv);
//        var nv = JointPoint_ws[PositionIndex.Nose.Int()].Now3D - JointPoint_ws[PositionIndex.neck.Int()].Now3D;
//        JointPoint_ws[PositionIndex.head.Int()].Now3D = JointPoint_ws[PositionIndex.neck.Int()].Now3D + nhv * Vector3.Dot(nhv, nv);

//        // Calculate spine location
//        JointPoint_ws[PositionIndex.spine.Int()].Now3D = JointPoint_ws[PositionIndex.abdomenUpper.Int()].Now3D;

//        // Kalman filter
//        foreach (var jp in JointPoint_ws)
//        {
//            KalmanUpdate(jp);
//        }

//        // Low pass filter
//        if (UseLowPassFilter)
//        {
//            foreach (var jp in JointPoint_ws)
//            {
//                jp.PrevPos3D[0] = jp.Pos3D;
//                for (var i = 1; i < jp.PrevPos3D.Length; i++)
//                {
//                    jp.PrevPos3D[i] = jp.PrevPos3D[i] * LowPassParam + jp.PrevPos3D[i - 1] * (1f - LowPassParam);
//                }
//                jp.Pos3D = jp.PrevPos3D[jp.PrevPos3D.Length - 1];
//            }
//        }

//        if (debug != null)
//            debug.transform.position = JointPoint_ws[PositionIndex.hip.Int()].Now3D;
//    }

//    /// <summary>
//    /// Kalman filter
//    /// </summary>
//    /// <param name="measurement">joint points</param>
//    void KalmanUpdate(VNectModel_w.JointPoint_w measurement)
//    {
//        measurementUpdate(measurement);
//        measurement.Pos3D.x = measurement.X.x + (measurement.Now3D.x - measurement.X.x) * measurement.K.x;
//        measurement.Pos3D.y = measurement.X.y + (measurement.Now3D.y - measurement.X.y) * measurement.K.y;
//        measurement.Pos3D.z = measurement.X.z + (measurement.Now3D.z - measurement.X.z) * measurement.K.z;
//        measurement.X = measurement.Pos3D;
//    }

//	void measurementUpdate(VNectModel_w.JointPoint_w measurement)
//    {
//        measurement.K.x = (measurement.P.x + KalmanParamQ) / (measurement.P.x + KalmanParamQ + KalmanParamR);
//        measurement.K.y = (measurement.P.y + KalmanParamQ) / (measurement.P.y + KalmanParamQ + KalmanParamR);
//        measurement.K.z = (measurement.P.z + KalmanParamQ) / (measurement.P.z + KalmanParamQ + KalmanParamR);
//        measurement.P.x = KalmanParamR * (measurement.P.x + KalmanParamQ) / (KalmanParamR + measurement.P.x + KalmanParamQ);
//        measurement.P.y = KalmanParamR * (measurement.P.y + KalmanParamQ) / (KalmanParamR + measurement.P.y + KalmanParamQ);
//        measurement.P.z = KalmanParamR * (measurement.P.z + KalmanParamQ) / (KalmanParamR + measurement.P.z + KalmanParamQ);
//    }
//}
