using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class DamageBody : MonoBehaviour, IDamagePhysics
{
    public float knockbackTime = 1f;
    private Rigidbody2D rb;
    private bool active = true;
    
    private CoroutineHandle countdownRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        active = true;
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
        Timing.KillCoroutines(countdownRoutine);
        countdownRoutine = Timing.RunCoroutine(HurtCountdown());
    }

    private IEnumerator<float> HurtCountdown()
    {
        yield return Timing.WaitForSeconds(knockbackTime);
        rb.velocity = Vector2.zero;
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
