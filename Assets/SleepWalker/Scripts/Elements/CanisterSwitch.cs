using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CanisterSwitch : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public SleepCanister canister;
    [FormerlySerializedAs("pffSwitch")] [BoxGroup("Setup Variables")] public Sprite offSwitch;
    [BoxGroup("Setup Variables")] public Sprite onSwitch;
    [BoxGroup("Setup Variables")] public RectTransform promptHolder;
    
    [BoxGroup("Unity Events")] public UnityEvent onActivate;

    [NonSerialized, ShowInInspector, ReadOnly]
    private bool activated = false;
    private PlayerControls playerControls;
    private bool canInteract = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.PlayerInput.Enable();
        playerControls.PlayerInput.Interact.started += InteractStarted;
        spriteRenderer = GetComponent<SpriteRenderer>();
        promptHolder.gameObject.SetActiveFast(false);
    }

    private void OnTriggerEnter2D(Collider2D _collider2D)
    {
        if (activated)
            return;
        //Hacky way to check if player. I don't have much time, evidently.
        if (_collider2D.gameObject.TryGetComponent(out PlayerAwake _))
        {
            promptHolder.gameObject.SetActiveFast(true);
            canister.Highlight(true);
            canInteract = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D _collider2D)
    {
        if (activated)
            return;
        //Hacky way to check if player. I don't have much time, evidently.
        if (_collider2D.gameObject.TryGetComponent(out PlayerAwake _))
        {
            promptHolder.gameObject.SetActiveFast(false);
            canister.Highlight(false);
            canInteract = false;
        }
    }
    
    private void InteractStarted(InputAction.CallbackContext _callbackContext)
    {
        if (canInteract && !activated)
        {
            Activate();
        }
    }
    
    private void Activate()
    {
        promptHolder.gameObject.SetActiveFast(false);
        spriteRenderer.sprite = onSwitch;
        canister.Activate();
        activated = true;
        onActivate?.Invoke();
    }

    public void Reactivate()
    {
        activated = false;
        spriteRenderer.sprite = offSwitch;
        canister.Reactivate();
    }

    private void OnDestroy()
    {
        playerControls.Dispose();
    }
}
