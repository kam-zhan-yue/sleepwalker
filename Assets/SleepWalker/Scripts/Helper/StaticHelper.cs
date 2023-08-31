using System;
using System.Collections.Generic;
using UnityEngine;

public static class StaticHelper
{
    private const float FPS = 24;
    
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

    public static float GetFrameInSeconds(int _frame)
    {
        float frameSecond = 1 / FPS;
        return _frame * frameSecond;
    }
    
    public static float GetFrameInSeconds(int _frame, float _sampleRate)
    {
        if (_sampleRate == 0)
            return 0;
        float frameSecond = 1 / _sampleRate;
        return _frame * frameSecond;
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
    
    /// <summary>
    /// Draws a debug ray in 2D and does the actual raycast
    /// </summary>
    /// <returns>The raycast hit.</returns>
    /// <param name="rayOriginPoint">Ray origin point.</param>
    /// <param name="rayDirection">Ray direction.</param>
    /// <param name="rayDistance">Ray distance.</param>
    /// <param name="mask">Mask.</param>
    /// <param name="debug">If set to <c>true</c> debug.</param>
    /// <param name="color">Color.</param>
    public static RaycastHit2D RayCast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color,bool drawGizmo=false)
    {	
        if (drawGizmo) 
        {
            Debug.DrawRay (rayOriginPoint, rayDirection * rayDistance, color);
        }
        return Physics2D.Raycast(rayOriginPoint,rayDirection,rayDistance,mask);		
    }
}
