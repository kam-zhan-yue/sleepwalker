using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPopup : Popup
{
    public MiniHealthBarPopup healthBarPopup;
    public MiniSleepBarPopup sleepBarPopup;
    
    protected override void InitPopup()
    {
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        healthBarPopup.ShowPopup();
        sleepBarPopup.ShowPopup();
    }

    public override void HidePopup()
    {
        base.HidePopup();
        healthBarPopup.HidePopup();
        sleepBarPopup.HidePopup();
    }
}
