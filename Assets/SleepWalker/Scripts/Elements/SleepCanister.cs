using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class SleepCanister : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public Sprite defaultSprite;
    [BoxGroup("Setup Variables")] public Sprite highlightedSprite;
    [BoxGroup("Setup Variables")] public Sprite activatedSprite;
    [BoxGroup("Setup Variables")] public Collider2D sleepCollider;
    [BoxGroup("Setup Variables")] public Fade fade;
    [BoxGroup("Setup Variables")] public float timeActive;

    public UnityEvent onActivate;

    private SpriteRenderer spriteRenderer;
    private CoroutineHandle countdownRoutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sleepCollider.enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D _collider2D)
    {
        if (_collider2D.gameObject.TryGetComponent(out StateController stateController))
        {
            //Don't sleep if dead
            if (_collider2D.gameObject.TryGetComponent(out Health health))
            {
                if (health.IsDead())
                    return;
            }
            stateController.TryEnqueueState<PlayerSleep>();
            stateController.TryEnqueueState<EnemySleep>();
        }
    }

    public void Highlight(bool _toggle)
    {
        if (_toggle)
        {
            // spriteRenderer.sprite = highlightedSprite;
        }
        else
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }

    public void Activate()
    {
        spriteRenderer.sprite = activatedSprite;
        countdownRoutine = Timing.RunCoroutine(Countdown());
    }

    private IEnumerator<float> Countdown()
    {
        sleepCollider.enabled = true;
        yield return Timing.WaitForSeconds(timeActive);
        sleepCollider.enabled = false;
    }

    public void Reactivate()
    {
        spriteRenderer.sprite = defaultSprite;
    }
}
