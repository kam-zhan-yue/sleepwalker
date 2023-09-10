using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolReaction : Reaction
{
    public StateController stateController;
    public override void React()
    {
        stateController.TryEnqueueState<EnemyPatrol>();
    }
}
