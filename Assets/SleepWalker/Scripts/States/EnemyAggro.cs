using System;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyAggro : State
{
    [BoxGroup("Setup Variables")] public Attack attack;
    [BoxGroup("Setup Variables")] public float speed;
    [BoxGroup("Setup Variables")] public float attackDistance;
    [BoxGroup("Setup Variables")] public float aggroRange;
    [BoxGroup("Setup Variables")] public bool resetDecision;
    [ShowIf("resetDecision")]
    [BoxGroup("Setup Variables")] public Decision decision;
    
    [ShowInInspector, NonSerialized, ReadOnly]
    public Transform target;

    private Rigidbody2D rb;
    private Aiming aiming;
    
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void EnterState()
    {
        base.EnterState();
        aiming = attack.aiming;
        aiming.idle = false;
        Timing.RunCoroutine(AggroRoutine());
    }
    
    private IEnumerator<float> AggroRoutine()
    {
        while (CanAggro())
        {
            AimTarget();
            MoveTowardsTarget();
            if (CanAttack())
                Timing.WaitUntilDone(AttackRoutine());
            yield return Timing.WaitForOneFrame;
        }
        StateController.EnterDefaultState();
        yield return 0f;
    }

    private IEnumerator<float> AttackRoutine()
    {
        bool attackOver = false;
        rb.velocity = Vector2.zero;
        attack.Activate(() =>
        {
            attackOver = true;
        });
        while (!attackOver)
        {
            yield return Timing.WaitForOneFrame;
        }
        yield return 0f;
    }

    private void AimTarget()
    {
        Vector3 direction = transform.DirectionToObject(target);
        aiming.AimWeapon(direction);
    }

    private void MoveTowardsTarget()
    {
        // Calculate the direction from this object to the target
        float distanceToTarget = transform.DistanceToObject(target);
        if (distanceToTarget > attackDistance)
        {
            Vector3 direction = transform.DirectionToObject(target);
            rb.velocity = direction * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    
    private bool CanAttack()
    {
        return transform.DistanceToObject(target) <= attackDistance && !attack.InCooldown();
    }
    
    private bool CanAggro()
    {
        return transform.DistanceToObject(target) <= aggroRange;
    }

    public override void ExitState()
    {
        base.ExitState();
        Timing.KillCoroutines();
        attack.Deactivate();
        rb.velocity = Vector2.zero;
        aiming.ResetAim();
        aiming.idle = true;
        if(resetDecision)
            decision.ToggleActive(true);
    }
}
