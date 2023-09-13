using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MiniSleepBarPopup : Popup
{ 
    [BoxGroup("System Objects")] public EnemySleep sleep;
    [BoxGroup("UI Objects")] public Slider slider;
    protected override void InitPopup()
    {
        HidePopup();
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