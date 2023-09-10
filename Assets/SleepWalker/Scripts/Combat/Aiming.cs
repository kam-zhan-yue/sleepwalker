using UnityEngine;

public class Aiming : MonoBehaviour
{
    public enum AimingState
    {
        Idle = 0,
        Aiming = 1,
        Firing = 2,
        Deactivated = 3
    }
    public Orientation orientation;
    public SpriteRenderer weapon;
    public AimingState aimingState;
    
    private bool hasOrientation = false;
    private AimingState initialState;

    private void Awake()
    {
        hasOrientation = orientation != null;
        initialState = aimingState;
    }

    protected virtual void Update()
    {
        if (aimingState == AimingState.Idle && hasOrientation)
        {
            weapon.flipX = !orientation.facingRight;
        }
        else
        {
            weapon.flipX = false;
        }
    }

    public void AimWeapon(Vector2 _direction)
    {
        if (aimingState == AimingState.Aiming)
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            weapon.flipY = angle <= -90f || angle > 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void SetAimingState(AimingState _state)
    {
        aimingState = _state;
    }

    public void ResetAim()
    {
        weapon.flipY = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void ResetState()
    {
        aimingState = initialState;
    }

    public void Deactivate()
    {
        aimingState = AimingState.Deactivated;
    }

    public void EditorFlip()
    {
        weapon.flipX = !orientation.facingRight;
    }
}
