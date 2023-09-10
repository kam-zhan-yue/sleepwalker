using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitDecision : Decision
{
    //Set Time Between Decisions in Editor
    protected override bool CanActivate()
    {
        return true;
    }
}
