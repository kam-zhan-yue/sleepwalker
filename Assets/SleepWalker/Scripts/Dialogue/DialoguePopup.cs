using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class DialoguePopup : Popup
{
    [BoxGroup("UI Objects")] public TMP_Text nameText;
    [BoxGroup("UI Objects")] public TMP_Text dialogueText;
    [BoxGroup("Setup Variables")] public Vector3 offset;
    [BoxGroup("Setup Variables")] public float speed = 5f;
    private Tween typeWriterTween;

    public void Show(Vector3 _position, string _name, string _text, Action _onComplete = null)
    {
        gameObject.SetActiveFast(true);
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
                dialogueText.SetText(text);
            })
            .OnComplete(() =>
            {
                dialogueText.SetText(_text);
                _onComplete?.Invoke();
            }).OnKill(() =>
            {
                dialogueText.SetText(_text);
                _onComplete?.Invoke();
            });
    }

    public void Stop()
    {
        typeWriterTween.Kill();
    }

    public void Hide()
    {
        gameObject.SetActiveFast(false);
    }

    protected override void InitPopup()
    {
        
    }
}