using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwake : State
{
    [SerializeField] float speed = 10f; //maybe derive from scriptable object with speeds for awake and asleep modes
    private Rigidbody2D rb;
    private float vert = 0f;
    private float horiz = 0f;
    private Vector2 speedVector = new();
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void EnterState()
    {
        base.EnterState();
        spriteRenderer.color = Color.white;
    }
    
    public override void UpdateBehaviour()
    {
        vert = Input.GetAxis("Vertical");
        horiz = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StateController.TryEnqueueState<PlayerSleep>();
        }
    }

    public override void FixedUpdateBehaviour()
    {
        //update velocity directly for snappy controls
        speedVector.x = horiz * speed;
        speedVector.y = vert * speed;
        rb.velocity = speedVector;
    }
}
