using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Attack : MonoBehaviour, IDamageSource
{
    [BoxGroup("Setup Variables")] 
    public float initialDelay = 0f;

    public Vector2 areaOffset;
    public Vector2 areaSize;
    public float damage = 0f;
    private GameObject damageArea;
    
    public string GetId()
    {
        return gameObject.name;
    }

    public void ModifyDamage(Damage _damage)
    {
        _damage.damage = damage;
    }

    private void OnDrawGizmos()
    {
        StaticHelper.DrawGizmoRectangle(transform.position + (Vector3)areaOffset, areaSize, Color.red);
    }
}
