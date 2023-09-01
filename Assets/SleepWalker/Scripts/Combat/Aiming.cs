using UnityEngine;

public class Aiming : MonoBehaviour
{
    public Orientation orientation;
    public SpriteRenderer weapon;
    public bool active = false;
    public bool idle = true;
    
    private bool hasOrientation = false;

    private void Awake()
    {
        hasOrientation = orientation != null;
    }

    protected virtual void Update()
    {
        if (idle && hasOrientation)
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
        if (!active)
            return;
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        weapon.flipY = angle <= -90f || angle > 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Toggle(bool _active)
    {
        active = _active;
    }

    public void ResetAim()
    {
        weapon.flipY = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
