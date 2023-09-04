using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPopup : Popup
{
    [FoldoutGroup("Setup")] public float primaryTime;
    [FoldoutGroup("Setup")] public float secondaryTime;
    [FoldoutGroup("Setup")] public float delay;
    [FoldoutGroup("Setup")] public Ease easing;
    [FoldoutGroup("UI Objects")] public FloatReference maxHealth;
    [FoldoutGroup("UI Objects")] public Slider primarySlider;
    [FoldoutGroup("UI Objects")] public Slider secondarySlider;
    
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
}