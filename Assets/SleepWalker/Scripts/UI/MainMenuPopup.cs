using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPopup : Popup
{
    [BoxGroup("UI Objects")] public LevelSelectPopup levelSelectPopup;
    [BoxGroup("UI Objects")] public SettingsPopup settingsPopup;
    [BoxGroup("UI Objects")] public CreditsPopup creditsPopup;
    [BoxGroup("UI Objects")] public BoolVariable showLevelSelection;
    [BoxGroup("Animation")] public List<GameObject> buttonList = new();
    [BoxGroup("Animation")] public float animationDuration = 1f;
    [BoxGroup("Animation")] public float animationDelay = 1f;
    [BoxGroup("Animation")] public Ease easing = Ease.OutElastic;

    private CoroutineHandle buttonRoutine;
    
    protected override void InitPopup()
    {
        ShowPopup();
    }

    public void PlayButtonClicked()
    {
        ButtonClicked();
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
            PlayButtonAnimations();
        }
    }

    [Button]
    private void PlayButtonAnimations()
    {
        // Sequence sequence = DOTween.Sequence();
        // for (int i = 0; i < buttonList.Count; ++i)
        // {
        //     buttonList[i].gameObject.transform.localScale = Vector3.zero;
        //     sequence.Append(buttonList[i].gameObject.transform.DOScale(Vector3.one, animationDuration).SetEase(easing));
        //     sequence.AppendInterval(animationDelay);
        // }
        // sequence.Play();
        // Debug.Log("Playing Sequence");
        buttonRoutine = Timing.RunCoroutine(ButtonAnimations().CancelWith(gameObject));
    }

    private IEnumerator<float> ButtonAnimations()
    {
        for (int i = 0; i < buttonList.Count; ++i)
        {
            buttonList[i].gameObject.transform.localScale = Vector3.zero;
        }
        
        for (int i = 0; i < buttonList.Count; ++i)
        {
            buttonList[i].gameObject.transform.DOScale(Vector3.one, animationDuration).SetEase(easing);
            yield return Timing.WaitForSeconds(animationDelay);
        }
    }

    public void LevelSelectButtonClicked()
    {
        ButtonClicked();
        HidePopup();
        levelSelectPopup.ShowPopup();
    }

    public void SettingsButtonClicked()
    {
        ButtonClicked();
        HidePopup();
        settingsPopup.ShowPopup();
    }
    
    public void CreditsButtonClicked()
    {
        ButtonClicked();
        HidePopup();
        creditsPopup.ShowPopup();
    }

    public override void CloseButtonClicked()
    {
        ButtonClicked();
        base.CloseButtonClicked();
        levelSelectPopup.HidePopup();
        settingsPopup.HidePopup();
        creditsPopup.HidePopup();
        ShowPopup();
    }
}
