
using UnityEngine;
using EasyBuildSystem.Features.Runtime.Buildings.Part;
using UnityEngine.Events;
using SimpleFileBrowser;
using System.Collections;
using UnityEngine.UI;
using BestHTTP;
using System;
using System.IO;
using Defective.JSON;
using Vuplex.WebView;
using DungeonArchitect;
using DungeonArchitect.Builders.Grid;
using DA_Assets.Shared.Extensions;

public class CustomRoomEditor : MonoBehaviour
{
    Dungeon Dungeon;
    public BuildingPart BuildingPart;
    DungeonArchitect.Builders.Grid.PlatformVolume platformVolume;
    void Awake()
    {
        if (GameObject.FindAnyObjectByType<NetworkSyncScript>() != null)
        {
            platformVolume = this.GetComponentInChildren<PlatformVolume>();
            Dungeon = GameObject.Find("DungeonGrid").GetComponent<Dungeon>();
            platformVolume.dungeon = Dungeon;
     
          //  this.gameObject.TryDestroyComponent<BuildingPart>();
            return;
        }

        platformVolume = this.GetComponentInChildren<PlatformVolume>();
        Dungeon = GameObject.Find("DungeonGrid").GetComponent<Dungeon>();
        platformVolume.dungeon = Dungeon;
        if (BuildingPart != null)
        {
            BuildingPart.enabled = true;
            BuildingPart.OnChangedStateEvent.AddListener(OnChangedState);  
        }

        
    }
    IEnumerator ExecuteBuildEverySecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            platformVolume.dungeon.Build();
        }
    }

    void OnChangedState(BuildingPart.StateType state)
    {
        Debug.Log(state+"  "+this.gameObject.name );

        switch (state)
        {
            case BuildingPart.StateType.PREVIEW:
                StartCoroutine("ExecuteBuildEverySecond");
                break;

            case BuildingPart.StateType.DESTROY:
                //platformVolume.dungeon = Dungeon;
                //platformVolume.dungeon.Build();
                break;

            case BuildingPart.StateType.EDIT:
                break;

            case BuildingPart.StateType.PLACED:
                StopCoroutine("ExecuteBuildEverySecond");
                //platformVolume.dungeon = Dungeon;
                platformVolume.dungeon.Build();
                break;

            case BuildingPart.StateType.QUEUE:
                break;
        }
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
