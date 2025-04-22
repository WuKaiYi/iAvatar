using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hostRecordData : MonoBehaviour
{
    [SerializeField]
    public List<Student> studentList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private float timer = 0.0f;
    private float waitTime = 2.0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            string json = JsonUtility.ToJson(studentList);
            Debug.Log(json);
            timer = timer - waitTime;
        }
    }

}
