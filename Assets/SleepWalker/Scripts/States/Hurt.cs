using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

public class Hurt : State
{
    [BoxGroup("Setup Variables")] public float hurtTime;
    private SpriteRenderer spriteRenderer;
    

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    public override void EnterState()
    {
        base.EnterState();
        Timing.RunCoroutine(HurtCountdown());
        spriteRenderer.color = Color.blue;
    }

    private IEnumerator<float> HurtCountdown()
    {
        yield return Timing.WaitForSeconds(hurtTime);
        StateController.EnterDefaultState();
    }

    public override void ExitState()
    {
        base.ExitState();
        Timing.KillCoroutines();
        spriteRenderer.color = Color.white;
    }
}
