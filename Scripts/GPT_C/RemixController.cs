
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RemixController : MonoBehaviour
{
    public Button Btn;
    public TMP_Text tt;

    public bool isClick;
    public float tempTime = 0;
    public bool isstart = false;

    public RemixGetData remixgetdata;

    private int progress_label;

    // 这里用来记录开关的状态
    private int state_machine = 0;
    //初始状态为0也就是可以使用状态打开AI
    //打开后变为1
    //变为1后就只能进行关闭，然后又变为0

    // Start is called before the first frame update

    void Start()
    {

        //Btn.onClick.AddListener(OnClick);

    }
    // Update is called once per frame
    void Update()
    {

        //if (state_machine == 0)
        //{
        //    tt.text = "Open AI";
        //}

        //if (state_machine == 1)
        //{
        //    tt.text = "Using";
        //}

        //if (state_machine == 2)
        //{
        //    tt.text = "UsingAI";
        //}


        //if (isClick)
        //{ //如果被点击
        //    waitTime(15);
        //    state_machine = 2;
        //}


    }
    private void OnClick()

    {
        isClick = true;
        Btn.enabled = false;
    }

    private void waitTime(int time)
    {
        tempTime += Time.deltaTime;

        if (tempTime > time)

        {

            tempTime = 0;

            Btn.enabled = true;

            isClick = false;
            print("WaitingTime!");
        }
    }
    int the_progress_label;

    public void Click_Remix_Start()
    {

        //判断是否执行过开启，如果执行过就打开RemixClose.bat
        if (state_machine == 0)
        {
            state_machine = 1;
            Application.OpenURL(Application.streamingAssetsPath + "/RemixStart.bat");
            Debug.Log(Application.streamingAssetsPath + "/RemixStart.bat");
        }
        else
        {
            state_machine = 0;
            Application.OpenURL(Application.streamingAssetsPath + "/RemixClose.bat");
            Debug.Log(Application.streamingAssetsPath + "/RemixClose.bat");
        }


    }
    private void OnApplicationQuit()
    {
        if (state_machine == 1)
        {
            Application.OpenURL(Application.streamingAssetsPath + "/RemixClose.bat");
        }
    }

    private static void RunBat(string batFile)
    {

    }


    static string FormatPath(string path)
    {
        //path = path.Replace("/", "\\");
        //if (Application.platform == RuntimePlatform.OSXEditor)
        //{
        //    path = path.Replace("\\", "/");
        //}
        string the_path = "";
        the_path = "file:///" + Application.streamingAssetsPath + "/" + path;
        return the_path;
    }
}
