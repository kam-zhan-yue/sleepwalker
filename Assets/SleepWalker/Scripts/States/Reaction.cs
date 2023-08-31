using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reaction : MonoBehaviour
{
    protected Brain brain;
    
    public abstract void React();

    protected virtual void Awake()
    {
        brain = GetComponent<Brain>();
    }
}
