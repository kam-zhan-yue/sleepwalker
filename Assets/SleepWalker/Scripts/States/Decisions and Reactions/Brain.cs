using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Brain : MonoBehaviour
{
    [NonSerialized, ShowInInspector, ReadOnly]
    public Transform target;

    [NonSerialized, ShowInInspector, ReadOnly]
    private bool active = true;

    public bool IsActive()
    {
        return active;
    }

    public void Activate()
    {
        active = true;
    }
    
    public void Deactivate()
    {
        active = false;
    }

    public void FindPlayer()
    {
        
    }
}
