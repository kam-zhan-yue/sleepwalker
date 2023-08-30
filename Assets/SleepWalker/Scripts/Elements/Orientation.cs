using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro.EditorUtilities;
using UnityEngine;

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
    private Camera mainCamera;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
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
        
    }

    private void ProcessMovement()
    {
        if (facingRight)
        {
            FlipModel(1);
        }
        else
        {
            FlipModel(-1);
        }
    }

    private void ProcessCursor()
    {
        float xPos = transform.position.x;
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 0;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
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
