using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class PlayerAwake : State
{
    [BoxGroup("Init State")] public bool dashAbility = true;
    [BoxGroup("Init State")] public bool sleepAbility = true;
    [BoxGroup("Game Events")] public GameEvent sleepEvent;
    [BoxGroup("Game Events")] public GameEvent dashEvent;
    
    [BoxGroup("Debug")] public bool noSleep = false;
    
    [BoxGroup("Setup Variables")] 
    [SerializeField]
    private FloatReference speed;

    [BoxGroup("Setup Variables")] 
    [SerializeField]
    private FloatReference dashSpeed;

    [BoxGroup("Setup Variables")] 
    [SerializeField]
    private FloatReference maxStamina;
    
    [BoxGroup("Setup Variables")] 
    [SerializeField] 
    private FloatReference staminaTime;
    
    [BoxGroup("Setup Variables")] 
    [SerializeField] 
    private FloatReference stamina;
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private bool canDash = true;
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private bool canSleep = false;
    private Rigidbody2D rb;
    private float vert = 0f;
    private float horiz = 0f;
    private Vector2 speedVector = new();
    private SpriteRenderer spriteRenderer;
    private Orientation orientation;
    
    //Input Actions
    private PlayerControls playerControls;
    private Animator animator;

    [FoldoutGroup("Testing")] 
    public Attack playerAttack;

    private bool pauseStamina = false;
    
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        orientation = GetComponent<Orientation>();
        animator = GetComponent<Animator>();
        
        playerControls = new PlayerControls();
        playerControls.PlayerInput.Enable();
        playerControls.PlayerInput.Dash.started += DashStarted;
        playerControls.PlayerInput.Sleep.started += SleepStarted;
        //not needed because player can't attack while awake (keeping it commented in case we use it later)
        playerControls.PlayerInput.Fire.started += FireStarted;
    }

    private void Start()
    {
        //Avoid messy null pointers due to different awake calls
        playerAttack.aiming.SetAimingState(Aiming.AimingState.Aiming);
    }

    public override void EnterState()
    {
        base.EnterState();
        spriteRenderer.color = Color.white;
        staminaTime.Value = maxStamina;
        stamina.Value = staminaTime;
        rb.velocity = Vector2.zero;
        canSleep = true; //get rid of this when you fix the cooldown
        //Uncomment when done testing attack 1st September Alex
        // orientation.facingMode = Orientation.FacingMode.Movement;
    }
    
    public override void UpdateBehaviour()
    {
        Vector2 moveInput = playerControls.PlayerInput.Move.ReadValue<Vector2>();
        vert = moveInput.y;
        horiz = moveInput.x;
        UpdateOrientation();
        UpdateStamina();
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        animator.SetFloat(AnimationHelper.SpeedParameter, Mathf.Abs(vert) + Mathf.Abs(horiz));
    }

    private void UpdateOrientation()
    {
        if (horiz > 0)
        {
            orientation.facingRight = true;
        }
        else if (horiz < 0)
        {
            orientation.facingRight = false;
        }
    }

    public override void FixedUpdateBehaviour()
    {
        if (canDash)
        {
            //can't be overwriting the velocity on a dash

            //update velocity directly for snappy controls
            speedVector.x = horiz * speed;
            speedVector.y = vert * speed;
            rb.velocity = speedVector;
        }
    }

    private void UpdateStamina()
    {
        if (!sleepAbility)
            return;
        if (noSleep)
            return;
        if (pauseStamina)
            return;
        stamina.Value -= Time.deltaTime;

        if (stamina <= 0f)
        {
            StateController.TryEnqueueState<PlayerSleep>();
        }
    }

    private void Dash()
    {
        dashEvent.Raise();
        //dash vector needs to be normalized
        Vector3 reference = CameraManager.instance.GetMouseDirection(transform.position);
        Vector2 dashVector = new Vector2(reference.x, reference.y);
        dashVector = dashVector.normalized;

        rb.velocity = dashSpeed * dashVector;
        canDash = false;

        //wait small time and then allow for dashing
        IEnumerator coroutine = DashCoolDown(0.1f);
        StartCoroutine(coroutine);
    }
    
    private void DashStarted(InputAction.CallbackContext _callbackContext)
    {
        if (!dashAbility)
            return;
        if (StateController.currentState == this && canDash)
            Dash();
    }
    
    private void SleepStarted(InputAction.CallbackContext _callbackContext)
    {
        if (!sleepAbility)
            return;
        if (canSleep)
        {
            if(StateController.TryEnqueueState<PlayerSleep>())
                sleepEvent.Raise();
        }
    }
    
    private void FireStarted(InputAction.CallbackContext _callbackContext)
    {
        if(playerAttack != null)
            playerAttack.Activate();
    }

    public void OnDialogueEventStarted()
    {
        playerControls.Disable();
        pauseStamina = true;
    }

    public void OnDialogueEventEnded()
    {
        playerControls.Enable();
        pauseStamina = false;
    }

    IEnumerator DashCoolDown(float _coolDownTime)
    {
        yield return new WaitForSeconds(_coolDownTime);
        canDash = true;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        playerControls.Disable();
        pauseStamina = true;
    }
    
    private void OnDestroy()
    {
        playerControls.PlayerInput.Disable();
        playerControls.Dispose();
    }
    
}
