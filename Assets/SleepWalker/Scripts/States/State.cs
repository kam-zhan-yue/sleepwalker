using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public const string INIT = "INIT";
    protected const string EXIT = "EXIT";

    protected StateController StateController { get; set; }
    protected string ID { get; set; } = string.Empty;
    
    [NonSerialized, ShowInInspector, ReadOnly]
    public bool active = false;

    [NonSerialized, ShowInInspector, ReadOnly]
    protected float elapsedTime = 0f;

    protected virtual void Awake()
    {
        bool validState = TryGetComponent(out StateController controller);
        if (!validState)
        {
            Debug.LogError($"{this.gameObject.name} has a missing state controller!");
            gameObject.SetActiveFast(false);
            return;
        }
        StateController = controller;
        ID = GetType().Name;
    }

    public virtual bool CanEnterState(State _currentState)
    {
        return true;
    }
    
    public virtual void EnterState()
    {
        active = true;
        elapsedTime = 0f;
    }

    public virtual void PreUpdateBehaviour()
    {
        
    }

    public virtual void UpdateBehaviour()
    {
        elapsedTime += Time.deltaTime;
    }

    public virtual void PostUpdateBehaviour()
    {
    }

    public virtual void FixedUpdateBehaviour()
    {
        
    }

    public virtual void CheckExitState()
    {
    }

    public virtual void ExitState()
    {
        active = false;
    }

    public virtual void Deactivate()
    {
        active = false;
    }
}