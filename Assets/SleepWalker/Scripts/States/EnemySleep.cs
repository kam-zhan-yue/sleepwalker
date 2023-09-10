using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemySleep : State
{
    [BoxGroup("Setup Variables")] public float sleepTime;
    private CoroutineHandle sleepRoutine;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void EnterState()
    {
        base.EnterState();
        sleepRoutine = Timing.RunCoroutine(Countdown());
        spriteRenderer.color = Color.black;
    }

    private IEnumerator<float> Countdown()
    {
        yield return Timing.WaitForSeconds(sleepTime);
        StateController.EnterPreviousState();
    }

    public override void ExitState()
    {
        base.ExitState();
        Timing.KillCoroutines(sleepRoutine);
        spriteRenderer.color = Color.white;
    }
}