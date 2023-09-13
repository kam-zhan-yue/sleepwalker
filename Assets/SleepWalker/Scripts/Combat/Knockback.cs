using UnityEngine;

public class Knockback
{
    private float force;
    private Vector3 customDirection;
    private KnockbackDirection direction;
    private Transform source;
    private IDamagePhysics target;

    public Knockback(float _force, KnockbackDirection _direction, Transform _source, IDamagePhysics _target)
    {
        force = _force;
        direction = _direction;
        source = _source;
        target = _target;
    }

    public void SetCustomDirection(Vector3 _direction)
    {
        customDirection = _direction;
    }

    public void KnockbackTarget()
    {
        Vector3 finalDirection = Vector3.zero;
        switch (direction)
        {
            case KnockbackDirection.BasedOnOwnerPosition:
                Transform targetTransform = target.GetTransform();
                finalDirection = source.DirectionToObject(targetTransform);
                break;
            case KnockbackDirection.Custom:
                finalDirection = customDirection;
                break;
        }

        Rigidbody2D targetRigidbody = target.GetRigidbody2D();
        targetRigidbody.velocity = Vector2.zero;
        targetRigidbody.AddForce(force * finalDirection, ForceMode2D.Impulse);
        target.Knockback();
        // Debug.Log("Add Knockback: "+force * finalDirection);
    }
}