using UnityEngine;

public interface IDamagePhysics
{
    public Transform GetTransform();
    public Rigidbody2D GetRigidbody2D();
    public void Deactivate();
    public bool IsActive();
}