using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPopup : Popup
{
    [BoxGroup("UI Objects")] public RectTransform mainHolder;
    public override void InitPopup()
    {
        mainHolder.gameObject.SetActiveFast(false);
    }

    public override void ShowPopup()
    {
        mainHolder.gameObject.SetActiveFast(true);
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

    public override void HidePopup()
    {
        mainHolder.gameObject.SetActiveFast(false);
    }
}
