using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public enum CameraState
    {
        None = 0,
        TrackingPlayer = 1,
        Transition = 2,
    }
    
    public static CameraManager instance;

    [BoxGroup("Setup Variables")] public float transitionDuration = 0.5f;
    [BoxGroup("Setup Variables")] public Ease easing = Ease.OutQuart;

    [NonSerialized, ShowInInspector, ReadOnly] 
    private Vector3 offset = Vector3.zero; //for z offset and offset to follow mouse pointer
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private float damp = 0.3f;
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private float dirOffset = 1.5f;
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private CameraState state = CameraState.None;

    [NonSerialized, ShowInInspector, ReadOnly] 
    private Transform target;
    
    private Camera mainCamera;

    private Tween transitionTween;

    private void Awake()
    {
        if (instance && instance != this)
            Destroy(gameObject);
        else
            instance = this;
        mainCamera = Camera.main;

        //target = GameObject.FindGameObjectWithTag("Player").transform;
        //state = CameraState.TrackingPlayer;

        OnPlayerAdded(GameObject.FindGameObjectWithTag("Player"));
    }

    private void Update()
    {
        switch (state)
        {
            case CameraState.None:
                break;
            case CameraState.TrackingPlayer:
                //offset ensure player is in front of camera, and that camera follows pointer slightly
                offset = (dirOffset*GetMouseDirection(target.transform.position));
                //offset = Vector3.zero; //take this out when you fix the other offset
                offset.z = -10f;

                //transform.position = Vector3.Lerp(transform.position, target.position, damp) + offset;
                transform.position = Vector3.Lerp(transform.position, target.position + offset, damp);
                    break;
            case CameraState.Transition:
                break;
        }
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        mouseScreenPosition.z = 0;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        return mouseWorldPosition;
    }

    public void OnPlayerAdded(GameObject _player)
    {
        target = _player.transform;
        state = CameraState.TrackingPlayer;
    }

    public void OnPlayerRemoved(GameObject _player)
    {
        state = CameraState.None;
    }

    public void OnDialogueEventStarted(Transform _transform)
    {
        state = CameraState.Transition;
        Vector3 position = _transform.position;
        position.z = transform.position.z;
        transitionTween = transform.DOMove(position, transitionDuration)
            .SetEase(easing);
    }

    public void OnDialogueEventEnded(Transform _transform)
    {
        Vector3 position = target.position;
        position.z = transform.position.z;
        transitionTween = transform.DOMove(position, transitionDuration)
            .SetEase(easing)
            .OnComplete(() =>
            {
                state = CameraState.TrackingPlayer;
            });;
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

    private void OnDestroy()
    {
        instance = null;
    }
}
