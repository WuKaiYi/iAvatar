using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizTigger : MonoBehaviour
{
	public Color32 Enter, Exit;
	// Start is called before the first frame update
	void Start()
    {
        
    }
	private void OnTriggerEnter(Collider other)
	{
		this.GetComponent<MeshRenderer>().material.color = Enter;
	}

	private void OnTriggerExit(Collider other)
	{
		this.GetComponent<MeshRenderer>().material.color = Exit;
	}
	// Update is called once per frame
	void Update()
    {
        
    }
}
