using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class TransformUnityEvent : UnityEvent<Transform> { }

public class TransformGameEventListener : GameEventListener<Transform>
{
    [SerializeField] private TransformUnityEvent transformUnityEvent;
    [SerializeField] private TransformGameEvent transformGameEvent;

    public override UnityEvent<Transform> unityEvent => transformUnityEvent;
    public override GameEvent<Transform> gameEvent => transformGameEvent;
}