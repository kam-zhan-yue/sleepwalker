using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerSleep : State
{
    public enum SleepState
    {
        Attacking,
        Deactivated
    }
    
    //ok so heres the logic

    //check for enemies within a range and that aren't obstructed by a wall
    //check if there's a restriction on choosing a given enemy
    //check if they are alive
    //choose the closest one and dash towards it, attacking it
    //disallow selecting the attacked enemy for a small time
    //redo this process
    [BoxGroup("Setup Variables")] public FloatReference speed;
    [BoxGroup("Setup Variables")] public FloatReference maxSleepTime;
    [BoxGroup("Setup Variables")] public FloatReference staminaTime;
    [SerializeField]
    [BoxGroup("Setup Variables")] private FloatReference stamina;
    
    [BoxGroup("Setup Variables")] public GameObjectRuntimeSet enemyRuntimeSet;
    [BoxGroup("Setup Variables")] public Attack playerAttack;
    [BoxGroup("Setup Variables")] public float stopMoveDistance;
    [BoxGroup("Setup Variables")] public float attackDistance;
    
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] float maxDistance = 2f;

    private List<Enemy> enemies = new();
    private bool continueAttack = true;

    private List<Enemy> activeEnemies = new();
    private Rigidbody2D rb;
    private Aiming aiming;
    private Orientation orientation;
    private DamageBody damageBody;
    private Animator animator;
    private readonly RaycastHit2D[] hits = new RaycastHit2D[10];
    private readonly LayerMask enemyLayerMask = LayerHelper.obstaclesLayerMask;

    [NonSerialized, ShowInInspector, ReadOnly]
    private SleepState sleepState;
    
    private CoroutineHandle aggroRoutine;
    private CoroutineHandle attackRoutine;
    private bool pauseStamina = false;
    private Vector3 target = Vector3.zero;
    private bool hasTarget;
    
    public class Enemy
    {
        public GameObject gameObject;
        public bool canSee = true;
    }

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        orientation = GetComponent<Orientation>();
        damageBody = GetComponent<DamageBody>();
    }

    public override void EnterState()
    {
        base.EnterState();
        
        //Setup Variables
        staminaTime.Value = maxSleepTime;
        stamina.Value = staminaTime;
        playerAttack.ReInit();
        aiming = playerAttack.aiming;
        aiming.SetAimingState(Aiming.AimingState.Aiming);
        
        aggroRoutine = Timing.RunCoroutine(AggroRoutine());
        attackRoutine = Timing.RunCoroutine(AttackRoutine());
    }

    public override void UpdateBehaviour()
    {
        if (!pauseStamina)
            stamina.Value -= Time.deltaTime;

        if (sleepState == SleepState.Deactivated)
            return;
        // stateText.text = $"Sleep State: {aiState}";

        if (stamina <= 0f)
        {
            StateController.TryEnqueueState<PlayerAwake>();
        }
    }

    private IEnumerator<float> AggroRoutine()
    {
        //Ignore if in deactivated mode
        while (true)
        {
            if (sleepState == SleepState.Deactivated)
            {
                yield return Timing.WaitForOneFrame;
            }
            
            LookForEnemies();
            ChooseEnemy();
            MoveTowardsTarget();
            yield return Timing.WaitForOneFrame;
        }
    }

    private IEnumerator<float> AttackRoutine()
    {
        //Attack non-stop until out of sleep!
        while (true)
        {
            //If deactivated, don't bother
            if (sleepState == SleepState.Deactivated)
            {
                yield return Timing.WaitForOneFrame;
                continue;
            }
            //if no target, don't bother
            if (!hasTarget)
            {
                orientation.SetFacingMode(Orientation.FacingMode.Movement);
                yield return Timing.WaitForOneFrame;
                continue;
            }
            
            Vector3 direction = transform.DirectionToPoint(target);
            aiming.AimWeapon(direction);
            orientation.SetAimTargetPosition(target);
            orientation.SetFacingMode(Orientation.FacingMode.Aiming);
            
            float distanceToTarget = Vector2.Distance(transform.position, target);
            //if the target is out of distance, don't bother
            if (distanceToTarget > attackDistance)
                yield return Timing.WaitForOneFrame;
            //If attack in cooldown, don't bother
            if (playerAttack.InCooldown())
                yield return Timing.WaitForOneFrame;
            bool attackOver = false;
            playerAttack.Activate(() =>
            {
                attackOver = true;
            });
            while (!attackOver)
            {
                yield return Timing.WaitForOneFrame;
            }
        }
    }

    private void MoveTowardsTarget()
    {
        //Don't move if in knockback or target is null
        if (damageBody.InKnockback())
            return;
        if (target == Vector3.zero)
        {
            rb.velocity = Vector2.zero;
            animator.SetFloat(AnimationHelper.SpeedParameter, 0f);
            return;
        }

        // Debug.Log($"Target: {target}");
        // Calculate the direction from this object to the target
        float distanceToTarget = transform.DistanceToPoint(target);
        if (distanceToTarget > stopMoveDistance)
        {
            Vector3 direction = transform.DirectionToPoint(target);
            // Debug.Log($"Direction: {direction}");
            rb.velocity = direction * speed;
            animator.SetFloat(AnimationHelper.SpeedParameter, speed);
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetFloat(AnimationHelper.SpeedParameter, 0f);
        }
    }
    
    private void LookForEnemies()
    {
        //check for enemies within a range and that aren't obstructed by a wall
        activeEnemies.Clear();
        enemies = new List<Enemy>();
        for (int i = 0; i < enemyRuntimeSet.items.Count; ++i)
        {
            Enemy enemy = new()
            {
                gameObject = enemyRuntimeSet.items[i],
                canSee = true
            };
            enemies.Add(enemy);
        }
        
        for (int i = 0; i < enemies.Count; i++) {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = enemies[i].gameObject.transform.position;
            //Skip if the enemy is too far
            float distance = Vector2.Distance(startPosition, endPosition);
            if (distance > maxDistance)
                continue;
            
            int hitCount = Physics2D.RaycastNonAlloc(startPosition, endPosition - startPosition, hits, maxDistance, enemyLayerMask);
            //Skip if there are obstacles (hitCount > 0)
            if (hitCount == 0)
            {
                if (!enemies[i].canSee)
                    continue;
                if (enemies[i].gameObject.TryGetComponent(out Health enemyHealth))
                {
                    //Skip vulnerable dead enemies. Need to target invulnerable for boss
                    if (!enemyHealth.IsInvulnerable() && enemyHealth.IsDead())
                        continue;
                }
                activeEnemies.Add(enemies[i]);
            }
        }
    }

    private void ChooseEnemy()
    {
        Vector2 playerPos = transform.position;
        //choose the closest one
        if (activeEnemies.Count > 0)
        {
            Enemy closestEnemy = activeEnemies[0];
            foreach (Enemy e in activeEnemies)
            {
                Vector2 enemyPos = e.gameObject.transform.position;
                if (Vector2.Distance(playerPos, enemyPos) < Vector2.Distance(playerPos, enemyPos))
                {
                    closestEnemy = e;
                }
            }
            
            // value = closestEnemy.gameObject.transform.position;
            target = closestEnemy.gameObject.transform.position;
            hasTarget = true;
            // IEnumerator coroutine = IgnoreEnemyFor(timeBetweenAttacks + 1, closestEnemy);
            // StartCoroutine(coroutine);
        }
        else
        {
            hasTarget = false;
            target = Vector3.zero;
        }
    }

    private void LungeAttack()
    {
        //attack
        // Debug.Log("Attacking now");

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

    public void OnDialogueEventStarted()
    {
        pauseStamina = true;
    }

    public void OnDialogueEventEnded()
    {
        pauseStamina = false;
    }
    
    public override void Deactivate()
    {
        base.Deactivate();
        sleepState = SleepState.Deactivated;
        rb.velocity = Vector2.zero;
    }

    public override void ExitState()
    {
        base.ExitState();
        Timing.KillCoroutines(aggroRoutine);
        Timing.KillCoroutines(attackRoutine);
        playerAttack.Deactivate();
        playerAttack.ResetAim();
        aiming.SetAimingState(Aiming.AimingState.Idle);
        animator.SetFloat(AnimationHelper.SpeedParameter, 0f);
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
