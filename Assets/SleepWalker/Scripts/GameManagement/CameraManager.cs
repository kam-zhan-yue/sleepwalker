using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    public enum CameraState
    {
        None = 0,
        TrackingPlayer = 1,
        Transition = 2,
        Paused = 3
    }
    
    public static CameraManager instance;

    [BoxGroup("Setup Variables")] public float transitionInDuration = 0.5f;
    [BoxGroup("Setup Variables")] public float transitionOutDuration = 0f;
    [BoxGroup("Setup Variables")] public Ease easing = Ease.OutQuart;

    [NonSerialized, ShowInInspector, ReadOnly] 
    private Vector3 offset = Vector3.zero; //for z offset and offset to follow mouse pointer
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private float damp = 0.3f;
    
    [NonSerialized, ShowInInspector, ReadOnly] 
    private float dirOffset = 3f;

    private CameraState state = CameraState.None;

    [ShowInInspector, ReadOnly]
    private CameraState State
    {
        get => state;
        set
        {
            previousState = state;
            state = value;
        }
    }

    [NonSerialized, ShowInInspector, ReadOnly] 
    private Transform target;
    
    private Camera mainCamera;

    private Tween transitionTween;

    private CameraState previousState;

    public Action onTransitionOutEnded;

    private void Awake()
    {
        if (instance && instance != this)
            Destroy(gameObject);
        else
            instance = this;
        mainCamera = Camera.main;

        OnPlayerAdded(GameObject.FindGameObjectWithTag("Player"));
    }

    private void Update()
    {
        switch (State)
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
            case CameraState.Paused:
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
        State = CameraState.TrackingPlayer;
    }

    public void OnPlayerRemoved(GameObject _player)
    {
        State = CameraState.None;
    }

    public void OnDialogueEventStarted(Transform _transform)
    {
        State = CameraState.Transition;
        Vector3 position = _transform.position;
        position.z = transform.position.z;
        transitionTween = transform.DOMove(position, transitionInDuration)
            .SetEase(easing);
    }

    public void OnDialogueEventEnded(Transform _transform)
    {
        Vector3 position = target.position;
        position.z = transform.position.z;
        transitionTween = transform.DOMove(position, transitionOutDuration)
            .SetEase(easing)
            .OnComplete(() =>
            {
                State = CameraState.TrackingPlayer;
                onTransitionOutEnded?.Invoke();
            });;
    }

    public void OnPauseStarted()
    {
        State = CameraState.Paused;
    }

    public void OnPauseEnded()
    {
        State = previousState;
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
