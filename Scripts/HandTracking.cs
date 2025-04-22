using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
   // public UnityEngine.Animations.Rigging.ChainIKConstraint[] chainIKConstraintsL;
    public LandmarkInterface.HandLandmark HandLandmarkL;
    public LandmarkInterface.HandLandmark HandLandmarkR;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    Vector3 setPostion( float4 f)
    {
       return new Vector3(f.x, f.y, f.z);
    }
    public void SycPostion(float4[] Lf, float4[] Rf)
    {
        List<Vector3> outputL = new List<Vector3>();
        for (int i = 0; i < Lf.Length; i++)
        {
            var v = new Vector3(Lf[i].x, Lf[i].y, Lf[i].z);
            outputL.Add(v);
        }

        List<Vector3> outputR = new List<Vector3>();
        for (int i = 0; i < Rf.Length; i++)
        {
            var v = new Vector3(Rf[i].x, Rf[i].y, Rf[i].z);
            outputR.Add(v);
        }

        HandLandmarkL.updateLandmarkPosition(outputL);
        HandLandmarkR.updateLandmarkPosition(outputR);
        HandLandmarkL.updateLandmarkScale(outputL);
        HandLandmarkR.updateLandmarkScale(outputR);
        HandLandmarkL.updateWristRotation();
        HandLandmarkR.updateWristRotation();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
