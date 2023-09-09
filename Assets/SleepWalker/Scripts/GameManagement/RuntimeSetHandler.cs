using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeSetHandler<T> : MonoBehaviour
{
    public RuntimeSet<T>[] runtimeSets = Array.Empty<RuntimeSet<T>>();
    
    private void Awake()
    {
        for (int i = 0; i < runtimeSets.Length; ++i)
            runtimeSets[i].OnInit();
    }
}