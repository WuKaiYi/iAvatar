using System;
using UnityEngine;
//using Unity.Services.Authentication;
//using Unity.Services.Core;
//using Unity.Services.Vivox;
//using VivoxUnity;

public class AvatarVivox : MonoBehaviour
{
    //    public LoginScreenUI loginScreenUI;
    //    private void Awake()
    //    {

    //    }
    //    private void LoginToVivox()
    //    {
    //       // loginScreenUI.AvatarLogin(PlayerPrefs.GetString("UnityPlayerID"));
    //        loginScreenUI.AvatarLogin(AuthenticationService.Instance.PlayerId);

    //    }

    //    // Start is called before the first frame update
    //    void Start()
    //    {
    //        //Invoke("LoginToVivox", 2);

    //        LoginToVivox();

    //    }
    //    public ILoginSession LoginSession;
    //    private VivoxVoiceManager _vivoxVoiceManager;

    //    public void JoinChannel(string channelName, ChannelType channelType, bool connectAudio, bool connectText, bool transmissionSwitch = true, Channel3DProperties properties = null)
    //    {
    //        if (LoginSession.State == LoginState.LoggedIn)
    //        {
    //            Channel channel = new Channel(channelName, channelType, properties);

    //            IChannelSession channelSession = LoginSession.GetChannelSession(channel);

    //            channelSession.BeginConnect(connectAudio, connectText, transmissionSwitch, channelSession.GetConnectToken(), ar =>
    //            {
    //                try
    //                {
    //                    channelSession.EndConnect(ar);
    //                }
    //                catch (Exception e)
    //                {
    //                    Debug.LogError($"Could not connect to channel: {e.Message}");
    //                    return;
    //                }
    //            });
    //        }
    //        else
    //        {
    //            Debug.LogError("Can't join a channel when not logged in.");
    //        }
    //    }

    //    public void Login(string displayName = null)
    //    {
    //        var account = new Account(displayName);
    //        bool connectAudio = true;
    //        bool connectText = true;

    //        LoginSession = VivoxService.Instance.Client.GetLoginSession(account);
    //        LoginSession.PropertyChanged += LoginSession_PropertyChanged;

    //        LoginSession.BeginLogin(LoginSession.GetLoginToken(), SubscriptionMode.Accept, null, null, null, ar =>
    //        {
    //            try
    //            {
    //                LoginSession.EndLogin(ar);
    //            }
    //            catch (Exception e)
    //            {
    //                // 取消绑定您订阅的所有登录会话相关事件。
    //                // 处理错误
    //                return;
    //            }
    //            // 至此，我们已经成功请求登录。 
    //            // 当您能够加入频道时，LoginSession.State 将设置为 LoginState.LoggedIn。
    //            // 参考 LoginSession_PropertyChanged()
    //        });
    //    }

    //    // 在本例中，我们在 LoginState 更改为 LoginState.LoggedIn 后立即加入频道。
    //    // 在实际游戏中，何时加入频道会因实现而异。
    //    private void LoginSession_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //    {
    //        var loginSession = (ILoginSession)sender;
    //        if (e.PropertyName == "State")
    //        {
    //            if (loginSession.State == LoginState.LoggedIn)
    //            {
    //                bool connectAudio = true;
    //                bool connectText = true;

    //                // 这会将您带入回声频道，您可以在其中听到自己的讲话。
    //                // 如果能听到自己的声音，那么一切正常，您已准备好可将 Vivox 集成到项目中。
    //                JoinChannel("TestChannel", ChannelType.Echo, connectAudio, connectText);
    //                // 要测试多个用户，请尝试加入非位置频道。
    //                // JoinChannel("MultipleUserTestChannel", ChannelType.NonPositional, connectAudio, connectText);
    //            }
    //        }
    //    }
    //    // Update is called once per frame
    //    void Update()
    //    {

    //    }
}




