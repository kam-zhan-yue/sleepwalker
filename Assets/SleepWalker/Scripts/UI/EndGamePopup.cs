using System.Collections;
using System.Collections.Generic;
using System.Text;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class EndGamePopup : Popup
{
    [BoxGroup("UI Objects")] public RectTransform worldEndHolder;
    [BoxGroup("UI Objects")] public RectTransform gameOverHolder;
    [BoxGroup("UI Objects")] public RectTransform thankYouHolder;
    [BoxGroup("UI Objects")] public RectTransform mainMenuHolder;
    [BoxGroup("System Objects")] public BoolVariable canPause;

    private bool showingLastScreen = false;
    
    protected override void InitPopup()
    {
        mainHolder.gameObject.SetActiveFast(false);
        worldEndHolder.gameObject.SetActiveFast(false);
        gameOverHolder.gameObject.SetActiveFast(false);
        thankYouHolder.gameObject.SetActiveFast(false);
        mainMenuHolder.gameObject.SetActiveFast(false);
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
        Time.timeScale = 0f;
        mainHolder.gameObject.SetActive(true);
        canPause.Value = false;
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
        base.HidePopup();
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
        ButtonClicked();
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
        ButtonClicked();
        Debug.Log("Return to Main Menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
