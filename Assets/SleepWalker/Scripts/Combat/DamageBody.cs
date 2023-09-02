using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBody : MonoBehaviour, IDamagePhysics
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Rigidbody2D GetRigidbody2D()
    {
        return rb;
    }
}
