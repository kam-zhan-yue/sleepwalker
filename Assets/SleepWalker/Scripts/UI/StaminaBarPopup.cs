using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StaminaBarPopup : Popup
{
    [BoxGroup("UI Objects")] public FloatReference playerMaxStamina;
    [BoxGroup("UI Objects")] public FloatReference playerStamina;
    [BoxGroup("UI Objects")] public Slider staminaSlider;

    protected override void InitPopup()
    {
        staminaSlider.maxValue = playerMaxStamina;
    }
    
    public void OnStaminaChanged(FloatPair _floatPair)
    {
        staminaSlider.value = _floatPair.Item1;
    }

    public void OnMaxStaminaChanged(FloatPair _floatPair)
    {
        staminaSlider.maxValue = _floatPair.Item1;
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