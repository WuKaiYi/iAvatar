using System.Collections;
using System.Collections.Generic;
using Unity.BossRoom.ApplicationLifecycle;
using Unity.Services.Vivox;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("GetLobbyID:  "+GameObject.FindFirstObjectByType<ApplicationControllerAvatar>().GetLobbyID());
        LoginToVivox(ES3FormUser.Load<string >("custom_name")); 
    }
    public async void LoginToVivox(string name)
    {
        var loginOptions = new LoginOptions()
        {
            DisplayName = name,
            ParticipantUpdateFrequency = ParticipantPropertyUpdateFrequency.FivePerSecond
        };
        await VivoxService.Instance.InitializeAsync();
        VivoxVoiceManager.Instance.LobbyChannelName = GameObject.FindFirstObjectByType<ApplicationControllerAvatar>().GetLobbyID();
        await VivoxService.Instance.LoginAsync(loginOptions);
    }
}