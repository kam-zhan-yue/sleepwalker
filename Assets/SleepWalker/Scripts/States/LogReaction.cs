using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogReaction : Reaction
{
    public string log;
    public override void React()
    {
        Debug.Log(log);
    }
}
