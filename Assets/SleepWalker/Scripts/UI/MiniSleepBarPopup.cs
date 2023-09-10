using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MiniSleepBarPopup : Popup
{ 
    [FoldoutGroup("System Objects")] public EnemySleep sleep;
    [FoldoutGroup("UI Objects")] public Slider slider;
    [FoldoutGroup("UI Objects")] public RectTransform holder;
    public override void InitPopup()
    {
        HidePopup();
    }

    public override void ShowPopup()
    {
        isShowing = true;
        holder.gameObject.SetActiveFast(true);
    }

    public override void HidePopup()
    {
        isShowing = false;
        holder.gameObject.SetActiveFast(false);
    }

    private void Update()
    {
        if(isShowing)
            slider.value = sleep.GetSleepPercentage();
    }

    public void OnDialogueEventStarted()
    {
        HidePopup();
    }
    
    public void OnDialogueEventEnded()
    {
        ShowPopup();
    }
}