using System;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class EnemyAggro : State
{
    [BoxGroup("Setup Variables")] public Attack attack;
    [BoxGroup("Setup Variables")] public FloatReference speed;
    [BoxGroup("Setup Variables")] public float attackDistance;
    [BoxGroup("Setup Variables")] public float aggroRange;
    [BoxGroup("Setup Variables")] public bool resetDecision;
    [ShowIf("resetDecision")]
    [BoxGroup("Setup Variables")] public Decision decision;
    
    [ShowInInspector, NonSerialized, ReadOnly]
    public Transform target;

    private Rigidbody2D rb;
    private Aiming aiming;

    private CoroutineHandle aggroRoutine;
    private Animator animator;
    private Orientation orientation;
    private DamageBody damageBody;
    private bool hasDecision = false;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        orientation = GetComponent<Orientation>();
        damageBody = GetComponent<DamageBody>();
        hasDecision = decision != null;
    }

    //Need CanAggro to avoid changing states twice in EnterState
    public override bool CanEnterState(State _currentState)
    {
        return CanAggro() && !StateController.IsCurrentState<EnemySleep>();
    }

    public override void EnterState()
    {
        base.EnterState();
        aiming = attack.aiming;
        aiming.SetAimingState(Aiming.AimingState.Aiming);
        aggroRoutine = Timing.RunCoroutine(AggroRoutine());
        if(hasDecision)
            decision.ToggleActive(false);
        orientation.SetAimTarget(target);
        orientation.SetFacingMode(Orientation.FacingMode.Aiming);
    }
    
    private IEnumerator<float> AggroRoutine()
    {
        while (CanAggro())
        {
            AimTarget();
            MoveTowardsTarget();
            if (CanAttack())
            {
                Timing.WaitUntilDone(AttackRoutine());
            }
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
        if (damageBody.InKnockback())
            return;
        // Calculate the direction from this object to the target
        float distanceToTarget = transform.DistanceToObject(target);
        if (distanceToTarget > attackDistance)
        {
            Vector3 direction = transform.DirectionToObject(target);
            rb.velocity = direction * speed;
            animator.SetFloat(AnimationHelper.SpeedParameter, speed);
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetFloat(AnimationHelper.SpeedParameter, 0f);
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
        Timing.KillCoroutines(aggroRoutine);
        attack.Deactivate();
        attack.ResetAim();
        //TODO this will cause enemies to not reset aim when they lose aggro, but also will be bugged if they go to hurt while attacking
        // aiming.ResetAim();
        aiming.SetAimingState(Aiming.AimingState.Idle);
        if (hasDecision && resetDecision)
            decision.ToggleActive(true);
        animator.SetFloat(AnimationHelper.SpeedParameter, 0f);
        // orientation.SetFacingMode(Orientation.FacingMode.Automatic);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        Timing.KillCoroutines(aggroRoutine);
        attack.Deactivate();
    }
    
    private void OnDestroy()
    {
        Timing.KillCoroutines(aggroRoutine);
    }
}
