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
    [BoxGroup("UI Objects")] public BoolReference playerSleep;
    [BoxGroup("UI Objects")] public Slider staminaSlider;
    [BoxGroup("UI Objects")] public Image fillImage;
    [BoxGroup("UI Objects")] public Image backgroundImage;

    [BoxGroup("Setup Variables")] public Color sleepFill;
    [BoxGroup("Setup Variables")] public Color sleepBackground;
    [BoxGroup("Setup Variables")] public Color awakeFill;
    [BoxGroup("Setup Variables")] public Color awakeBackground;

    protected override void InitPopup()
    {
        staminaSlider.maxValue = playerMaxStamina;
        SetColours(playerSleep.Value);
    }
    
    public void OnStaminaChanged(FloatPair _floatPair)
    {
        staminaSlider.value = _floatPair.Item1;
    }

    public void OnMaxStaminaChanged(FloatPair _floatPair)
    {
        staminaSlider.maxValue = _floatPair.Item1;
    }

    public void OnPlayerSleep(bool _sleep)
    {
        SetColours(_sleep);
    }

    private void SetColours(bool _sleep)
    {
        fillImage.color = _sleep ? sleepFill : awakeFill;
        backgroundImage.color = _sleep ? sleepBackground : awakeBackground;
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