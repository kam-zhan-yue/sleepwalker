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

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        damageBody = GetComponent<DamageBody>();
        retreatVector = retreatPosition.position;
    }
    
    public override void EnterState()
    {
        base.EnterState();
        health.ToggleInvulnerability(true);
        damageBody.Deactivate();
        float distance = Vector3.Distance(transform.position, retreatVector);
        float time = distance / retreatSpeed;
        rb.DOMove(retreatVector, time).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                StateController.TryEnqueueState<EnemyIdle>();
            });
    }

    public void ForceActivate()
    {
        StateController.TryEnqueueState<BossRetreat>();
    }

    public void SetEvents(UnityEvent _events)
    {
        onComplete = _events;
    }
}
