using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class OpenWebLink : MonoBehaviour
{
    public void Open(string  link)
    {
        Application.OpenURL(link );
    }
    public TextMeshProUGUI text;

    public void PasteFromClipboard()
    {
        TextEditor textEditor = new TextEditor();
        //textEditor.multiline = true;
        textEditor.Paste();  //Copy string from Clipboard to textEditor.text
        text.text = textEditor.text;

      //  PlayerPrefs.SetString("avatarlink", textEditor.text);
        ES3.Save("AvatarUrl", textEditor.text);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
