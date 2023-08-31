using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class Decision : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public Reaction reaction;
    [BoxGroup("Setup Variables")] public bool disableAfterActivation = false;
    [BoxGroup("Setup Variables")] public float timeBetweenDecisions = 1f;

    [NonSerialized, ShowInInspector, ReadOnly]
    public bool active = true;

    protected Brain brain;
    private float decisionTimer = 0f;

    protected virtual void Awake()
    {
        decisionTimer = timeBetweenDecisions;
        brain = GetComponent<Brain>();
    }
    
    protected virtual void Update()
    {
        if (!active)
            return;
        decisionTimer -= Time.deltaTime;
        if (decisionTimer <= 0f)
        {
            if (CanActivate())
                Activate();
            decisionTimer = timeBetweenDecisions;
        }
    }

    protected abstract bool CanActivate();

    private void Activate()
    {
        reaction.React();
        if(disableAfterActivation)
            active = false;
    }

    public void ToggleActive(bool _active)
    {
        active = _active;
    }
}
