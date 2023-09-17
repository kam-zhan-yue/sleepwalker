using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public float timeBeforeFade = 1f;
    [BoxGroup("Setup Variables")] public bool destroyAfterFade = true;
    [BoxGroup("Setup Variables")] public float floatDuration = 1f;
    private SpriteRenderer spriteRenderer;

    private Sequence fadeIn;
    private Sequence fadeOut;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FadeIn()
    {
        fadeIn = DOTween.Sequence();
        fadeIn.AppendInterval(timeBeforeFade);
        fadeIn.Append(spriteRenderer.DOFade(1f, floatDuration).SetEase(Ease.Linear));
        fadeIn.Play();
    }

    [Button]
    public void FadeOut()
    {
        fadeOut = DOTween.Sequence();
        fadeOut.AppendInterval(timeBeforeFade);
        fadeOut.Append(spriteRenderer.DOFade(0f, floatDuration).SetEase(Ease.Linear));
        fadeOut.Play().OnComplete(() =>
        {
            if(destroyAfterFade)
                Destroy(gameObject);
        });
    }

    private void OnDestroy()
    {
        fadeOut.Kill();
        fadeIn.Kill();
    }
}
