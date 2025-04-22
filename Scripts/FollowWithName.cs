using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithName : MonoBehaviour
{
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    GameObject tagher;
    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find(name) != null&& tagher==null)
        {
            tagher = GameObject.Find(name);
        }
        if (tagher != null)
        {
            this.transform.position = tagher.transform.position;
            this.transform.rotation = tagher.transform.rotation;
        }

        
    }
}
