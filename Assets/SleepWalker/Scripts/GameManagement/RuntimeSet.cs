using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor()]
public abstract class RuntimeSet<T> : ScriptableObject
{
    public List<T> items = new();
    public abstract GameEvent<T> OnItemAdded { get; }
    public abstract GameEvent<T> OnItemRemoved { get;  }

    public virtual void OnInit()
    {
        Clear();
    }

    public void Add(T _item)
    {
        if (!items.Contains(_item))
        {
            items.Add(_item);
            if (OnItemAdded != null)
                OnItemAdded.Raise(_item);
        }
    }

    public void Remove(T _item)
    {
        if (items.Contains(_item))
        {
            items.Remove(_item);
            if (OnItemRemoved != null)
                OnItemRemoved.Raise(_item);
        }
    }

    public void Clear()
    {
        int count = items.Count;
        for (int i = count - 1; i >= 0; --i)
        {
            T item = items[i];
            items.Remove(item);
        }
    }
}