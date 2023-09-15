using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

public class DamageBody : MonoBehaviour, IDamagePhysics
{
    public float knockbackTime = 1f;
    private Rigidbody2D rb;
    [NonSerialized, ShowInInspector, ReadOnly]
    private bool active = true;
    private Health health;

    private bool hasHealth = false;
    private CoroutineHandle countdownRoutine;
    private bool inKnockback = false;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        active = true;
        hasHealth = health != null;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Rigidbody2D GetRigidbody2D()
    {
        return rb;
    }

    public void Knockback()
    {
        inKnockback = true;
        Timing.KillCoroutines(countdownRoutine);
        countdownRoutine = Timing.RunCoroutine(HurtCountdown().CancelWith(gameObject));
    }

    private IEnumerator<float> HurtCountdown()
    {
        yield return Timing.WaitForSeconds(knockbackTime);
        rb.velocity = Vector2.zero;
        inKnockback = false;
        if(hasHealth && health.IsDead())
            Deactivate();
    }

    public bool InKnockback()
    {
        return inKnockback;
    }

    public void Activate()
    {
        active = true;
    }
    
    public void Deactivate()
    {
        active = false;
    }

    public bool IsActive()
    {
        return active;
    }
}
