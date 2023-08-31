using UnityEngine;

public class Aiming : MonoBehaviour
{
    public SpriteRenderer weapon;
    protected Transform weaponTransform;
    public bool active = false;

    private void Awake()
    {
        weaponTransform = weapon.transform;
    }

    protected void AimWeapon(Vector2 _direction)
    {
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        if (angle > -90f && angle <= 90f)
            weapon.flipY = false;
        else
            weapon.flipY = true;
        weaponTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Toggle(bool _active)
    {
        active = _active;
    }
}
