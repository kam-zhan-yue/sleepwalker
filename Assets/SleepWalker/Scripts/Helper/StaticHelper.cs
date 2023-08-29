using System;
using System.Collections.Generic;
using UnityEngine;

public static class StaticHelper
{
    public static void AddOnce<T>(this HashSet<T> _list, T _item)
    {
        if (_item != null && !_list.Contains(_item))
            _list.Add(_item);
    }
    
    public static void AddOnce<T>(this List<T> _list, T _item)
    {
        if (_item != null && !_list.Contains(_item))
            _list.Add(_item);
    }
    
    public static void SetActiveFast(this GameObject _gameObject, bool _active)
    {
        if (_gameObject.activeSelf == _active)
            return;

        _gameObject.SetActive(_active);
    }

    public static T[] Copy<T>(this List<T> _list)
    {
        if (_list == null)
            return Array.Empty<T>();
        int count = _list.Count;
        T[] array = new T[count];
        for (int i = 0; i < count; ++i)
            array[i] = _list[i];
        return array;
    }

    public static bool BelongsToLayerMask(int _layer, int _layerMask)
    {
        return (_layerMask & (1 << _layer)) > 0;
    }
}
