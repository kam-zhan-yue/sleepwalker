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

    public static bool BelongsToLayerMask(this GameObject _gameObject, LayerMask _layerMask)
    {
        return ((1 << _gameObject.layer) & _layerMask) != 0;
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
    

    /// <summary>
    /// Draws a gizmo rectangle
    /// </summary>
    /// <param name="_center">Center.</param>
    /// <param name="_size">Size.</param>
    /// <param name="_color">Color.</param>
    public static void DrawGizmoRectangle(Vector2 _center, Vector2 _size, Color _color)
    {
        Gizmos.color = _color;

        Vector3 v3TopLeft = new(_center.x - _size.x/2, _center.y + _size.y/2, 0);
        Vector3 v3TopRight = new(_center.x + _size.x/2, _center.y + _size.y/2, 0);;
        Vector3 v3BottomRight = new(_center.x + _size.x/2, _center.y - _size.y/2, 0);;
        Vector3 v3BottomLeft = new(_center.x - _size.x/2, _center.y - _size.y/2, 0);;

        Gizmos.DrawLine(v3TopLeft,v3TopRight);
        Gizmos.DrawLine(v3TopRight,v3BottomRight);
        Gizmos.DrawLine(v3BottomRight,v3BottomLeft);
        Gizmos.DrawLine(v3BottomLeft,v3TopLeft);
    }
}
