using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class Orientation : MonoBehaviour
{
    public enum FacingMode
    {
        Automatic = 0,
        Movement = 1,
        Cursor = 2,
        Both = 3
    }
    
    public FacingMode facingMode = FacingMode.Automatic;

    public bool facingRight;

    private SpriteRenderer spriteRenderer;
    private Vector3 positionLastFrame;
    private Vector3 positionDifference;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        positionLastFrame = transform.position;
        FlipModel(facingRight ? 1 : -1);
    }

    private void Update()
    {
        switch(facingMode)
        {
            case FacingMode.Automatic:
                ProcessAutomatic();
                break;
            case FacingMode.Movement:
                ProcessMovement();
                break;
            case FacingMode.Cursor:
                ProcessCursor();
                break;
            case FacingMode.Both:
                break;
            default:
                ProcessMovement();
                break;
        }
    }

    private void ProcessAutomatic()
    {
        positionDifference = (transform.position - positionLastFrame);
        if (positionDifference.x > 0f)
        {
            facingRight = true;
            FlipModel(1);
        }
        else if (positionDifference.x < 0f)
        {
            facingRight = false;
            FlipModel(-1);
        }
        positionLastFrame = transform.position;
    }

    private void ProcessMovement()
    {
        FlipModel(facingRight ? 1 : -1);
    }

    private void ProcessCursor()
    {
        float xPos = transform.position.x;
        Vector3 mouseWorldPosition = CameraManager.instance.GetMousePosition();
        if(mouseWorldPosition.x >= xPos)
            FlipModel(1);
        else
        {
            FlipModel(-1);
        }
    }

    private void FlipModel(int _direction)
    {
        spriteRenderer.flipX = (_direction == -1);
    }
}
