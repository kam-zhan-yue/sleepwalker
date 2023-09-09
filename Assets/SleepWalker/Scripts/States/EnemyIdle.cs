using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : State
{
    private Rigidbody2D rb;
    private Orientation orientation;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        orientation = GetComponent<Orientation>();
    }
    
    public override void EnterState()
    {
        base.EnterState();
        rb.velocity = Vector2.zero;
        orientation.SetFacingMode(Orientation.FacingMode.Automatic);
    }
}
