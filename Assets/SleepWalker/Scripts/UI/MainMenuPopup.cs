using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPopup : Popup
{
    public LevelSelectPopup levelSelectPopup;
    public SettingsPopup settingsPopup;
    public CreditsPopup creditsPopup;
    public BoolVariable showLevelSelection;
    
    protected override void InitPopup()
    {
        ShowPopup();
    }

    public void PlayButtonClicked()
    {
        Debug.Log("Load First Level");
        SceneManager.LoadScene(1);
    }

    public override void ShowPopup()
    {
        if (showLevelSelection.Value)
        {
            showLevelSelection.Value = false;
            HidePopup();
            levelSelectPopup.ShowPopup();
            settingsPopup.HidePopup();
            creditsPopup.HidePopup();
        }
        else
        {
            base.ShowPopup();
            levelSelectPopup.HidePopup();
            settingsPopup.HidePopup();
            creditsPopup.HidePopup();
        }
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
