using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class RegisterUnityCam : MonoBehaviour
{
    public void Register()
    {
        // ExecuteCommand(Application.streamingAssetsPath + "/RunMe First/x32/Register.bat",5);
        RunMyBat(Application.streamingAssetsPath+ "/Install/BAT/InstallLearningverse.bat");

    }

    public void openFolder()
    {
        Application.OpenURL("file:///" + Application.streamingAssetsPath + "/Install/BAT");
    }

    public void UnRegister()
    {
        //  ExecuteCommand(Application.streamingAssetsPath + "/RunMe First/x32/Unregister.bat", 5);

        RunMyBat( Application.streamingAssetsPath + "/Install/Uninstall.bat");

    }
    private static void RunMyBat(string path)
    {
     
        string bat = @path;
        UnityEngine.Debug.Log(bat);

        var psi = new ProcessStartInfo();
        psi.CreateNoWindow = true; //This hides the dos-style black window that the command prompt usually shows
       // psi.FileName = @"cmd.exe";

        psi.FileName = path;

        psi.Verb = "runas"; //This is what actually runs the command as administrator
      //  psi.Arguments ="/c"+ @path;

        //�Ƿ�ʹ�ò���ϵͳshell����
        psi.UseShellExecute = true;
              // �������Ե��ó����������Ϣ
      //  psi.RedirectStandardInput = true;
        //�����Ϣ
      //  psi.RedirectStandardOutput = true;
        // �������
      //  psi.RedirectStandardError = true;
        //����ʾ���򴰿�
       


        try
        {
            var process = new Process();
            process.StartInfo = psi;
            process.Start();
            /*
            using (StreamWriter sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("LearningverseToolkitCamer");
                    //sw.WriteLine("cd " + Application.persistentDataPath + "/forge");
                    //sw.WriteLine("gradlew.bat - cleanCache - clean - setupDecompWorkspace--refresh - dependencies");
                }
            }
            */
          //  StreamWriter sIn = process.StandardInput;
         //   sIn.WriteLine("LearningverseToolkitCamera");

            // process.StandardInput.WriteLine("LearningverseToolkitCamera");
          //  process. StandardInput.WriteLine("Test");
          //  process.StandardInput.AutoFlush = true;
            process.WaitForExit();
            UnityEngine.Debug.Log(process.ExitCode);
        }
        catch (Exception)
        {
            UnityEngine.Debug.Log("error");
            //If you are here the user clicked decline to grant admin privileges (or he's not administrator)
        }

    }



    

    private static void Run()

    {

      
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
