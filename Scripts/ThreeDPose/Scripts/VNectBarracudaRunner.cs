//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;
//using System;
//using System.IO;
//// using Unity.Barracuda;
//using System.Text;
//using Unity.Mathematics;

//public class VNectBarracudaRunner : MonoBehaviour
//{
//    public NNModel NNModel;
//    public WorkerFactory.Type WorkerType = WorkerFactory.Type.Auto;
//    public bool Verbose = false;

//    public VNectModel VNectModel;

//    public HandTracking HandTracking;
//    public UnityChanController unityChanController;
//    public VideoCapture videoCapture;

//    private Model _model;
//    private IWorker _worker;
//    private VNectModel.JointPoint[] jointPoints;
//    private const int JointNum = 24;
//    private const int JointNum_Squared = JointNum * 2;
//    private const int JointNum_Cube = JointNum * 3;

//    public int InputImageSize;
//    private float InputImageSizeF;
//    private float InputImageSizeHalf;
//    public int HeatMapCol;
//    public int HeatMapCol_Half;
//    private int HeatMapCol_Squared;
//    private int HeatMapCol_Cube;
//    private int HeatMapCol_JointNum;
//    private float[] heatMap2D;
//    private float[] offset2D;
//    private float[] heatMap3D;
//    private float[] offset3D;
//    private float unit;

//    private int cubeOffsetLinear;
//    private int cubeOffsetSquared;

//    private bool Lock = true;
//    private float waitSec = 1f / 30f;

//    private float elapsedMeasurementSec = 0f;
//    private float fpsMeasurementSec = 0f;
//    public float FPS = 0f;
//    private int fpsCounter = 0;

//    public bool UseLPF;
//    public float Smooth;
//    public bool UseKalmanF;
//    public float KalmanParamQ;
//    public float KalmanParamR;
//    public float ForwardThreshold;
//    public float BackwardThreshold;
//    public int NOrderLPF;
//    private List<FIRFilter> filter = new List<FIRFilter>();
//    private FilterWindow filterWindow = new FilterWindow();

//    private delegate void UpdateVNectModelDelegate();
//    private UpdateVNectModelDelegate UpdateVNectModel;
//    public int ModelQuality = 1;
//    private string HighQualityModelName = "HighQualityTrainedModel.nn";

//    [SerializeField]
//    private float EstimatedScore;

//    public bool DebugMode;
//    public bool User3Input;
//    public bool UpperBodyMode;

//    public GameObject nose;

//    StreamWriter writer;

//    private void Start()
//    {


//        StartCoroutine("WaitLoad");

       
//    }

//    public void InitVNectModel(VNectModel avatar, ConfigurationSetting config)
//    {
//        VNectModel = avatar;
//        jointPoints = VNectModel.Init(InputImageSize, config);

//    }

//    public void Exit()
//    {
//        Lock = true;
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#elif UNITY_STANDALONE
//      UnityEngine.Application.Quit();
//#endif
//    }

//    public void SetVNectModel(VNectModel avatar)
//    {
//        VNectModel = avatar;
//        jointPoints = avatar.JointPoints;

//        VNectModel.Show();
//    }

//    public void VideoPlayStart(string path)
//    {
//        Lock = true;
//        VNectModel.IsPoseUpdate = false;
//        waitSec = 1f / videoCapture.SourceFps * 0.85f;

//        StartCoroutine(videoCapture.VideoStart(path));
//    }

//    public void videoCapture_VideoReady()
//    {
//        Lock = false;
//    }

//    public void CameraPlayStart(int index)
//    {
//        Lock = false;
//        waitSec = 1f / videoCapture.SourceFps * 0.85f;

//        videoCapture.CameraPlayStart(index);
//    }

//    public void PlayStop()
//    {
//        Lock = true;
//        videoCapture.PlayStop();
//    }

//    public void PlayPause()
//    {
//        Lock = true;
//        videoCapture.Pause();
//        //StartCoroutine(PlayPauseAsync());
//    }
//    public void Resume()
//    {
//        Lock = false;
//        videoCapture.Resume();
//    }
//    /*
//    private IEnumerator PlayPauseAsync()
//    {
//        yield return new WaitForSeconds(1f);
//        videoCapture.Pause();
//    }
//    */
//    public Vector3 GetHeadPosition()
//    {
//        return VNectModel.GetHeadPosition();
//    }

//    public void SetPredictSetting(ConfigurationSetting config)
//    {
//        Smooth = config.LowPassFilter;
//        NOrderLPF = config.NOrderLPF;

//        filterWindow.Init(config.FIROrderN03, config.FIRFromHz, config.FIRToHz, 30f);

//        filter.Clear();
//        for (var i = 0; i < JointNum; i++)
//        {
//            filter.Add(new FIRFilter(filterWindow, config.RangePathFilterBuffer03));
//        }

//        ForwardThreshold = config.ForwardThreshold;
//        BackwardThreshold = config.BackwardThreshold;

//        if(VNectModel != null)
//        {
//            VNectModel.SetPredictSetting(config);
//        }
//    }

//    private void Update()
//    {
 
//    }

//    private IEnumerator WaitLoad()
//    {
//        yield return new WaitForSeconds(1);
//        Lock = false;
//    }

//    private void UpdateVNect()
//    {
//        ExecuteModel();
//    //    PredictPose();

//        fpsCounter++;
//    }

//    private void ExecuteModel()
//    {

//        // Create input and Execute model
//        input = new Tensor(videoCapture.MainTexture, 3);
//        _worker.Execute(input);
//        input.Dispose();

//        // Get outputs
//        for (var i = 2; i < _model.outputs.Count; i++)
//        {
//            b_outputs[i] = _worker.PeekOutput(_model.outputs[i]);
//        }

//        // Get data from outputs
//        //heatMap2D = b_outputs[0].data.Download(b_outputs[0].shape);
//        //offset2D = b_outputs[1].data.Download(b_outputs[1].shape);
//        offset3D = b_outputs[2].data.Download(b_outputs[2].shape);
//        heatMap3D = b_outputs[3].data.Download(b_outputs[3].shape);
//    }

//    private void UpdateVNectAsync()
//    {
//        input = new Tensor(videoCapture.MainTexture, 3);
//        if (inputs[inputName_1] == null)
//        {
//            inputs[inputName_1] = input;
//            inputs[inputName_2] = new Tensor(videoCapture.MainTexture, 3);
//            inputs[inputName_3] = new Tensor(videoCapture.MainTexture, 3);
//        }
//        else
//        {/*
//            inputs[inputName_3].Dispose();
//            inputs[inputName_1] = input;
//            inputs[inputName_2] = input;
//            inputs[inputName_3] = input;
//            */
//            /**/
//             inputs[inputName_3].Dispose();
            
//             inputs[inputName_3] = inputs[inputName_2];
//              inputs[inputName_2] = inputs[inputName_1];
//              inputs[inputName_1] = input;
//             /* */
//        }

//        if (!Lock && videoCapture.IsPlay())
//        {
//            StartCoroutine(ExecuteModelAsync());
//        }
//    }
    
//    private const string inputName_1 = "input.1";
//    private const string inputName_2 = "input.4";
//    private const string inputName_3 = "input.7";
//    /*
//    private const string inputName_1 = "input.1";
//    private const string inputName_2 = "input.3";
//    private const string inputName_3 = "input.5";
//    */
//    /*
//    private const string inputName_1 = "0";
//    private const string inputName_2 = "1";
//    private const string inputName_3 = "2";
//    */

//    Tensor input = new Tensor();
//    Dictionary<string, Tensor> inputs = new Dictionary<string, Tensor>() { { inputName_1, null }, { inputName_2, null }, { inputName_3, null }, };
//    Tensor[] b_outputs = new Tensor[4];

//    private IEnumerator ExecuteModelAsync()
//    {
//        if(Lock)
//        {
//            yield return null;
//        }

//        // Create input and Execute model
//        yield return _worker.StartManualSchedule(inputs);

//        if (!Lock)
//        {
//            // Get outputs
//            for (var i = 2; i < _model.outputs.Count; i++)
//            {
//                b_outputs[i] = _worker.PeekOutput(_model.outputs[i]);
//            }

//            // Get data from outputs
//            //heatMap2D = b_outputs[0].data.Download(b_outputs[0].shape);
//            //offset2D = b_outputs[1].data.Download(b_outputs[1].shape);
//            offset3D = b_outputs[2].data.Download(b_outputs[2].shape);
//            heatMap3D = b_outputs[3].data.Download(b_outputs[3].shape);

//      //      PredictPose();

//            fpsCounter++;
//        }
//    }

//   public float z_offex=1;
//    public Transform Root;
//    VNectModel.JointPoint JointPoint_set(VNectModel.JointPoint JointPoint, float4 f)
//    {
     

//        JointPoint.Now3D.x = f.x;
//        JointPoint.Now3D.y = f.y;
//        JointPoint.Now3D.z = f.z* z_offex;
//        JointPoint.Score3D = f.w;

//        JointPoint.Now3D = Root.TransformDirection(JointPoint.Now3D);

//        return JointPoint;
//    }
//    public void PredictPose(float4[] f,float4[] lh,float4[] rh)
//    {
//        var score = 0f;
//        JointPoint_set(jointPoints[PositionIndex.rShldrBend.Int()], f[12]);
//        JointPoint_set(jointPoints[PositionIndex.rForearmBend.Int()], f[14]);

       
//        JointPoint_set(jointPoints[PositionIndex.rHand.Int()], f[16]);
     

//        JointPoint_set(jointPoints[PositionIndex.rThumb2.Int()], f[16] + rh[5] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.rMid1.Int()], f[16] + rh[17] - rh[0]);

//        JointPoint_set(jointPoints[PositionIndex.lShldrBend.Int()], f[11]);
//        JointPoint_set(jointPoints[PositionIndex.lForearmBend.Int()], f[13]);
//        JointPoint_set(jointPoints[PositionIndex.lHand.Int()], f[15]);
//        JointPoint_set(jointPoints[PositionIndex.lThumb2.Int()], f[15] + lh[5] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.lMid1.Int()], f[15] + lh[17] - lh[0]);

//        JointPoint_set(jointPoints[PositionIndex.lEar.Int()], f[7]);
//        JointPoint_set(jointPoints[PositionIndex.lEye.Int()], f[2]);
//        JointPoint_set(jointPoints[PositionIndex.rEar.Int()], f[8]);
//        JointPoint_set(jointPoints[PositionIndex.rEye.Int()], f[5]);
//        JointPoint_set(jointPoints[PositionIndex.Nose.Int()], f[0]);

//        JointPoint_set(jointPoints[PositionIndex.rThighBend.Int()], f[24]);
//        JointPoint_set(jointPoints[PositionIndex.rShin.Int()], f[26]);
//        JointPoint_set(jointPoints[PositionIndex.rFoot.Int()], f[28]);
//        JointPoint_set(jointPoints[PositionIndex.rToe.Int()], f[32]);

//        JointPoint_set(jointPoints[PositionIndex.lThighBend.Int()], f[23]);
//        JointPoint_set(jointPoints[PositionIndex.lShin.Int()], f[25]);
//        JointPoint_set(jointPoints[PositionIndex.lFoot.Int()], f[27]);
//        JointPoint_set(jointPoints[PositionIndex.lToe.Int()], f[31]);


        

//        JointPoint_set(jointPoints[PositionIndex.LeftThumbProximal.Int()], f[15] + lh[1] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftThumbIntermediate.Int()], f[15] + lh[2] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftThumbDistal.Int()], f[15] + lh[3] - lh[0]);

//        JointPoint_set(jointPoints[PositionIndex.LeftIndexProximal.Int()], f[15] + lh[5] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftIndexIntermediate.Int()], f[15] + lh[6] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftIndexDistal.Int()], f[15] + lh[7] - lh[0]);

//        JointPoint_set(jointPoints[PositionIndex.LeftMiddleProximal.Int()], f[15] + lh[9] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftMiddleIntermediate.Int()], f[15] + lh[10] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftMiddleDistal.Int()], f[15] + lh[11] - lh[0]);

//        JointPoint_set(jointPoints[PositionIndex.LeftRingProximal.Int()], f[15] + lh[13] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftRingIntermediate.Int()], f[15] + lh[14] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftRingDistal.Int()], f[15] + lh[15] - lh[0]);

//        JointPoint_set(jointPoints[PositionIndex.LeftLittleProximal.Int()], f[15] + lh[17] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftLittleIntermediate.Int()], f[15] + lh[18] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftLittleDistal.Int()], f[15] + lh[19] - lh[0]);

        
//        JointPoint_set(jointPoints[PositionIndex.RightThumbProximal.Int()], f[16] + rh[1] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightThumbIntermediate.Int()], f[16] + rh[2] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightThumbDistal.Int()], f[16] + rh[3] - rh[0]);

//        JointPoint_set(jointPoints[PositionIndex.RightIndexProximal.Int()], f[16] + rh[5] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightIndexIntermediate.Int()], f[16] + rh[6] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightIndexDistal.Int()], f[16] + rh[7] - rh[0]);

//        JointPoint_set(jointPoints[PositionIndex.RightMiddleProximal.Int()], f[16] + rh[9] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightMiddleIntermediate.Int()], f[16] + rh[10] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightMiddleDistal.Int()], f[16] + rh[11] - rh[0]);

//        JointPoint_set(jointPoints[PositionIndex.RightRingProximal.Int()], f[16] + rh[13] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightRingIntermediate.Int()], f[16] + rh[14] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightRingDistal.Int()], f[16] + rh[15] - rh[0]);

//        JointPoint_set(jointPoints[PositionIndex.RightLittleProximal.Int()], f[16] + rh[17] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightLittleIntermediate.Int()], f[16] + rh[18] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightLittleDistal.Int()], f[16] + rh[19] - rh[0]);
        
     



//        JointPoint_set(jointPoints[PositionIndex.RightThumbDistalTip.Int()], rh[4] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightIndexDistalTip.Int()], rh[8] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightMiddleDistalTip.Int()], rh[12] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightRingDistalTip.Int()], rh[16] - rh[0]);
//        JointPoint_set(jointPoints[PositionIndex.RightLittleDistalTip.Int()], rh[20] - rh[0]);

//        JointPoint_set(jointPoints[PositionIndex.LeftThumbDistalTip.Int()], lh[4] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftIndexDistalTip.Int()], lh[8] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftMiddleDistalTip.Int()], lh[12] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftRingDistalTip.Int()], lh[16] - lh[0]);
//        JointPoint_set(jointPoints[PositionIndex.LeftLittleDistalTip.Int()], lh[20] - lh[0]);

//        jointPoints[PositionIndex.abdomenUpper.Int()].Now3D = (new Vector3(f[11].x + f[24].x, f[11].y + f[24].y, f[11].z + f[24].z) / 2);
//        jointPoints[PositionIndex.abdomenUpper.Int()].Now3D = Root.TransformDirection(jointPoints[PositionIndex.abdomenUpper.Int()].Now3D);
//        //writer.WriteLine(csv);

//        // nose.transform.position = new Vector3(f[0].x, f[0].y, f[0].z);

//        EstimatedScore = score / JointNum;

//        // Calculate hip location
//        var lc = (jointPoints[PositionIndex.rThighBend.Int()].Now3D + jointPoints[PositionIndex.lThighBend.Int()].Now3D) / 2f;
//        jointPoints[PositionIndex.hip.Int()].Now3D = (jointPoints[PositionIndex.abdomenUpper.Int()].Now3D + lc) / 2f;
//        // Calculate neck location
//        jointPoints[PositionIndex.neck.Int()].Now3D = (jointPoints[PositionIndex.rShldrBend.Int()].Now3D + jointPoints[PositionIndex.lShldrBend.Int()].Now3D) / 2f;
//        // Calculate head location
//        var cEar = (jointPoints[PositionIndex.rEar.Int()].Now3D + jointPoints[PositionIndex.lEar.Int()].Now3D) / 2f;
//        var hv = cEar - jointPoints[PositionIndex.neck.Int()].Now3D;
//        var nhv = Vector3.Normalize(hv);
//        var nv = jointPoints[PositionIndex.Nose.Int()].Now3D - jointPoints[PositionIndex.neck.Int()].Now3D;
//         jointPoints[PositionIndex.head.Int()].Now3D = jointPoints[PositionIndex.neck.Int()].Now3D + nhv * Vector3.Dot(nhv, nv);
//        //jointPoints[PositionIndex.head.Int()].Now3D = jointPoints[PositionIndex.neck.Int()].Now3D+ nv;

//        // Calculate spine location
//        jointPoints[PositionIndex.spine.Int()].Now3D = jointPoints[PositionIndex.abdomenUpper.Int()].Now3D;

//        // Filters
//        //

//        var frwd = TriangleNormal(jointPoints[PositionIndex.hip.Int()].Now3D, jointPoints[PositionIndex.lThighBend.Int()].Now3D, jointPoints[PositionIndex.rThighBend.Int()].Now3D);
//        var frwdAngle = Vector3.Angle(frwd, Vector3.back);

//        foreach (var jp in jointPoints)
//        {
//            KalmanUpdate(jp);

//            jp.PrevPos3D[0] = jp.Pos3D;
//            for (var i = 1; i < jp.PrevPos3D.Length; i++)
//            {
//                //jp.PrevPos3D[i] = jp.PrevPos3D[i] * Smooth + jp.PrevPos3D[i - 1] * (1f - Smooth);
//                jp.PrevPos3D[i] = jp.PrevPos3D[i] * Smooth + jp.PrevPos3D[i - 1] * (1f - Smooth);
//            }
//            jp.Pos3D = jp.PrevPos3D[jp.PrevPos3D.Length - 1];


//            jp.Visibled = true;
//        }

//        if (frwdAngle < 45f)
//        {
//            if (EstimatedScore > ForwardThreshold)
//            {
//                VNectModel.IsPoseUpdate = true;
//            }
//        }
//        else
//        {
//            if (EstimatedScore > BackwardThreshold)
//            {
//                VNectModel.IsPoseUpdate = true;
//            }
//        }


//       // Debug.DrawLine(new Vector3(f[15].x, f[15].y, f[15].z), new Vector3(f[21].x, f[21].y, f[21].z));
//    }
//    public void HandTrackingGet(float4[] Lf, float4[]Rf)
//    {
//        if (HandTracking != null)
//        {

//            HandTracking.SycPostion(Lf,Rf);

//        }
//    }

//        public void PredictPose()
//    {
//        var score = 0f;
//        //var csv = videoCapture.VideoPlayer.frame.ToString();
//        filterWindow.SetFps(FPS);

//        for (var j = 0; j < JointNum; j++)
//        {
//            var jp = jointPoints[j];
//            var maxXIndex = 0;
//            var maxYIndex = 0;
//            var maxZIndex = 0;
//            jp.Score3D = 0.0f;
//            var jj = j * HeatMapCol;

//            for (var z = 0; z < HeatMapCol; z++)
//            {
//                var zz = jj + z;
//                for (var y = 0; y < HeatMapCol; y++)
//                {
//                    var yy = y * HeatMapCol_Squared * JointNum + zz;
//                    for (var x = 0; x < HeatMapCol; x++)
//                    {
//                        float v = heatMap3D[yy + x * HeatMapCol_JointNum];
//                        if (v > jp.Score3D)
//                        {
//                            jp.Score3D = v;
//                            maxXIndex = x;
//                            maxYIndex = y;
//                            maxZIndex = z;
//                        }
//                    }
//                }
//            }

//            //jp.PrevNow3D = jp.Now3D;
//            score += jp.Score3D;
//            var yi = maxYIndex * cubeOffsetSquared + maxXIndex * cubeOffsetLinear;
//            jp.Now3D.x = ((offset3D[yi + jj + maxZIndex] + 0.5f + (float)maxXIndex) / (float)HeatMapCol) * InputImageSizeF - InputImageSizeHalf;
//            jp.Now3D.y = InputImageSizeF - ((offset3D[yi + (j + JointNum) * HeatMapCol + maxZIndex] + 0.5f + (float)maxYIndex) / (float)HeatMapCol) * InputImageSizeF - InputImageSizeHalf;
//            jp.Now3D.z = ((offset3D[yi + (j + JointNum_Squared) * HeatMapCol + maxZIndex] + 0.5f + (float)(maxZIndex - HeatMapCol_Half)) / (float)HeatMapCol) * InputImageSizeF;
//            ////(jp.Now3D.x, jp.Now3D.y, jp.Now3D.z) = filter[j].Add(fx, fy, fz, FPS);
//            (jp.Now3D.x, jp.Now3D.y, jp.Now3D.z) = filter[j].Add(jp.Now3D.x, jp.Now3D.y, jp.Now3D.z, FPS);
            
//        }
//        //writer.WriteLine(csv);

//        EstimatedScore = score / JointNum;

//        // Calculate hip location
//        var lc = (jointPoints[PositionIndex.rThighBend.Int()].Now3D + jointPoints[PositionIndex.lThighBend.Int()].Now3D) / 2f;
//        jointPoints[PositionIndex.hip.Int()].Now3D = (jointPoints[PositionIndex.abdomenUpper.Int()].Now3D + lc) / 2f;
//        // Calculate neck location
//        jointPoints[PositionIndex.neck.Int()].Now3D = (jointPoints[PositionIndex.rShldrBend.Int()].Now3D + jointPoints[PositionIndex.lShldrBend.Int()].Now3D) / 2f;
//        // Calculate head location
//        var cEar = (jointPoints[PositionIndex.rEar.Int()].Now3D + jointPoints[PositionIndex.lEar.Int()].Now3D) / 2f;
//        var hv = cEar - jointPoints[PositionIndex.neck.Int()].Now3D;
//        var nhv = Vector3.Normalize(hv);
//        var nv = jointPoints[PositionIndex.Nose.Int()].Now3D - jointPoints[PositionIndex.neck.Int()].Now3D;
//        jointPoints[PositionIndex.head.Int()].Now3D = jointPoints[PositionIndex.neck.Int()].Now3D + nhv * Vector3.Dot(nhv, nv);
//        // Calculate spine location
//        jointPoints[PositionIndex.spine.Int()].Now3D = jointPoints[PositionIndex.abdomenUpper.Int()].Now3D;

//        // Filters
//        //

//        var frwd = TriangleNormal(jointPoints[PositionIndex.hip.Int()].Now3D, jointPoints[PositionIndex.lThighBend.Int()].Now3D, jointPoints[PositionIndex.rThighBend.Int()].Now3D);
//        var frwdAngle = Vector3.Angle(frwd, Vector3.back);
 
//        foreach (var jp in jointPoints)
//        {
//            KalmanUpdate(jp);

//            jp.PrevPos3D[0] = jp.Pos3D;
//            for (var i = 1; i < NOrderLPF; i++)
//            {
//                //jp.PrevPos3D[i] = jp.PrevPos3D[i] * Smooth + jp.PrevPos3D[i - 1] * (1f - Smooth);
//                jp.PrevPos3D[i] = jp.PrevPos3D[i] * Smooth + jp.PrevPos3D[i - 1] * (1f - Smooth);
//            }
//            jp.Pos3D = jp.PrevPos3D[NOrderLPF - 1];

 
//            jp.Visibled = true;
//        }

//        if (frwdAngle < 45f)
//        {
//            if (EstimatedScore > ForwardThreshold)
//            {
//                VNectModel.IsPoseUpdate = true;
//            }
//        }
//        else
//        {
//            if (EstimatedScore > BackwardThreshold)
//            {
//                VNectModel.IsPoseUpdate = true;
//            }
//        }
//    }
    
//    Vector3 TriangleNormal(Vector3 a, Vector3 b, Vector3 c)
//    {
//        Vector3 d1 = a - b;
//        Vector3 d2 = a - c;

//        Vector3 dd = Vector3.Cross(d1, d2);
//        dd.Normalize();

//        return dd;
//    }
//    /*
//    bool FrontBackCheckv(VNectModel.JointPoint jp1, VNectModel.JointPoint jp2, bool flag)
//    {
//        var l1 = Vector3.Distance(jp1.PrevNow3D, jp1.Now3D);
//        var c1 = Vector3.Distance(jp2.PrevNow3D, jp1.Now3D);

//        var l2 = Vector3.Distance(jp2.PrevNow3D, jp2.Now3D);
//        var c2 = Vector3.Distance(jp1.PrevNow3D, jp2.Now3D);

//        if(l1 > c1 && l2 > c2)
//        {
//            jp1.Error++;
//            jp2.Error++;
//            if (!flag && jp1.Error == jp1.RattlingCheckFrame)
//            {
//                jp1.Error = 0;
//                jp2.Error = 0;
//                return false;
//            }

//            return true;
//        }
//        else
//        {
//            jp1.Error = 0;
//            jp2.Error = 0;

//            return false;
//        }
//    }
//    */

//    void KalmanUpdate(VNectModel.JointPoint measurement)
//    {
//        measurementUpdate(measurement);
//        measurement.Pos3D.x = measurement.X.x + (measurement.Now3D.x - measurement.X.x) * measurement.K.x;
//        measurement.Pos3D.y = measurement.X.y + (measurement.Now3D.y - measurement.X.y) * measurement.K.y;
//        measurement.Pos3D.z = measurement.X.z + (measurement.Now3D.z - measurement.X.z) * measurement.K.z;
//        measurement.X = measurement.Pos3D;
//    }

//    void measurementUpdate(VNectModel.JointPoint measurement)
//    {
//        measurement.K.x = (measurement.P.x + KalmanParamQ) / (measurement.P.x + KalmanParamQ + KalmanParamR);
//        measurement.K.y = (measurement.P.y + KalmanParamQ) / (measurement.P.y + KalmanParamQ + KalmanParamR);
//        measurement.K.z = (measurement.P.z + KalmanParamQ) / (measurement.P.z + KalmanParamQ + KalmanParamR);
//        measurement.P.x = KalmanParamR * (measurement.P.x + KalmanParamQ) / (KalmanParamR + measurement.P.x + KalmanParamQ);
//        measurement.P.y = KalmanParamR * (measurement.P.y + KalmanParamQ) / (KalmanParamR + measurement.P.y + KalmanParamQ);
//        measurement.P.z = KalmanParamR * (measurement.P.z + KalmanParamQ) / (KalmanParamR + measurement.P.z + KalmanParamQ);
//    }

//    private void OnDestroy()
//    {
//        _worker?.Dispose();

//        if (User3Input)
//        {
//            // Assuming model with multiple inputs that were passed as a Dictionary
//            foreach (var key in inputs.Keys)
//            {
//                inputs[key].Dispose();
//            }

//            inputs.Clear();
//        }
//        else
//        {
//            input.Dispose();
//        }
//    }
//}
