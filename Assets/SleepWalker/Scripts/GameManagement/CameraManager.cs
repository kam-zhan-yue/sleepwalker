using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private Vector3 offset = Vector3.zero; //for z offset and offset to follow mouse pointer
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private float damp = 0.3f;
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private float dirOffset = 1.5f;
    
    private Transform target;
    
    private Camera mainCamera;

    private void Awake()
    {
        if (instance && instance != this)
            Destroy(gameObject);
        else
            instance = this;
        mainCamera = Camera.main;
    }

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        //offset ensure player is in front of camera, and that camera follows pointer slightly
        offset = (dirOffset*GetMouseDirection(target.transform.position));
        //offset = Vector3.zero; //take this out when you fix the other offset
        offset.z = -10f;

        //transform.position = Vector3.Lerp(transform.position, target.position, damp) + offset;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, damp);
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        mouseScreenPosition.z = 0;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        return mouseWorldPosition;
    }

    public Vector3 GetMouseDirection(Vector3 _targetPosition)
    {
        //get mouse pointer position
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        mouseScreenPosition.z = 0;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        //find direction from current player to mouse
        Vector3 mouseDirection = (mouseWorldPosition - _targetPosition).normalized;

        return mouseDirection;
    }
}
