using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_Canvas : MonoBehaviour
{
    public Camera camera;
    public Transform hand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
       hand.transform.position= camera.ScreenToWorldPoint(Input.mousePosition)+Vector3.forward;
    }
}
