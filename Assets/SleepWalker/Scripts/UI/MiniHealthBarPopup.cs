using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MiniHealthBarPopup : Popup
{
    [FoldoutGroup("System Objects")] public Health health;
    [FoldoutGroup("UI Objects")] public Slider slider;
    [FoldoutGroup("UI Objects")] public RectTransform holder;
    public override void InitPopup()
    {
    }

    public override void ShowPopup()
    {
        holder.gameObject.SetActiveFast(true);
    }

    public override void HidePopup()
    {
        holder.gameObject.SetActiveFast(false);
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
        ShowPopup();
    }
}
