using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MiniHealthBarPopup : Popup
{
    [FoldoutGroup("System Objects")] public Health health;
    [FoldoutGroup("UI Objects")] public Slider slider;
    public override void InitPopup()
    {
    }

    public override void ShowPopup()
    {
        gameObject.SetActiveFast(true);
    }

    public override void HidePopup()
    {
        gameObject.SetActiveFast(false);
    }

    public void UpdateHealth()
    {
        slider.value = health.GetHeathPercentage();
    }
}
