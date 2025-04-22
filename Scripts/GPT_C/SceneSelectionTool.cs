using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSelectionTool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!ES3FormUser.KeyExists("Username"))
        {
            ES3FormUser.Save<string>("CustomToolScenceName", "CustomA");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
