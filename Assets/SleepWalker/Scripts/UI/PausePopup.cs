using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PausePopup : Popup
{
    public GameEvent pauseStarted;
    public GameEvent pauseEnded;
    public VolumePopup volumePopup;
    public BoolVariable canPause;
    public BoolVariable showLevelSelection;
    private UIControls uiControls;
    protected override void InitPopup()
    {
        uiControls = new UIControls();
        uiControls.UIInput.Enable();
        uiControls.UIInput.Pause.started += PauseStarted;
    }
    
    private void PauseStarted(InputAction.CallbackContext _callbackContext)
    {
        if (isShowing)
        {
            ButtonClicked();
            HidePopup();
        }
        else if (canPause.Value)
        {
            ButtonClicked();
            ShowPopup();
        }
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        volumePopup.ShowPopup();
        Time.timeScale = 0f;
        pauseStarted.Raise();
    }

    public override void HidePopup()
    {
        base.HidePopup();
        volumePopup.HidePopup();
        Time.timeScale = 1f;
        pauseEnded.Raise();
    }

    public void RestartLevelPressed()
    {
        ButtonClicked();
        Debug.Log("Restart Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        HidePopup();
    }
    
    public void LevelSelectionButtonClicked()
    {
        ButtonClicked();
        Debug.Log("Level Select");
        showLevelSelection.Value = true;
        SceneManager.LoadScene(0);
        HidePopup();
    }

    public void MainMenuButtonClicked()
    {
        ButtonClicked();
        Debug.Log("Main Menu");
        SceneManager.LoadScene(0);
        HidePopup();
    }
    
    private void OnDestroy()
    {
        uiControls.Dispose();
    }
}
