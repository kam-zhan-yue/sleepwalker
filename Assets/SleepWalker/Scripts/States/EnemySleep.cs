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
    [BoxGroup("Setup Variables")] public Attack attack;
    private Rigidbody2D rb;
    private float sleepTimer;

    [BoxGroup("Unity Events")] public UnityEvent onSleep;
    [BoxGroup("Unity Events")] public UnityEvent onAwake;

    [SerializeField, ReadOnly] private ParticleSystem zzzParticles;

    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        for (int i = 0; i < transform.childCount; i++)
        {
            ParticleSystem inChild = transform.GetChild(i).GetComponent<ParticleSystem>();
            if (inChild != null)
            {
                zzzParticles = inChild;
            }
        }
    }

    public override bool CanEnterState(State _currentState)
    {
        return !StateController.IsCurrentState<BossRetreat>();
    }

    public override void EnterState()
    {
        base.EnterState();
        attack.UnInit();
        rb.velocity = Vector2.zero;
        sleepTimer = sleepTime;
        if(sleepPopup)
            sleepPopup.ShowPopup();
        onSleep?.Invoke();
        zzzParticles.Play();
        Debug.Log("Sleeping True");
        animator.SetBool(AnimationHelper.SleepParameter, true);
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
        Debug.Log("Exit State");
        base.ExitState();
        if(sleepPopup)
            sleepPopup.HidePopup();
        Debug.Log("Sleeping False");
        animator.SetBool(AnimationHelper.SleepParameter, false);
        //Don't trigger on boss retreat...
        if(StateController.IsCurrentState<BossRetreat>())
            onAwake?.Invoke();
        zzzParticles.Stop();
    }
}