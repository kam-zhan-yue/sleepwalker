using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class EnemySleep : State
{
    [BoxGroup("Setup Variables")] public float sleepTime;
    
    //I am so lazy right now, I'm sorry
    [BoxGroup("Setup Variables")] public MiniSleepBarPopup sleepPopup;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float sleepTimer;

    [BoxGroup("Unity Events")] public UnityEvent onSleep;
    [BoxGroup("Unity Events")] public UnityEvent onAwake;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override bool CanEnterState(State _currentState)
    {
        return !StateController.IsCurrentState<BossRetreat>();
    }

    public override void EnterState()
    {
        base.EnterState();
        rb.velocity = Vector2.zero;
        spriteRenderer.color = Color.black;
        sleepTimer = sleepTime;
        if(sleepPopup)
            sleepPopup.ShowPopup();
        onSleep?.Invoke();
    }

    public override void UpdateBehaviour()
    {
        if (sleepTimer > 0f)
        {
            sleepTimer -= Time.deltaTime;
            if (sleepTimer <= 0f)
            {
                StateController.EnterPreviousState();
            }
        }
    }

    public float GetSleepPercentage()
    {
        return sleepTimer / sleepTime;
    }
    
    public override void ExitState()
    {
        base.ExitState();
        spriteRenderer.color = Color.white;
        if(sleepPopup)
            sleepPopup.HidePopup();
        //Don't trigger on boss retreat...
        if(StateController.IsCurrentState<BossRetreat>())
            onAwake?.Invoke();
    }
}