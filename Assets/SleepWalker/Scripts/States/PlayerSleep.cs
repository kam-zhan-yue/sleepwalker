using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSleep : State
{
    public float sleepTime = 5f;
    private float sleepTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void EnterState()
    {
        base.EnterState();
        sleepTimer = sleepTime;
        spriteRenderer.color = Color.red;
        rb.velocity = Vector2.zero;
    }

    public override void UpdateBehaviour()
    {
        sleepTimer -= Time.deltaTime;
        if (sleepTimer <= 0f)
        {
            StateController.TryEnqueueState<PlayerAwake>();
        }
    }
}
