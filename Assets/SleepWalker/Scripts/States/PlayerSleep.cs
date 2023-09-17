using System;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSleep : State
{
    private enum SleepState
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
    [BoxGroup("Setup Variables")] public FloatReference maxStamina;
    [BoxGroup("Setup Variables")] public FloatReference staminaIncreaseRate;
    [BoxGroup("Setup Variables")] public FloatReference staminaTime;
    [BoxGroup("Setup Variables")] public BoolReference playerSleep;
    [BoxGroup("Setup Variables")] public FloatReference volume;

    [SerializeField] [BoxGroup("Setup Variables")]
    private FloatReference stamina;

    [BoxGroup("Setup Variables")] public GameObjectRuntimeSet bossRuntimeSet;
    [BoxGroup("Setup Variables")] public GameObjectRuntimeSet enemyRuntimeSet;
    [BoxGroup("Setup Variables")] public Attack playerAttack;
    [BoxGroup("Setup Variables")] public float stopMoveDistance;
    [BoxGroup("Setup Variables")] public float attackDistance;

    [SerializeField] float maxDistance = 2f;

    private List<Enemy> enemies = new();

    private List<Enemy> activeEnemies = new();
    private Rigidbody2D rb;
    private Aiming aiming;
    private Orientation orientation;
    private DamageBody damageBody;
    private Animator animator;
    private readonly RaycastHit2D[] hits = new RaycastHit2D[10];
    private readonly LayerMask obstacleLayerMask = LayerHelper.obstaclesLayerMask;

    [NonSerialized, ShowInInspector, ReadOnly]
    private SleepState sleepState;

    private CoroutineHandle aggroRoutine;
    private CoroutineHandle attackRoutine;
    private bool pauseStamina = false;
    private Vector3 target = Vector3.zero;
    private bool hasTarget;

    private PlayerControls playerControls;
    
    private Vector3 debugEnemyPosition = Vector3.zero;

    [SerializeField, ReadOnly] private ParticleSystem zzzParticles;

    [SerializeField, ReadOnly] private AudioSource footstepAudio;

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
        playerControls = new PlayerControls();
        playerControls.PlayerInput.Enable();
        playerControls.PlayerInput.Sleep.started += SleepStarted;

        //find what child has the zs particles and the footstep audio
        for (int i = 0; i < transform.childCount; i++)
        {
            ParticleSystem inChild = transform.GetChild(i).GetComponent<ParticleSystem>();
            if (inChild != null)
            {
                zzzParticles = inChild;
            }

            AudioSource source = transform.GetChild(i).GetComponent<AudioSource>();
            if (source != null)
            {
                footstepAudio = source;
            }
        }
    }

    public override void EnterState()
    {
        base.EnterState();

        //Setup Variables
        playerSleep.Value = true;
        playerAttack.ReInit();
        aiming = playerAttack.aiming;
        aiming.SetAimingState(Aiming.AimingState.Aiming);

        aggroRoutine = Timing.RunCoroutine(AggroRoutine());
        attackRoutine = Timing.RunCoroutine(AttackRoutine());

        zzzParticles.Play();
    }

    public override void UpdateBehaviour()
    {
        if (sleepState == SleepState.Deactivated)
            return;
        
        if (!pauseStamina)
            stamina.Value += staminaIncreaseRate * Time.deltaTime;

        // stateText.text = $"Sleep State: {aiState}";

        if (stamina >= maxStamina)
        {
            StateController.TryEnqueueState<PlayerAwake>();
        }

        if (!animator.GetBool(AnimationHelper.SleepParameter))
        {
            animator.SetBool(AnimationHelper.SleepParameter, true);
        }
    }

    public void ForceAwake()
    {
        StateController.TryEnqueueState<PlayerAwake>();
    }

    private void SleepStarted(InputAction.CallbackContext _callbackContext)
    {
        //If not sleeping, don't bother
        if (!playerSleep.Value)
            return;
        StateController.TryEnqueueState<PlayerAwake>();
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
            playerAttack.Activate(() => { attackOver = true; });
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
            footstepAudio.Pause();
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

            if (!footstepAudio.isPlaying)
            {
                //play footsteps sfx
                footstepAudio.Play();
                footstepAudio.volume = volume.Value;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetFloat(AnimationHelper.SpeedParameter, 0f);
            footstepAudio.Pause();
        }
    }

    private void LookForEnemies()
    {
        //check for enemies within a range and that aren't obstructed by a wall
        activeEnemies.Clear();
        enemies = new List<Enemy>();

        for (int i = 0; i < bossRuntimeSet.items.Count; ++i)
        {
            Enemy enemy = new()
            {
                gameObject = bossRuntimeSet.items[i],
                canSee = true
            };
            enemies.Add(enemy);
        }

        for (int i = 0; i < enemyRuntimeSet.items.Count; ++i)
        {
            Enemy enemy = new()
            {
                gameObject = enemyRuntimeSet.items[i],
                canSee = true
            };
            enemies.Add(enemy);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            Vector3 endPosition = enemies[i].gameObject.transform.position;
            Vector3 direction = transform.DirectionToPoint(endPosition);
            //Skip if the enemy is too far
            float distance = transform.DistanceToPoint(endPosition);
            if (distance > maxDistance)
                continue;
            debugEnemyPosition = endPosition;

            int hitCount = Physics2D.RaycastNonAlloc(transform.position, direction, hits, distance,
                obstacleLayerMask);
            //Skip if there are obstacles (hitCount > 0)
            if (hitCount == 0)
            {
                if (!enemies[i].canSee)
                    continue;
                if (enemies[i].gameObject.TryGetComponent(out Health enemyHealth))
                {
                    switch (enemyHealth.targetState)
                    {
                        //If invisible, forget about it
                        case Health.TargetState.Invisible:
                            continue;
                        //If always target, just set and forget
                        case Health.TargetState.Always:
                            activeEnemies.Add(enemies[i]);
                            continue;
                        //If alive target but dead, ignore
                        case Health.TargetState.Alive when enemyHealth.IsDead():
                            continue;
                    }
                }

                activeEnemies.Add(enemies[i]);
            }
        }
    }

    private void ChooseEnemy()
    {
        //choose the closest one
        if (activeEnemies.Count > 0)
        {
            int targetIndex = 0;
            float minDistance = transform.DistanceToObject(activeEnemies[targetIndex].gameObject.transform);
            for (int i = 1; i < activeEnemies.Count; ++i)
            {
                float distance = transform.DistanceToObject(activeEnemies[i].gameObject.transform);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetIndex = i;
                }
            }

            target = activeEnemies[targetIndex].gameObject.transform.position;
            hasTarget = true;
        }
        else
        {
            hasTarget = false;
            target = Vector3.zero;
        }
    }

    //Disable because in boss battle, it's good to wait until awake to face the next wave
    public void OnDialogueEventStarted()
    {
        // pauseStamina = true;
    }

    public void OnDialogueEventEnded()
    {
        // pauseStamina = false;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        zzzParticles.Stop();
        playerAttack.Deactivate();
        sleepState = SleepState.Deactivated;
        rb.velocity = Vector2.zero;
    }

    public override void ExitState()
    {
        base.ExitState();
        zzzParticles.Stop();
        Timing.KillCoroutines(aggroRoutine);
        Timing.KillCoroutines(attackRoutine);
        // playerAttack.Deactivate();
        playerAttack.ResetAim();
        aiming.SetAimingState(Aiming.AimingState.Idle);
        animator.SetFloat(AnimationHelper.SpeedParameter, 0f);
        footstepAudio.Pause();
    }

    private void OnDrawGizmos()
    {
        Transform transform1;
        Vector3 direction = (transform1 = transform).DirectionToPoint(debugEnemyPosition);
        Debug.DrawRay(transform1.position, direction, Color.green);
    }

    private void OnDestroy()
    {
        Timing.KillCoroutines(aggroRoutine);
        Timing.KillCoroutines(attackRoutine);
        playerControls.Dispose();
    }
}