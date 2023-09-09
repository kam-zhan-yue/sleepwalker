using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBody : MonoBehaviour, IDamagePhysics
{
    private Rigidbody2D rb;
    private bool active = true;

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

    public void Deactivate()
    {
        active = false;
    }

    public bool IsActive()
    {
        return active;
    }
}
