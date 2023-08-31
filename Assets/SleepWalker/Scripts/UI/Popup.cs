using System;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Popup : MonoBehaviour
{
    [NonSerialized, ShowInInspector, ReadOnly]
    public bool isAnimating = false;
    
    [NonSerialized, ShowInInspector, ReadOnly]
    public bool isShowing = false;
    
    public Action<Popup> onCloseButtonClicked = null;

    private void Awake()
    {
        InitPopup();
    }

    public abstract void InitPopup();
    public abstract void ShowPopup();
    public abstract void HidePopup();


    public virtual void DoneShowing()
    {
        isAnimating = false;
    }

    public virtual void DoneHiding()
    {
        isAnimating = false;
        isShowing = false;
    }

    public virtual void CloseButtonClicked()
    {
        onCloseButtonClicked?.Invoke(this);
    }

    public virtual void EscapeButtonClicked()
    {
        CloseButtonClicked();
    }
}