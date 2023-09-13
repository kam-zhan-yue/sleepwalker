using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPopup : Popup
{
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
        HidePopup();
    }

    public void MainMenuButtonClicked()
    {
        Debug.Log("Main Menu");
        HidePopup();
    }

    public void OnPlayerDead()
    {
        ShowPopup();
    }
}
