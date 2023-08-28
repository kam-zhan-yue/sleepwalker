using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f; //maybe derive from scriptable object with speeds for awake and asleep modes
    private Rigidbody2D rb;
    private float vert = 0f;
    private float horiz = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update for input
    void Update()
    {
        vert = Input.GetAxis("Vertical");
        horiz = Input.GetAxis("Horizontal");
    }

    //fixed update for physics
    private void FixedUpdate()
    {
        //update velocity directly for snappy controls
        rb.velocity = new Vector3(horiz * speed, vert * speed);
    }
}
