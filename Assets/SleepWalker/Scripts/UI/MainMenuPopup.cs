using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPopup : Popup
{
    public LevelSelectPopup levelSelectPopup;
    public SettingsPopup settingsPopup;
    public CreditsPopup creditsPopup;
    
    protected override void InitPopup()
    {
        levelSelectPopup.HidePopup();
        settingsPopup.HidePopup();
        creditsPopup.HidePopup();
    }

    public void PlayButtonClicked()
    {
        Debug.Log("Load First Level");
    }

    public void LevelSelectButtonClicked()
    {
        HidePopup();
        levelSelectPopup.ShowPopup();
    }

    public void SettingsButtonClicked()
    {
        HidePopup();
        settingsPopup.ShowPopup();
    }
    
    public void CreditsButtonClicked()
    {
        HidePopup();
        creditsPopup.ShowPopup();
    }

    public override void CloseButtonClicked()
    {
        base.CloseButtonClicked();
        levelSelectPopup.HidePopup();
        settingsPopup.HidePopup();
        creditsPopup.HidePopup();
        ShowPopup();
    }
}
