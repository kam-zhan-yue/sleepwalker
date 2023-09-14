using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Sirenix.OdinInspector;
using UnityEngine;

public class DamageBox : MonoBehaviour, IDamageSource
{
    [BoxGroup("Setup Variables")] public LayerMask targetLayerMask;
    
    [NonSerialized, ShowInInspector, ReadOnly]
    public float damage = 0f;

    [NonSerialized, ShowInInspector, ReadOnly]
    public float knockbackForce = 0f;

    [NonSerialized, ShowInInspector, ReadOnly]
    public KnockbackDirection knockbackDirection = KnockbackDirection.None;

    [NonSerialized, ShowInInspector, ReadOnly]
    public GameObject owner;
    
    [NonSerialized, ShowInInspector, ReadOnly]
    public BoxCollider2D boxCollider2D;
    
    private readonly RaycastHit2D[] hits = new RaycastHit2D[10];

    private readonly LayerMask obstacleLayerMask = LayerHelper.obstaclesLayerMask;

    public string GetId()
    {
        return name;
    }

    public void ModifyDamage(Damage _damage)
    {
        _damage.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        // Debug.Log(_collider.name + " Entered Trigger");
        
        if (!CanDamage(_collider.gameObject))
            return;
        if (HasObstacle(_collider.gameObject.transform))
        {
            Debug.Log("Obstacle!");
            return;
        }
        
        Debug.Log($"HITTT {name}");
        if (_collider.gameObject.TryGetComponent(out IDamageTarget target))
        {
            Debug.Log($"Found {target}");
            Damage damageObject = new(this, target);
            damageObject.DamageTarget();
            // Debug.Log(damageObject.ToString());
        }

        if (_collider.gameObject.TryGetComponent(out IDamagePhysics physics))
        {
            if (physics.IsActive())
            {
                Knockback knockback = new (knockbackForce, knockbackDirection, transform, physics);
                knockback.KnockbackTarget();
            }
        }
    }

    private bool HasObstacle(Transform _target)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = _target.position;
        int hitCount = Physics2D.RaycastNonAlloc(startPosition, endPosition - startPosition, hits, Vector2.Distance(startPosition, endPosition), obstacleLayerMask);
        return hitCount != 0;
    }
    
    private bool CanDamage(GameObject _gameObject)
    {
        return _gameObject.BelongsToLayerMask(targetLayerMask);
    }
}
