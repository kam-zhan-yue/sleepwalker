using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class SleepCanister : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public GameObject highlight;
    [BoxGroup("Setup Variables")] public Collider2D sleepCollider;
    [BoxGroup("Setup Variables")] public Fade fade;
    [BoxGroup("Setup Variables")] public float timeActive;

    public UnityEvent onActivate;

    private CoroutineHandle countdownRoutine;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sleepCollider.enabled = false;
        highlight.SetActiveFast(false);
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
        highlight.SetActiveFast(_toggle);
    }

    public void Activate()
    {
        animator.StopPlayback();
        highlight.SetActiveFast(false);
        animator.SetTrigger(AnimationHelper.ActivateParameter);
        countdownRoutine = Timing.RunCoroutine(Countdown());
        onActivate?.Invoke();
    }

    private IEnumerator<float> Countdown()
    {
        sleepCollider.enabled = true;
        yield return Timing.WaitForSeconds(timeActive);
        sleepCollider.enabled = false;
    }

    public void Reactivate()
    {
        animator.SetTrigger(AnimationHelper.ReactivateParameter);
    }

    private void OnDestroy()
    {
        Timing.KillCoroutines(countdownRoutine);
    }
}
