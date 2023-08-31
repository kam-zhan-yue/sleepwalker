using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyAggro : State
{
    [ShowInInspector, NonSerialized, ReadOnly]
    public Transform target;

    public override void EnterState()
    {
        base.EnterState();
    }
}
