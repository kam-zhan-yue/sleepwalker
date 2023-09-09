using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class GameObjectUnityEvent : UnityEvent<GameObject> { }

public class GameObjectEventListener : GameEventListener<GameObject>
{
    [SerializeField] private GameObjectUnityEvent gameObjectUnityEvent;
    [SerializeField] private GameObjectEvent gameObjectGameEvent;

    public override UnityEvent<GameObject> unityEvent => gameObjectUnityEvent;
    public override GameEvent<GameObject> gameEvent => gameObjectGameEvent;
}