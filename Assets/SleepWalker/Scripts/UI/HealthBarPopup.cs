using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPopup : Popup
{
    [BoxGroup("Setup Variables")] public float primaryTime;
    [BoxGroup("Setup Variables")] public float secondaryTime;
    [BoxGroup("Setup Variables")] public float delay;
    [BoxGroup("Setup Variables")] public Ease easing;
    [BoxGroup("UI Objects")] public FloatReference maxHealth;
    [BoxGroup("UI Objects")] public Slider primarySlider;
    [BoxGroup("UI Objects")] public Slider secondarySlider;
    
    protected override void InitPopup()
    {
    }
    
    public void OnHealthChanged(FloatPair _floatPair)
    {
        if (maxHealth.Value <= 0)
            return;

        DOTween.Kill("Health");
        Sequence sequence = DOTween.Sequence();
        sequence.SetId("Health");
        sequence.Append(DOTween.To(_x => primarySlider.value = _x, _floatPair.Item2 / maxHealth.Value, _floatPair.Item1 / maxHealth.Value, primaryTime).SetEase(easing));
        sequence.AppendInterval(delay);
        sequence.Append(DOTween.To(_x => secondarySlider.value = _x, _floatPair.Item2 / maxHealth.Value, _floatPair.Item1 / maxHealth.Value, secondaryTime).SetEase(easing));
        sequence.Play();
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