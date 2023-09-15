using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class TutorialPopup : Popup
{
    public BoolVariable canPause;
    public PlayerAwake playerAwake;
    public RectTransform sleepTutorialHolder;
    public RectTransform dashTutorialHolder;
    private bool sleepTutoralOver = false;
    private bool inSleepTutorial = false;
    private bool inDashTutorial = false;

    protected override void InitPopup()
    {
        HidePopup();
    }

    public override void HidePopup()
    {
        base.HidePopup();
        sleepTutorialHolder.gameObject.SetActiveFast(false);
        dashTutorialHolder.gameObject.SetActiveFast(false);
    }

    public void ActivateSleepTutorial()
    {
        if (sleepTutoralOver)
            return;
        ShowPopup();
        canPause.Value = false;
        sleepTutorialHolder.gameObject.SetActiveFast(true);
        Time.timeScale = 0f;
        playerAwake.sleepAbility = true;
        inSleepTutorial = true;
        sleepTutoralOver = true;
    }

    public void ActivateDashTutorial()
    {
        ShowPopup();
        dashTutorialHolder.gameObject.SetActiveFast(true);
        playerAwake.dashAbility = true;
        inDashTutorial = true;
    }

    public void OnSleep()
    {
        if (inSleepTutorial)
        {
            HidePopup();
            canPause.Value = true;
            Time.timeScale = 1f;
            inSleepTutorial = false;
        }
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
