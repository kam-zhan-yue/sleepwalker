using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

public class Hurt : State
{
    [BoxGroup("Setup Variables")] public float hurtTime;
    private CoroutineHandle hurtRoutine;
    private Brain brain;
    private Health health;
    private bool hasHealth = false;
    private bool hasBrain = false;
    
    protected override void Awake()
    {
        base.Awake();
        brain = GetComponentInChildren<Brain>();
        hasBrain = brain != null;
        health = GetComponent<Health>();
        hasHealth = health != null;
    }
    
    //for now, the player cannot be interrupted during their sleep
    public override bool CanEnterState(State _currentState)
    {
        return !StateController.IsCurrentState<PlayerSleep>() && !StateController.IsCurrentState<EnemySleep>();
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Enter Hurt");
        hurtRoutine = Timing.RunCoroutine(HurtCountdown());
        if (hasBrain)
        {
            brain.Deactivate();
        }
    }

    private IEnumerator<float> HurtCountdown()
    {
        if (hasHealth && health.IsDead())
        {
            //Stay in this state lmao
            yield break;
        }
        yield return Timing.WaitForSeconds(hurtTime);
        Debug.Log("Exit Hurt");
        StateController.EnterPreviousState();
    }

    public override void ExitState()
    {
        base.ExitState();
        Timing.KillCoroutines(hurtRoutine);
        if (hasBrain)
        {
            brain.Activate();
        }
    }
}
