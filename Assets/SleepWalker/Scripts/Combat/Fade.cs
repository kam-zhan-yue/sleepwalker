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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FadeIn()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(timeBeforeFade);
        sequence.Append(spriteRenderer.DOFade(1f, floatDuration).SetEase(Ease.Linear));
        sequence.Play();

    }

    [Button]
    public void FadeOut()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(timeBeforeFade);
        sequence.Append(spriteRenderer.DOFade(0f, floatDuration).SetEase(Ease.Linear));
        sequence.Play().OnComplete(() =>
        {
            if(destroyAfterFade)
                Destroy(gameObject);
        });
    }
}
