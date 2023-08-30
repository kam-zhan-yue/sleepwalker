using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwake : State
{
    [SerializeField] float speed = 10f;
    [SerializeField] float dashSpeed = 25f;
    [SerializeField] float awakeTime = 15f;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool canSleep = false;
    private float awakeTimer;
    private UIStaminaManager staminaBar;
    private Rigidbody2D rb;
    private float vert = 0f;
    private float horiz = 0f;
    private Vector2 speedVector = new();
    private SpriteRenderer spriteRenderer;
    private Orientation orientation;
    private Camera mainCamera;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        staminaBar = GetComponent<UIStaminaManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        orientation = GetComponent<Orientation>();
        mainCamera = Camera.main;
    }

    public override void EnterState()
    {
        base.EnterState();
        spriteRenderer.color = Color.white;
        awakeTimer = awakeTime;
        staminaBar.UpdateMaxValue(awakeTime);
        rb.velocity = Vector2.zero;
        canSleep = true; //get rid of this when you fix the cooldown
        orientation.facingMode = Orientation.FacingMode.Movement;
    }
    
    public override void UpdateBehaviour()
    {
        vert = Input.GetAxis("Vertical");
        horiz = Input.GetAxis("Horizontal");

        UpdateOrientation();

        if (Input.GetKeyDown(KeyCode.Space) && canSleep)
        {
            StateController.TryEnqueueState<PlayerSleep>();
        }

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

            if (Input.GetButtonDown("Dash"))
            {
                Dash();
            }
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
        rb.velocity = dashSpeed * FindMouseDirection();
        canDash = false;

        //wait small time and then allow for dashing
        IEnumerator coroutine = DashCoolDown(0.1f);
        StartCoroutine(coroutine);
    }

    IEnumerator DashCoolDown(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        canDash = true;
    }

    private Vector3 FindMouseDirection()
    {
        //get mouse pointer position
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 0;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        //find direction from current player to mouse
        Vector3 mouseDirection = (mouseWorldPosition - transform.position).normalized;

        return mouseDirection;
    }
}
