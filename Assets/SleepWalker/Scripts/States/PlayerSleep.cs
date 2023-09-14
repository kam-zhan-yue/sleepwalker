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
    //ok so heres the logic

    //check for enemies within a range and that aren't obstructed by a wall
    //check if there's a restriction on choosing a given enemy
    //check if they are alive
    //choose the closest one and dash towards it, attacking it
    //disallow selecting the attacked enemy for a small time
    //redo this process

    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] float maxDistance = 2f;

    private Enemy[] enemies;
    private bool continueAttack = true;

    private List<Enemy> activeEnemies = new List<Enemy>();

    public class Enemy
    {
        public GameObject gameObject;
        public bool canSee = true;
    }

    private void Start()
    {
        GameObject[] startingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = new Enemy[startingEnemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject = startingEnemies[i];
            enemies[i].canSee = true;
        }

        //line up next attack
        IEnumerator coroutine = NextAttack(timeBetweenAttacks/2f);
        StartCoroutine(coroutine);
    }

    public override void UpdateBehaviour()
    {
        if (continueAttack)
        {
            activeEnemies.Clear();
            LookForEnemies();
            if (activeEnemies.Count > 0)
            {
                Vector3 target = ChooseEnemy();

                //dash here in direction of target

                LungeAttack();
            }
        }
    }

    void LookForEnemies()
    {
        //check for enemies within a range and that aren't obstructed by a wall
        for (int i = 0; i < enemies.Length; i++) {
            RaycastHit2D hit = 
                Physics2D.Raycast(transform.position, enemies[i].gameObject.transform.position - transform.position, maxDistance);

            //if it hits something
            if (hit.collider != false)
            {
                //check if its the intended enemy
                if (hit.transform != enemies[i].gameObject.transform)
                {
                    break;
                }
                //check if there's a restriction on choosing a given enemy
                if (!enemies[i].canSee)
                {
                    break;
                }
                //check if its alive
                Health health = enemies[i].gameObject.GetComponent<Health>();
                if (health != null)
                {
                    //not sure about accessing the health yet
                    //if (health <= 0)
                    //{
                        break;
                    //}
                }
                //if still here, add it to list
                activeEnemies.Add(enemies[i]);
            }
        }
        
    }

    Vector2 ChooseEnemy()
    {
        Vector2 playerPos = transform.position;
        Vector2 value = playerPos;
        //choose the closest one
        if (activeEnemies.Count > 0)
        {
            Enemy closestEnemy = enemies[0];
            foreach (Enemy e in enemies)
            {
                Vector2 enemyPos = e.gameObject.transform.position;
                if (Vector2.Distance(playerPos, enemyPos) < Vector2.Distance(playerPos, enemyPos))
                {
                    closestEnemy = e;
                }
            }

            value = closestEnemy.gameObject.transform.position;
            IEnumerator coroutine = IgnoreEnemyFor(timeBetweenAttacks + 1, closestEnemy);
            StartCoroutine(coroutine);
        }

        return value;
    }

    public void LungeAttack()
    {
        //attack
        Debug.Log("Attacking now");

        //line up next attack
        IEnumerator coroutine = NextAttack(timeBetweenAttacks);
        StartCoroutine(coroutine);
    }

    IEnumerator NextAttack(float waitTime)
    {
        continueAttack = false;

        yield return new WaitForSeconds(waitTime);

        LungeAttack();
        continueAttack = true;
    }

    IEnumerator IgnoreEnemyFor(float waitTime, Enemy enemy)
    {
        enemy.canSee = false;

        yield return new WaitForSeconds(waitTime);

        enemy.canSee = true;
    }

    /*
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
    */
}
