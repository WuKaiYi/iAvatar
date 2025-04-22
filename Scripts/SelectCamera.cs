using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCamera : MonoBehaviour
{
    public Dropdown Dropdown;
    List<Dropdown.OptionData> optionDatas;
    // Start is called before the first frame update
    void Start()
    {
        optionDatas = new List<Dropdown.OptionData>();

        foreach (WebCamDevice w in WebCamTexture.devices)
        {
            optionDatas.Add(new Dropdown.OptionData( w.name));
        }

        Dropdown.options = optionDatas;
        Dropdown.value= PlayerPrefs.GetInt("Camera",0);
        // PlayerPrefs.SetString("Camera", WebCamTexture.devices[0].name);

    }

    public void SelectCamera_change(int i)
    {
        PlayerPrefs.SetInt("Camera", i);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
