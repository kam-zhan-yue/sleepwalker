using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public State defaultState;

    [NonSerialized, ShowInInspector, ReadOnly] 
    public State previousState;

    [NonSerialized, ShowInInspector, ReadOnly] 
    public State currentState;

    private Queue<State> transitionQueue = new Queue<State>();

    private List<State> stateList = new List<State>();
    
    private void Awake()
    {
        if (defaultState == null)
        {
            Debug.LogError($"{gameObject.name} has no default state!");
            gameObject.SetActiveFast(false);
            return;
        }

        State[] stateArray = GetComponents<State>();
        if (stateArray != null)
        {
            int count = stateArray.Length;
            for (int i = 0; i < count; ++i)
            {
                State state = stateArray[i];
                stateList.AddOnce(state);
            }
        }
    }

    private void Start()
    {
        currentState = defaultState;
        currentState.EnterState();
    }
    
    private void Update()
    {
        currentState.PreUpdateBehaviour();
        currentState.UpdateBehaviour();
        currentState.PostUpdateBehaviour();
        currentState.CheckExitState();
        ProcessTransitions();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateBehaviour();
    }
    
    private bool TryGetState<T>(out State _state) where T : State
    {
        int count = stateList.Count;
        for (int i = 0; i < count; ++i)
        {
            State state = stateList[i];
            Type stateType = state.GetType();
            Type type = typeof(T);
            if (stateType == type)
            {
                _state = state;
                return true;
            }
        }

        _state = null;
        return false;
    }

    private void ProcessTransitions()
    {
        while (transitionQueue.Count > 0)
        {
            State state = transitionQueue.Dequeue();
            TryEnterState(state);
        }
    }

    public bool TryEnqueueState<T>() where T : State
    {
        bool enqueueSuccess = TryGetState<T>(out State state);
        if (enqueueSuccess)
            transitionQueue.Enqueue(state);
        return enqueueSuccess;
    }

    public bool TryEnterState(State _state)
    {
        bool canEnterState = _state.CanEnterState(currentState);
        if (canEnterState)
        {
            EnterState(_state);
        }
        return canEnterState;
    }

    public void EnterDefaultState()
    {
        EnterState(defaultState);
    }

    private void EnterState(State _state)
    {
        currentState.ExitState();
        _state.EnterState();
        previousState = currentState;
        currentState = _state;
    }
}
