using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.Netcode;
using Unity.Collections;

public class CustomLoaderNetworkBehaviour : NetworkBehaviour
{
    [SerializeField]
    NetworkVariable<bool> m_DynamicEditing = new NetworkVariable<bool>();

    [SerializeField]
    NetworkVariable<CustomLoader.DataType> m_DataType = new NetworkVariable<CustomLoader.DataType>();

    [SerializeField]
    NetworkVariable<FixedString128Bytes> m_Data = new NetworkVariable<FixedString128Bytes>();

    [SerializeField]
    NetworkVariable<FixedString128Bytes> m_InfoData = new NetworkVariable<FixedString128Bytes>();

    public NetworkVariable<bool> DynamicEditing => m_DynamicEditing;
    public NetworkVariable<CustomLoader.DataType> DataType => m_DataType;
    public NetworkVariable<FixedString128Bytes> Data => m_Data;
    public NetworkVariable<FixedString128Bytes> InfoData => m_InfoData;

    public string s_Data;

    //���F�@���W�j���w���d��r��߉݋

    public override void OnNetworkSpawn()
    {
        // FixedString128Bytes �D�Q��string
        s_Data = m_Data.Value.ToString();
        InitializeData();

        if (IsServer)
        {
           
            // Assin the current value based on the current message index value
            //m_Data.Value = m_Messages[m_MessageIndex];
        }
        else
        {
            // Subscribe to the OnValueChanged event
            m_Data.OnValueChanged += OnTextStringChanged;
            // Log the current value of the text string when the client connected
            Debug.Log($"Client-{NetworkManager.LocalClientId}'s TextString = {m_Data.Value}");
        }
    }

    public CustomLoader Model3D;

    /// <summary>
    /// ������ʼ��
    /// </summary>
    public void InitializeData()
    {
        if (m_DataType.Value == CustomLoader.DataType.Model3D)
        {
         
            Model3D.data = s_Data;
            Model3D.gameObject.SetActive(true);
        
        }
        // TODO: ���ݳ�ʼ���߼�
    }

    public override void OnNetworkDespawn()
    {
        m_Data.OnValueChanged -= OnTextStringChanged;
    }

    private void OnTextStringChanged(FixedString128Bytes previous, FixedString128Bytes current)
    {
        //s_Data = current.Value.ToString();
        //InitializeData();
        // Just log a notification when m_Data changes
        Debug.Log($"Client-{NetworkManager.LocalClientId}'s TextString = {m_Data.Value}");
    }

}