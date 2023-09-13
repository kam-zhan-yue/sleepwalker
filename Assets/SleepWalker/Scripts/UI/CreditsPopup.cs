using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPopup : Popup
{
    public MainMenuPopup mainMenuPopup;
    
    protected override void InitPopup()
    {
        mainHolder.gameObject.SetActiveFast(false);
    }

    public override void CloseButtonClicked()
    {
        base.CloseButtonClicked();
        mainMenuPopup.CloseButtonClicked();
    }
}
