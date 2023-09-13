using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPopup : Popup
{
    protected override void InitPopup()
    {
        mainHolder.gameObject.SetActiveFast(false);
    }
}
