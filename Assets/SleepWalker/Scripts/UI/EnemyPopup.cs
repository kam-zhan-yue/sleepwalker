using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPopup : Popup
{
    public MiniHealthBarPopup healthBarPopup;
    public MiniSleepBarPopup sleepBarPopup;
    
    public override void InitPopup()
    {
    }

    public override void ShowPopup()
    {
    }

    public override void HidePopup()
    {
        healthBarPopup.HidePopup();
        sleepBarPopup.HidePopup();
    }
}
