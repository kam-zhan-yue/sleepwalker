using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public Damage.Direction damageDirection = Damage.Direction.None;
    [BoxGroup("Setup Variables")] public Transform aimingTransform;
    [BoxGroup("Setup Variables")] public int activationFrame = 0;
    [BoxGroup("Setup Variables")] public int endFrame = 0;
    [BoxGroup("Setup Variables")] public int sampleRate = 0;
    [BoxGroup("Setup Variables")] public float activationTime = 0f;
    [BoxGroup("Setup Variables")] public LayerMask targetLayerMask;
    [BoxGroup("Setup Variables")] public Vector2 areaOffset;
    [BoxGroup("Setup Variables")] public Vector2 areaSize;
    [BoxGroup("Setup Variables")] public float damage = 0f;
    [BoxGroup("Animation Variables")] public bool usesAnimation = false;
    [ShowIf("usesAnimation"), BoxGroup("Animation Variables")]
    public Animator animator;
    [ShowIf("usesAnimation"), BoxGroup("Animation Variables")]
    public string animationToTrigger = String.Empty;

    //Damage Area Variables
    private GameObject damageArea;
    private BoxCollider2D damageAreaCollider;
    private DamageBox damageBox;
    private Aiming aiming;
    
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
        damageBox.direction = damageDirection;
    }

    public Aiming GetAiming()
    {
        return aiming;
    }

    private void Start()
    {
        DisableDamageArea();
    }

    public void Activate(Action _callback = null)
    {
        PlayAnimation();
        Timing.RunCoroutine(ActivateAttack(_callback));
        if(aiming != null)
            Timing.RunCoroutine(HandleAiming());
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

    private IEnumerator<float> HandleAiming()
    {
        aiming.Toggle(false);
        yield return Timing.WaitForSeconds(StaticHelper.GetFrameInSeconds(endFrame, sampleRate));
        aiming.Toggle(true);
    }

    public void Deactivate()
    {
        Timing.KillCoroutines();
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
