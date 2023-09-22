using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class DialoguePopup : Popup
{
    [FoldoutGroup("Audio")] public bool hasAudio;
    [FoldoutGroup("Audio"), ShowIf("hasAudio")] public BoolReference paused;
    [FoldoutGroup("Audio"), ShowIf("hasAudio")] public FloatVariable volume;
    [FoldoutGroup("Audio"), ShowIf("hasAudio")] public AudioSource audioSource;
    [BoxGroup("UI Objects")] public TMP_Text nameText;
    [BoxGroup("UI Objects")] public TMP_Text dialogueText;
    [BoxGroup("Setup Variables")] public Vector3 offset;
    [BoxGroup("Setup Variables")] public float speed = 5f;
    private Tween typeWriterTween;
    private float originalVolume;

    private void Awake()
    {
        if (hasAudio)
            originalVolume = audioSource.volume;
    }

    public void Show(Vector3 _position, string _name, string _text, Action _onComplete = null)
    {
        gameObject.SetActiveFast(true);
        if (hasAudio)
        {
            audioSource.volume = volume.Value * originalVolume;
            audioSource.Play();
        }
        _position.x += offset.x;
        _position.y += offset.y;
        _position.z += offset.z;
        transform.position = _position;
        nameText.SetText(_name);
        // dialogueText.SetText(_text);
        string text = string.Empty;
        typeWriterTween = DOTween.To(() => text, _x => text = _x, _text, _text.Length / speed)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                if (hasAudio)
                {
                    if(paused.Value)
                        audioSource.Pause();
                    else if (!audioSource.isPlaying)
                    {
                        audioSource.volume = volume.Value * originalVolume;
                        audioSource.Play();
                    }
                }
                dialogueText.SetText(text);
            })
            .OnComplete(() =>
            {
                if(hasAudio)
                    audioSource.Stop();
                dialogueText.SetText(_text);
                _onComplete?.Invoke();
            }).OnKill(() =>
            {
                if(hasAudio)
                    audioSource.Stop();
                dialogueText.SetText(_text);
                _onComplete?.Invoke();
            });
    }

    public void Stop()
    {
        //gibHandler.PauseGibberish();
        typeWriterTween.Kill();
    }

    public void Hide()
    {
       // gibHandler.PauseGibberish();
        gameObject.SetActiveFast(false);
    }

    protected override void InitPopup()
    {
        
    }
}