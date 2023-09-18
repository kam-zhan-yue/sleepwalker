using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MEC;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialPopup : Popup
{
    public FloatReference maxStamina;
    public FloatReference stamina;
    public BoolReference canPause;
    [FormerlySerializedAs("inTutorial")] public BoolReference tutorial;
    public float descriptionShowTime = 5f;
    public PlayerAwake playerAwake;
    public RectTransform sleepTutorialHolder;
    public RectTransform sleepTutorialDescriptionHolder;
    public RectTransform dashTutorialHolder;
    public bool sleepTutorial;
    public bool dashTutorial;
    private bool sleepTutorialOver = false;
    private bool inSleepTutorial = false;
    private bool inDashTutorial = false;
    private CoroutineHandle sleepTutorialRoutine;

    protected override void InitPopup()
    {
        HidePopup();
    }

    private void Start()
    {
        if (sleepTutorial)
            stamina.Value = maxStamina.Value * 0.1f;
    }

    public override void HidePopup()
    {
        base.HidePopup();
        sleepTutorialHolder.gameObject.SetActiveFast(false);
        sleepTutorialDescriptionHolder.gameObject.SetActive(false);
        dashTutorialHolder.gameObject.SetActiveFast(false);
    }

    public void ActivateSleepTutorial()
    {
        if (sleepTutorialOver)
            return;
        ShowPopup();
        tutorial.Value = true;
        canPause.Value = false;
        sleepTutorialHolder.gameObject.SetActiveFast(true);
        Time.timeScale = 0f;
        playerAwake.sleepAbility = true;
        inSleepTutorial = true;
        sleepTutorialOver = true;
    }

    public void ActivateDashTutorial()
    {
        ShowPopup();
        tutorial.Value = true;
        dashTutorialHolder.gameObject.SetActiveFast(true);
        playerAwake.dashAbility = true;
        inDashTutorial = true;
    }

    public void OnSleep()
    {
        if (inSleepTutorial)
        {
            tutorial.Value = false;
            canPause.Value = true;
            Time.timeScale = 1f;
            inSleepTutorial = false;
            sleepTutorialHolder.gameObject.SetActiveFast(false);
            sleepTutorialRoutine = Timing.RunCoroutine(SleepTutorialCoroutine().CancelWith(gameObject));
        }
    }

    private IEnumerator<float> SleepTutorialCoroutine()
    {
        yield return Timing.WaitForSeconds(1f);
        sleepTutorialDescriptionHolder.gameObject.SetActiveFast(true);
        yield return Timing.WaitForSeconds(descriptionShowTime);
        HidePopup();
    }

    public void OnDash()
    {
        if (inDashTutorial)
        {
            HidePopup();
            inDashTutorial = false;
        }
    }
}
