using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSleep : State
{
    public float sleepTime = 5f;
    private float sleepTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private UIStaminaManager staminaBar;
    private Rigidbody2D rb;

    //input actions
    private PlayerControls playerControls;

    public Attack playerAttack;

    //ai actions
    private List<GameObject> targets = new List<GameObject>();
    private CircleCollider2D trigger;
    const int BEGIN = -1;
    const int ROAM = 0;
    const int APPROACH = 1;
    const int LUNGE = 2;
    private int aiState = BEGIN;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        staminaBar = GetComponent<UIStaminaManager>();
        rb = GetComponent<Rigidbody2D>();

        playerControls = new PlayerControls();
        playerControls.PlayerInput.Enable();

        //ai initialisation
        trigger = GetComponent<CircleCollider2D>();
        trigger.radius = 0.1f;
        IEnumerator coroutine = Begin(5f);
        StartCoroutine(coroutine);

        if (!trigger.isTrigger)
        {
            trigger.isTrigger = true;
        }
    }

    public override void EnterState()
    {
        base.EnterState();
        sleepTimer = sleepTime;
        staminaBar.UpdateMaxValue(sleepTime);
        spriteRenderer.color = Color.red;
        rb.velocity = Vector2.zero;
    }

    public override void UpdateBehaviour()
    {
        sleepTimer -= Time.deltaTime;
        staminaBar.UpdateDisplayValue(sleepTimer);

        if (sleepTimer <= 0f)
        {
            StateController.TryEnqueueState<PlayerAwake>();
        }
    }

    private void FireStarted(InputAction.CallbackContext _callbackContext)
    {
        if (playerAttack != null)
            playerAttack.Activate();
    }

    //ai states 
    private IEnumerator Begin(float maxRadius, float damp = 0.3f)
    {
        while (trigger.radius < maxRadius - 0.1f)
        {
            trigger.radius = Mathf.Lerp(trigger.radius, maxRadius, damp);

            yield return null;
        }

        if (aiState == BEGIN)
        {
            Roam();
        }

        yield return null;
    }
    private void Roam() //when no one enters the trigger, find a random point and chase it
    {
        aiState = ROAM;
    }
    private void Approach() //when someone enters the trigger, find their position and chase it
    {
        aiState = APPROACH;
    }
    private void Lunge() //when someone is close enough, attack them
    {
        aiState = LUNGE;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Sleep Gas")
        {

        }
    }

}
