using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Brain : MonoBehaviour
{
    [NonSerialized, ShowInInspector, ReadOnly]
    public Transform target;

    private bool active = true;

    public bool IsActive()
    {
        return active;
    }
    public void Deactivate()
    {
        active = false;
    }
}
