using DA_Assets.FCU.Model;
using DA_Assets.FCU;
using DA_Assets.Shared;
using UnityEngine;
using UnityEngine.Events;
using Lean.Gui;
using UnityEngine.UI;
using DA_Assets.Shared.Extensions;
using Lean.Transition;
using System;


public class FigmaImporter : MonoBehaviour
{
#if UNITY_EDITOR

    /// <summary>
    /// Called when a component is added to a GameObject based on tag. Called multiple times per GameObject.
    /// </summary>
    LeanWindow _leanWindow;
    CanvasGroup canvasGroup;

    public void OnAddComponent(FigmaConverterUnity figmaConverterUnity, SyncData syncData, FcuTag fcuTag)
    {

        Debug.Log(syncData.FormattedName);
        //Debug.Log($"OnAddComponent name:{syncData.GameObject.name} FormattedName: {syncData.FormattedName} TagReason:{syncData.TagReason} :{syncData.GameObject.name} fcuTag: {fcuTag.ToString()}");
        if (syncData.FormattedName.StartsWith("raw"))
        {
            if (syncData.GameObject.GetComponent<RawImage>() == null)
            {
                if (syncData.GameObject.GetComponent<Image>() != null)
                {        
                    syncData.GameObject.TryDestroyComponent<Image>();
                }

                RawImage rawImage  = syncData.GameObject.AddComponent<RawImage>();
            }

        }
      
        if (syncData.FormattedName.StartsWith("Slider"))
        {
            if (syncData.GameObject.GetComponent<Slider>() == null)
            {
                Slider slider = syncData.GameObject.AddComponent<Slider>();
                slider.targetGraphic = syncData.GameObject.transform.GetChild(0).GetComponent<Image>();
                slider.fillRect = syncData.GameObject.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
                slider.handleRect = syncData.GameObject.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>();
            }
        }

        if (syncData.FormattedName.StartsWith("win"))
        {
            if (syncData.GameObject.GetComponent<LeanWindow>() == null)
            {
                syncData.GameObject.AddComponent<LeanWindow>();

                syncData.GameObject.AddComponent<Canvas>();
                syncData.GameObject.AddComponent<CanvasGroup>();
                syncData.GameObject.AddComponent<GraphicRaycaster>();

                Debug.Log(_leanWindow);
                syncData.GameObject.GetComponent<LeanWindow>().OnTransitions.Speed = leanWindow.OnTransitions.Speed;

                LeanPlayer.Entry entry = new LeanPlayer.Entry();

                entry.Root = leanWindow.OnTransitions.Entries[0].Root;
                entry.Speed = leanWindow.OnTransitions.Entries[1].Speed;
                entry.AddAlias(leanWindow.OnTransitions.Entries[0].Aliases[0].Key, syncData.GameObject.GetComponent<CanvasGroup>());

                LeanPlayer.Entry entry2 = new LeanPlayer.Entry();

                entry2.Root = leanWindow.OnTransitions.Entries[1].Root;
                entry2.Speed = leanWindow.OnTransitions.Entries[1].Speed;
                entry2.AddAlias(leanWindow.OnTransitions.Entries[1].Aliases[0].Key, syncData.GameObject.GetComponent<RectTransform>());
                syncData.GameObject.GetComponent<LeanWindow>().OnTransitions.Entries.Add(entry);
                syncData.GameObject.GetComponent<LeanWindow>().OnTransitions.Entries.Add(entry2);

                LeanPlayer.Entry entry3 = new LeanPlayer.Entry();

                entry3.Root = leanWindow.OffTransitions.Entries[0].Root;
                entry3.Speed = leanWindow.OnTransitions.Entries[1].Speed;
                entry3.AddAlias(leanWindow.OffTransitions.Entries[0].Aliases[0].Key, syncData.GameObject.GetComponent<CanvasGroup>());

                LeanPlayer.Entry entry4 = new LeanPlayer.Entry();

                entry4.Root = leanWindow.OffTransitions.Entries[1].Root;
                entry4.Speed = leanWindow.OnTransitions.Entries[1].Speed;
                entry4.AddAlias(leanWindow.OffTransitions.Entries[1].Aliases[0].Key, syncData.GameObject.GetComponent<RectTransform>());

                syncData.GameObject.GetComponent<LeanWindow>().OffTransitions.Entries.Add(entry3);
                syncData.GameObject.GetComponent<LeanWindow>().OffTransitions.Entries.Add(entry4);
            }
        }


    }
    /// <summary>
    /// Called when a fobject's GameObject is created on the scene. Called once per GameObject.
    /// </summary>
    public void OnObjectInstantiate(FigmaConverterUnity figmaConverterUnity, GameObject gameObject)
    {

        //if()
        //Debug.Log($"OnObjectInstantiate {gameObject.name } : ");
    }

    public Transform FigmaUI;
    public LeanWindow leanWindow;
    public void SetRootCanvse()
    {
        if (FigmaUI.childCount == 0)
        {
            return;
        }
        for (int i = 0; i < FigmaUI.childCount; i++)
        {
            RectTransform rectTransform = FigmaUI.GetChild(i).GetComponent<RectTransform>();

            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector3.zero;
            rectTransform.localPosition = Vector3.zero;

            //if (rectTransform.GetComponent<LeanWindow>() == null)
            //{
            //    rectTransform.gameObject.AddComponent<GraphicRaycaster>();
            //    Canvas canvas = rectTransform.gameObject.AddComponent<Canvas>();
            //    CanvasGroup canvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
            //    LeanWindow _leanWindow = canvasGroup.gameObject.AddComponent<LeanWindow>();

            //}


        }
        FigmaUI.GetComponent<CanvasScaler>().enabled = true;


    }

#endif
}