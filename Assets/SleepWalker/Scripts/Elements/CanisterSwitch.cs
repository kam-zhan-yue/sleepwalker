using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CanisterSwitch : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public SleepCanister canister;
    [BoxGroup("Setup Variables")] public Fade fade;
    [BoxGroup("Setup Variables")] public Sprite offSwitch;
    [BoxGroup("Setup Variables")] public Sprite onSwitch;
    [BoxGroup("Setup Variables")] public RectTransform promptHolder;
    [BoxGroup("Setup Variables")] public BoolVariable inDialogue;
    [BoxGroup("Setup Variables")] public List<CanisterSwitch> connectedSwitches = new();
    
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
        if (inDialogue.Value)
            return;
        if (canInteract && !activated)
        {
            Activate();
        }
    }

    public void FadeOut()
    {
        fade.FadeOut();
        canister.fade.FadeOut();
    }

    private void Activate()
    {
        ActivateEffect();
        for (int i = 0; i < connectedSwitches.Count; ++i)
        {
            connectedSwitches[i].ActivateEffect();
        }
        canister.Activate();
        onActivate?.Invoke();
    }

    public void ActivateEffect()
    {
        promptHolder.gameObject.SetActiveFast(false);
        spriteRenderer.sprite = onSwitch;
        activated = true;
    }

    [Button]
    public void Reactivate()
    {
        canInteract = false;
        activated = false;
        spriteRenderer.sprite = offSwitch;
        canister.Reactivate();
    }

    private void OnDestroy()
    {
        playerControls.Dispose();
    }
}
