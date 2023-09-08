using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class DialoguePopup : MonoBehaviour
{
    [BoxGroup("UI Objects")] public CanvasGroup textPanelCanvasGroup;
    [BoxGroup("UI Objects")] public TMP_Text dialogueText;
    [BoxGroup("Setup Variables")] public Vector3 offset;

    public void Show(Vector3 _position, string _text)
    {
        gameObject.SetActiveFast(true);
        _position.x += offset.x;
        _position.y += offset.y;
        _position.z += offset.z;
        transform.position = _position;
        dialogueText.SetText(_text);
    }

    public void Hide()
    {
        gameObject.SetActiveFast(false);
    }
}