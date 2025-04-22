using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TiggerOpenLink : MonoBehaviour
{
    public string link;
    public UnityEvent TriggerEnter, TriggerExit;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "AvatarCube")
        {
            Debug.Log("AvatarCubeExit");

            TriggerExit.Invoke();


        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AvatarCube")
        {
            Debug.Log("AvatarCubeEnter");

            if (link != "")
            {
                Application.OpenURL(link);
            }
            TriggerEnter.Invoke();


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
