using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public float damage;
    
    private IDamageSource source;
    private IDamageTarget target;
    
    public Damage(IDamageSource _source, IDamageTarget _target)
    {
        source = _source;
        target = _target;
        damage = 0f;
    }

    /// <summary>
    /// Damages the target based on the information that it is instantiated with
    /// </summary>
    public void DamageTarget()
    {
        source.ModifyDamage(this);
        target.TakeDamage(this);
    }

    public override string ToString()
    {
        string damageName = string.Empty;
        if (source != null && target != null)
            return $"'{target.GetId()}' was hit by '{source.GetId()}' with '{damageName}', dealing {damage}";

        if (target != null)
            return $"Target: {target.GetId()} | Damage: {damage}";
        
        if(source != null)
            return $"Source: {source.GetId()} | Damage: {damage}";

        return $"Damage: {damage}";
    }
}
