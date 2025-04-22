using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ES3AutoLoad : MonoBehaviour
{
    public UnityEvent<string> StringLoad;

    public string Key;
    // Start is called before the first frame update
    void Start()
    {
        
        if (StringLoad.GetPersistentEventCount() > 0)
        {
            StringLoad.Invoke((string )ES3.Load(Key));
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
