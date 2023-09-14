using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPopup : Popup
{
    public BoolVariable showLevelSelection;

    protected override void InitPopup()
    {
        mainHolder.gameObject.SetActiveFast(false);
    }

    public void RestartLevelButtonClicked()
    {
        Debug.Log("Restart Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        HidePopup();
    }
    
    public void LevelSelectionButtonClicked()
    {
        Debug.Log("Level Select");
        showLevelSelection.Value = true;
        SceneManager.LoadScene(0);
        HidePopup();
    }

    public void MainMenuButtonClicked()
    {
        Debug.Log("Main Menu");
        SceneManager.LoadScene(0);
        HidePopup();
    }

    public void OnPlayerDead()
    {
        ShowPopup();
    }
}
