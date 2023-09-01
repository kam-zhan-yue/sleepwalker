using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerAwake : State
{
    [NonSerialized, ShowInInspector, ReadOnly] 
    private float speed = 10f;
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private float dashSpeed = 50f;
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private float awakeTime = 15f;
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private bool canDash = true;
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private bool canSleep = false;
    
    private float awakeTimer;
    private UIStaminaManager staminaBar;
    private Rigidbody2D rb;
    private float vert = 0f;
    private float horiz = 0f;
    private Vector2 speedVector = new();
    private SpriteRenderer spriteRenderer;
    private Orientation orientation;
    
    //Input Actions
    private PlayerControls playerControls;

    [FoldoutGroup("Testing")] 
    public Attack playerAttack;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        staminaBar = GetComponent<UIStaminaManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        orientation = GetComponent<Orientation>();
        
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
        playerAttack.aiming.idle = false;
    }

    public override void EnterState()
    {
        base.EnterState();
        spriteRenderer.color = Color.white;
        awakeTimer = awakeTime;
        staminaBar.UpdateMaxValue(awakeTime);
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
        awakeTimer -= Time.deltaTime;
        staminaBar.UpdateDisplayValue(awakeTimer);

        if (awakeTimer <= 0f)
        {
            StateController.TryEnqueueState<PlayerSleep>();
        }
    }

    private void Dash()
    {
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
        if (StateController.currentState == this && canDash)
            Dash();
    }
    
    private void SleepStarted(InputAction.CallbackContext _callbackContext)
    {
        if(canSleep)
            StateController.TryEnqueueState<PlayerSleep>();
    }
    
    private void FireStarted(InputAction.CallbackContext _callbackContext)
    {
        if(playerAttack != null)
            playerAttack.Activate();
    }

    IEnumerator DashCoolDown(float _coolDownTime)
    {
        yield return new WaitForSeconds(_coolDownTime);
        canDash = true;
    }

    private void OnDestroy()
    {
        playerControls.PlayerInput.Disable();
        playerControls.Dispose();
    }
}
