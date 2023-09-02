using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HurtReaction : Reaction
{
    [BoxGroup("Setup Variables")] public StateController stateController;


    public override void React()
    {
        stateController.TryEnqueueState<Hurt>();
    }
}
