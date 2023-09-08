using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor()]
public class GameEvent<T> : ScriptableObject
{
    [TextArea(1,8)]
    public string description = string.Empty;
    
    private readonly List<GameEventListener<T>> listeners = new List<GameEventListener<T>>();

    public void Raise(T _value) 
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(_value);
    }

    public void Register(GameEventListener<T> _listener) 
    {
        listeners.Add(_listener);
    }

    public void UnRegister(GameEventListener<T> _listener) 
    {
        listeners.Remove(_listener);
    }
}

[CreateAssetMenu(menuName = "ScriptableObjects/GameEvent")]
[InlineEditor()]
[Serializable]
public class GameEvent : ScriptableObject
{
    [TextArea(1,8)]
    public string description = string.Empty;

    private readonly List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise() 
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised();
    }

    public void Register(GameEventListener _listener) 
    {
        listeners.Add(_listener);
    }

    public void UnRegister(GameEventListener _listener) 
    {
        listeners.Remove(_listener);
    }
}