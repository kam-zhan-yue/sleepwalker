using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerSleep : State
{
    [BoxGroup("Setup Variables")] public CircleCollider2D trigger;
    [BoxGroup("Setup Variables")] public FloatReference maxSleepTime;
    [BoxGroup("Setup Variables")] public FloatReference staminaTime;
    [SerializeField]
    [BoxGroup("Setup Variables")] private FloatReference stamina;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    //input actions
    private PlayerControls playerControls;

    public Attack playerAttack;

    //ai actions
    private List<Transform> targets = new List<Transform>();
    private Vector2 target;
    const int DEACTIVATE = -2;
    const int BEGIN = -1;
    const int ROAM = 0;
    const int APPROACH = 1;
    const int LUNGE = 2;
    
    private int aiState = BEGIN;
    private bool followTarget = false;

    [Header("AI Behaviour")]
    [SerializeField] private float maxTargetDistance = 7f;
    [SerializeField] private float minTargetDistance = 1f;
    [SerializeField] private float bufferDistance = 1f;
    [SerializeField] private FloatReference speed;

    [Header("Debugging")]
    [SerializeField] TMPro.TextMeshProUGUI stateText;

    private bool pauseStamina = false;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        playerControls = new PlayerControls();
        playerControls.PlayerInput.Enable();
        if(trigger == null)
            Debug.LogError("Trigger not found.");
    }

    public override void EnterState()
    {
        base.EnterState();
        staminaTime.Value = maxSleepTime;
        stamina.Value = staminaTime;
        spriteRenderer.color = Color.red;
        rb.velocity = Vector2.zero;

        //ai initialisation
        trigger.radius = 0.01f;
        IEnumerator coroutine = Begin(5f);
        StartCoroutine(coroutine);

        if (!trigger.isTrigger)
        {
            trigger.isTrigger = true;
        }
    }

    public override void UpdateBehaviour()
    {
        if(!pauseStamina)
            stamina.Value -= Time.deltaTime;
        if (aiState == DEACTIVATE)
            return;
        // stateText.text = $"Sleep State: {aiState}";

        if (stamina <= 0f)
        {
            StateController.TryEnqueueState<PlayerAwake>();
        }

        if (followTarget)
        {
            if (aiState == ROAM)
            {
                //check if state needs to change
                if (targets.Count > 0)
                {
                    Approach(targets[Random.Range(0, targets.Count - 1)].position);
                }
            }

            //ai behaviour
            if (Vector2.Distance(transform.position, target) < 1f)
            {
                if (targets.Count > 0)
                {
                    Approach(targets[Random.Range(0, targets.Count-1)].position);
                }
                else
                {
                    Roam();
                }
            }

            rb.velocity = speed * (target - (Vector2)transform.position).normalized;
        }  else
        {
            //make sure this doesn't overwrite fallback
            rb.velocity = Vector2.zero;
        }

        Debug.DrawLine(transform.position,target, Color.green);

        foreach (Transform t in targets)
        {
            Debug.DrawLine(transform.position, t.position, Color.yellow);
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

        followTarget = true;

        yield return null;
    }
    private void Roam() //when no one enters the trigger, find a random point and chase it
    {
        aiState = ROAM;

        //find random direction
        Vector2 direction = new Vector2(Random.Range(-1f,1f), Random.Range(-1f, 1f));
        direction = direction.normalized;

        //check if there's enough space to move in that direction
        //RaycastHit hit;
        Vector2 targetSpot;
        float avgScale = transform.localScale.x + transform.localScale.y / 2f;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxTargetDistance);

        if (hit.collider != null)
        {
            if (hit.distance < minTargetDistance + bufferDistance)
            {
                //if no, repeat this process
                Roam();
                return;
            } else
            {
                //if yes, set that as the target
                targetSpot = (Vector2)transform.position + ((hit.distance - ((avgScale / 2f)+bufferDistance)) * direction);
            }
        } else
        {
            //if yes, set that as the target
            targetSpot = (Vector2)transform.position + ((maxTargetDistance - (avgScale / 2f)) * direction);
        }

        target = targetSpot;
    }
    private void Approach(Vector2 approach) //when someone enters the trigger, find their position and chase it
    {
        aiState = APPROACH;

        target = approach;
    }
    private void Lunge() //when someone is close enough, attack them
    {
        aiState = LUNGE;

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject col = collision.gameObject;
        //add pontential targets to list
        if (col.layer == LayerMask.NameToLayer("Enemies") || col.layer == LayerMask.NameToLayer("Barrel"))
        {
            targets.Add(col.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject col = collision.gameObject;
        //remove targets as they move away
        if (col.layer == LayerMask.NameToLayer("Enemies") || col.layer == LayerMask.NameToLayer("Barrel"))
        {
            targets.Remove(col.transform);
        }
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

    public override void Deactivate()
    {
        base.Deactivate();
        playerControls.Disable();
        aiState = DEACTIVATE;
        rb.velocity = Vector2.zero;
    }

    private void OnDestroy()
    {
        playerControls.Dispose();
    }

}
