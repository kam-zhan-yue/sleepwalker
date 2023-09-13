using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class EndGamePopup : Popup
{
    [BoxGroup("UI Objects")] public RectTransform mainHolder;
    [BoxGroup("UI Objects")] public RectTransform worldEndHolder;
    [BoxGroup("UI Objects")] public RectTransform gameOverHolder;
    [BoxGroup("UI Objects")] public RectTransform thankYouHolder;
    [BoxGroup("UI Objects")] public RectTransform mainMenuHolder;

    private bool showingLastScreen = false;
    
    public override void InitPopup()
    {
        mainHolder.gameObject.SetActiveFast(false);
        worldEndHolder.gameObject.SetActiveFast(false);
        gameOverHolder.gameObject.SetActiveFast(false);
        thankYouHolder.gameObject.SetActiveFast(false);
        mainMenuHolder.gameObject.SetActiveFast(false);
    }

    public override void ShowPopup()
    {
        Time.timeScale = 0f;
        mainHolder.gameObject.SetActive(true);
    }

    [Button]
    public void WorldEnd()
    {
        ShowPopup();
        worldEndHolder.gameObject.SetActiveFast(true);
    }

    [Button]
    public void GameOver()
    {
        ShowPopup();
        gameOverHolder.gameObject.SetActiveFast(true);
    }

    public override void HidePopup()
    {
        mainHolder.gameObject.SetActiveFast(false);
        worldEndHolder.gameObject.SetActiveFast(false);
        gameOverHolder.gameObject.SetActiveFast(false);
        thankYouHolder.gameObject.SetActiveFast(false);
        mainMenuHolder.gameObject.SetActiveFast(false);
    }

    public void OnClick()
    {
        if (showingLastScreen)
            return;
        worldEndHolder.gameObject.SetActiveFast(false);
        gameOverHolder.gameObject.SetActiveFast(false);
        if (!thankYouHolder.gameObject.activeSelf)
        {
            thankYouHolder.gameObject.SetActiveFast(true);
        }
        else
        {
            showingLastScreen = true;
            thankYouHolder.gameObject.SetActiveFast(false);
            mainMenuHolder.gameObject.SetActiveFast(true);
        }
    }

    public void MainMenuButtonClicked()
    {
        Debug.Log("Return to Main Menu");
        Time.timeScale = 1f;
    }
}
