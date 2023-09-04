using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public FloatReference damage;
    [BoxGroup("Setup Variables")] public float attackCooldown = 0f;
    [BoxGroup("Setup Variables")] public float knockbackForce = 0f;
    [BoxGroup("Setup Variables")] public KnockbackDirection knockbackDirection;
    [BoxGroup("Setup Variables")] public Transform aimingTransform;
    [BoxGroup("Setup Variables")] public int activationFrame = 0;
    [BoxGroup("Setup Variables")] public int endFrame = 0;
    [BoxGroup("Setup Variables")] public int sampleRate = 0;
    [BoxGroup("Setup Variables")] public float activationTime = 0f;
    [BoxGroup("Setup Variables")] public LayerMask targetLayerMask;
    [BoxGroup("Setup Variables")] public Vector2 areaOffset;
    [BoxGroup("Setup Variables")] public Vector2 areaSize;
    [BoxGroup("Animation Variables")] public bool usesAnimation = false;
    [ShowIf("usesAnimation"), BoxGroup("Animation Variables")]
    public Animator animator;
    [ShowIf("usesAnimation"), BoxGroup("Animation Variables")]
    public string animationToTrigger = String.Empty;

    //Damage Area Variables
    private GameObject damageArea;
    private BoxCollider2D damageAreaCollider;
    private DamageBox damageBox;
    
    [HideInInspector]
    public Aiming aiming;
    private bool cooldown = false;

    private CoroutineHandle attackRoutine;
    private CoroutineHandle aimingRoutine;
    private CoroutineHandle cooldownRoutine;
    
    private void Awake()
    {
        aiming = GetComponent<Aiming>();
        
        damageArea = new GameObject();
        damageArea.name = name + "DamageArea";
        Transform transform1 = transform;
        damageArea.transform.position = transform1.position;
        damageArea.transform.rotation = transform1.rotation;
        damageArea.transform.SetParent(aimingTransform == null ? transform1 : aimingTransform);
        damageArea.transform.localScale = Vector3.one;
        damageArea.layer = gameObject.layer;

        damageAreaCollider = damageArea.AddComponent<BoxCollider2D>();
        damageAreaCollider.offset = areaOffset;
        damageAreaCollider.size = areaSize;
        damageAreaCollider.isTrigger = true;
        
        Rigidbody2D rb = damageArea.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        damageBox = damageArea.AddComponent<DamageBox>();
        damageBox.targetLayerMask = targetLayerMask;
        damageBox.damage = damage;
        damageBox.owner = gameObject;
        damageBox.boxCollider2D = damageAreaCollider;
        damageBox.knockbackForce = knockbackForce;
        damageBox.knockbackDirection = knockbackDirection;
    }
    
    private void Start()
    {
        DisableDamageArea();
    }

    public bool InCooldown()
    {
        return cooldown;
    }

    public void Activate(Action _callback = null)
    {
        PlayAnimation();
        cooldownRoutine = Timing.RunCoroutine(CooldownCountdown());
        if (aiming)
        {
            attackRoutine = Timing.RunCoroutine(ActivateAttack());
            aimingRoutine = Timing.RunCoroutine(HandleAiming(_callback));
        }
        else
        {
            attackRoutine = Timing.RunCoroutine(ActivateAttack(_callback));
        }
    }
    
    private IEnumerator<float> ActivateAttack(Action _callback = null)
    {
        float initialDelay = StaticHelper.GetFrameInSeconds(activationFrame, sampleRate);
        yield return Timing.WaitForSeconds(initialDelay);
        EnableDamageArea();
        yield return Timing.WaitForSeconds(activationTime);
        DisableDamageArea();
        _callback?.Invoke();
    }

    private IEnumerator<float> HandleAiming(Action _callback = null)
    {
        aiming.SetAimingState(Aiming.AimingState.Firing);
        yield return Timing.WaitForSeconds(StaticHelper.GetFrameInSeconds(endFrame, sampleRate));
        aiming.SetAimingState(Aiming.AimingState.Aiming);
        _callback?.Invoke();
    }

    private IEnumerator<float> CooldownCountdown()
    {
        cooldown = true;
        yield return Timing.WaitForSeconds(attackCooldown);
        cooldown = false;
    }

    public void Deactivate()
    {
        cooldown = false;
        Timing.KillCoroutines(attackRoutine);
        Timing.KillCoroutines(cooldownRoutine);
        Timing.KillCoroutines(aimingRoutine);
        DisableDamageArea();
    }

    private void PlayAnimation()
    {
        if (usesAnimation && animator != null)
            animator.Play(animationToTrigger);
    }

    private void DisableDamageArea()
    {
        damageAreaCollider.enabled = false;
    }

    private void EnableDamageArea()
    {
        damageAreaCollider.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        StaticHelper.DrawGizmoRectangle(Vector3.zero + (Vector3)areaOffset, areaSize, Color.red);
    }

    private void OnDestroy()
    {
        Deactivate();
    }
}
