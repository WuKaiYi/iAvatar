using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMove : MonoBehaviour
{
    public Unity.Cinemachine.CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer framingTransposer;

    // Start is called before the first frame update
    void Start()
    {
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(-Vector3.forward * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            framingTransposer.m_FollowOffset += Vector3.forward*Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.X))
        {
            framingTransposer.m_FollowOffset -= Vector3.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            this.transform.Rotate(Vector3.up);
        }
        if (Input.GetKey(KeyCode.E))
        {
            this.transform.Rotate(Vector3.down );
        }
    }
}
