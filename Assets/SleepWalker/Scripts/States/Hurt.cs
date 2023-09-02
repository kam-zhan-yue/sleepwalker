using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

public class Hurt : State
{
    [BoxGroup("Setup Variables")] public float hurtTime;
    private SpriteRenderer spriteRenderer;
    private CoroutineHandle hurtRoutine;
    
    //for now, the player cannot be interrupted during their sleep
    public override bool CanEnterState(State _currentState)
    {
        return !StateController.IsCurrentState<PlayerSleep>();
    }

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    public override void EnterState()
    {
        base.EnterState();
        hurtRoutine = Timing.RunCoroutine(HurtCountdown());
        spriteRenderer.color = Color.blue;
    }

    private IEnumerator<float> HurtCountdown()
    {
        yield return Timing.WaitForSeconds(hurtTime);
        StateController.EnterPreviousState();
    }

    public override void ExitState()
    {
        base.ExitState();
        Timing.KillCoroutines(hurtRoutine);
        spriteRenderer.color = Color.white;
    }
}
