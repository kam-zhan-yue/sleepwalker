using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StaminaBarPopup : Popup
{
    [FoldoutGroup("UI Objects")] public FloatReference playerMaxStamina;
    [FoldoutGroup("UI Objects")] public FloatReference playerStamina;
    [FoldoutGroup("UI Objects")] public Slider staminaSlider;

    public override void InitPopup()
    {
        staminaSlider.maxValue = playerMaxStamina;
    }

    public override void ShowPopup()
    {
        gameObject.SetActiveFast(true);
    }

    public override void HidePopup()
    {
        gameObject.SetActiveFast(false); 
    }
    
    public void OnStaminaChanged(FloatPair _floatPair)
    {
        staminaSlider.value = _floatPair.Item1;
    }

    public void OnMaxStaminaChanged(FloatPair _floatPair)
    {
        staminaSlider.maxValue = _floatPair.Item1;
    }
}