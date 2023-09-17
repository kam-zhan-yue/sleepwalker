using UnityEngine;

public class Orientation : MonoBehaviour
{
    public enum FacingMode
    {
        Automatic = 0,
        Movement = 1,
        Cursor = 2,
        Aiming = 3,
        Deactivated = 4
    }

    public FacingMode facingMode = FacingMode.Automatic;

    public bool facingRight;

    public SpriteRenderer spriteRenderer;
    private Vector3 positionLastFrame;
    private Vector3 positionDifference;
    private Transform target;
    private Vector3 targetPosition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        positionLastFrame = transform.position;
        FlipModel(facingRight ? 1 : -1);
    }

    private void Update()
    {
        switch (facingMode)
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
            case FacingMode.Aiming:
                ProcessAiming();
                break;
            case FacingMode.Deactivated:
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
        if (mouseWorldPosition.x >= xPos)
            FlipModel(1);
        else
        {
            FlipModel(-1);
        }
    }

    public void SetAimTarget(Transform _target)
    {
        target = _target;
    }

    public void SetAimTargetPosition(Vector3 _position)
    {
        targetPosition = _position;
    }

    private void ProcessAiming()
    {
        if (target != null)
        {
            Vector3 direction = transform.DirectionToObject(target);
            facingRight = direction.x >= 0f;
            FlipModel(facingRight ? 1 : -1);
        }
        else
        {
            Vector3 direction = transform.DirectionToPoint(targetPosition);
            facingRight = direction.x >= 0f;
            FlipModel(facingRight ? 1 : -1);
        }
    }

    public void SetFacingMode(FacingMode _mode)
    {
        facingMode = _mode;
    }

    private void FlipModel(int _direction)
    {
        spriteRenderer.flipX = (_direction == -1);
    }

    public void Deactivate()
    {
        facingMode = FacingMode.Deactivated;
    }

    public void EditorFlip()
    {
        facingRight = !facingRight;
        FlipModel(facingRight ? 1 : -1);
    }
}
