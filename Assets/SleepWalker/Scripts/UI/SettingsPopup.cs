using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopup : Popup
{
    public MainMenuPopup mainMenuPopup;
    public VolumePopup volumePopup;
    
    protected override void InitPopup()
    {
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        volumePopup.ShowPopup();
    }

    public override void HidePopup()
    {
        base.HidePopup();
        volumePopup.HidePopup();
    }

    public override void CloseButtonClicked()
    {
        base.CloseButtonClicked();
        mainMenuPopup.CloseButtonClicked();
    }
}
