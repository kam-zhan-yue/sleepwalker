using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MiniHealthBarPopup : Popup
{
    [BoxGroup("System Objects")] public Health health;
    [BoxGroup("UI Objects")] public Slider slider;
    protected override void InitPopup()
    {
    }

    public void UpdateHealth()
    {
        slider.value = health.GetHeathPercentage();
    }

    public void OnDialogueEventStarted()
    {
        HidePopup();
    }
    
    public void OnDialogueEventEnded()
    {
        //Don't show if dead in case of dialogue
        if(!health.IsDead())
            ShowPopup();
    }
}
