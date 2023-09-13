using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class BossRetreat : State
{
    [BoxGroup("Setup Variables")] public Transform retreatPosition;
    [BoxGroup("Setup Variables")] public float retreatSpeed = 5f;
    private Rigidbody2D rb;

    private Vector3 retreatVector;
    private Health health;
    private DamageBody damageBody;
    private UnityEvent onComplete;
    private Tween rbTween;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        damageBody = GetComponent<DamageBody>();
        retreatVector = retreatPosition.position;
    }

    public override bool CanEnterState(State _currentState)
    {
        //Stop a bug where boss retreat is called twice
        return !StateController.IsCurrentState<BossRetreat>();
    }
    
    public override void EnterState()
    {
        base.EnterState();
        health.ToggleInvulnerability(true);
        damageBody.Deactivate();
        float distance = Vector3.Distance(transform.position, retreatVector);
        float time = distance / retreatSpeed;
        rbTween.Kill();
        rbTween = rb.DOMove(retreatVector, time).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Debug.Log("Invoke");
                onComplete?.Invoke();
                StateController.TryEnqueueState<EnemyIdle>();
            });
    }

    public void ForceActivate()
    {
        StateController.TryEnqueueState<BossRetreat>();
    }

    public void ToggleInvulnerability(bool _toggle)
    {
        health.ToggleInvulnerability(_toggle);
    }

    public void ForceAttack()
    {
        //FOR TESTING PURPOSES, SET TO FALSE
        health.ToggleInvulnerability(true);
        damageBody.Activate();
        StateController.TryEnqueueState<EnemyAggro>();
    }

    public void SetEvents(UnityEvent _events)
    {
        onComplete = _events;
    }
}
