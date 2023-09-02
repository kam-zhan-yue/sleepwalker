using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : Hurt
{
    //for now, the player cannot be interrupted during their sleep
    public override bool CanEnterState(State _currentState)
    {
        return !StateController.IsCurrentState<PlayerSleep>();
    }
}
