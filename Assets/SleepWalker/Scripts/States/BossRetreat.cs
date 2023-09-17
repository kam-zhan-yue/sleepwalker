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
    [BoxGroup("Setup Variables")] public Aiming aiming;
    [BoxGroup("Setup Variables")] public Attack attack;
    [BoxGroup("Setup Variables")] public Brain brain;
    [BoxGroup("Setup Variables")] public Health health;
    private Rigidbody2D rb;

    private Vector3 retreatVector;
    private DamageBody damageBody;
    private UnityEvent onComplete;
    private Tween rbTween;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
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
        brain.Deactivate();
        attack.UnInit();
        aiming.ResetAim();
        health.ToggleInvulnerability(true);
        health.ToggleTargetable(false);
        damageBody.Deactivate();
        float distance = Vector3.Distance(transform.position, retreatVector);
        float time = distance / retreatSpeed;
        rbTween.Kill();
        rbTween = rb.DOMove(retreatVector, time).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                // StateController.TryEnqueueState<EnemyIdle>();
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

    public override void ExitState()
    {
        base.ExitState();
        attack.ReInit();
    }

    public void ForceAttack()
    {
        Debug.Log("Force Attack");
        Debug.Log(health);
        health.ToggleTargetable(true);
        health.ToggleInvulnerability(true);
        damageBody.Activate();
        StateController.TryEnqueueState<EnemyAggro>();
    }

    public void SetEvents(UnityEvent _events)
    {
        onComplete = _events;
    }
}
