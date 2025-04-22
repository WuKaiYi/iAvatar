using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CanvasTripod : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void LateUpdate()
    {
        Camera camera = Camera.main;
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }
    // Update is called once per frame
    public void Update()
    {
      
    }
}
