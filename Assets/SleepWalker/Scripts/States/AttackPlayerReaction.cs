using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerReaction : Reaction
{
    public StateController stateController;
    public EnemyAggro enemyAggro;
    
    public override void React()
    {
        enemyAggro.target = brain.target;
        stateController.TryEnqueueState<EnemyAggro>();
    }
}
