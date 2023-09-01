using UnityEngine;

public class Aiming : MonoBehaviour
{
    public SpriteRenderer weapon;
    public bool active = false;

    protected void AimWeapon(Vector2 _direction)
    {
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        weapon.flipY = angle <= -90f || angle > 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Toggle(bool _active)
    {
        active = _active;
    }
}
