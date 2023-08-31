using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour, IDamageSource
{
    public LayerMask targetLayerMask;
    public GameObject owner;
    public float damage = 0f;
    public BoxCollider2D boxCollider2D;
    public Damage.Direction direction = Damage.Direction.None;
    
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
        Debug.Log(_collider.name + " Entered Trigger");
        
        if (!CanDamage(_collider.gameObject))
            return;
        
        if (_collider.gameObject.TryGetComponent(out IDamageTarget target))
        {
            Debug.Log(target.GetId() + " Found");
            Damage damageObject = new(this, target);
            damageObject.DamageTarget();
            Debug.Log(damageObject.ToString());
        }
    }

    private bool CanDamage(GameObject _gameObject)
    {
        return _gameObject.BelongsToLayerMask(targetLayerMask);
    }
}
