using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : State
{
    private Rigidbody2D rb;
    private Orientation orientation;

    private ParticleSystem zzzParticles;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        orientation = GetComponent<Orientation>();

        //find particle system
        for (int i = 0; i < transform.childCount; i++)
        {
            ParticleSystem inChild = transform.GetChild(i).GetComponent<ParticleSystem>();
            if (inChild != null)
            {
                zzzParticles = inChild;
            }
        }
    }
    
    public override void EnterState()
    {
        base.EnterState();
        rb.velocity = Vector2.zero;
        orientation.SetFacingMode(Orientation.FacingMode.Automatic);

        zzzParticles.Stop();
    }
}
