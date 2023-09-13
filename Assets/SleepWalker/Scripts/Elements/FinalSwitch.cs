using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.InputSystem;

public class FinalSwitch : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public EndGamePopup endGamePopup;
    [BoxGroup("Setup Variables")] public Sprite onSwitch;
    [BoxGroup("Setup Variables")] public Fade fade;
    [BoxGroup("Setup Variables")] public BoxCollider2D boxCollider2D;
    [BoxGroup("Setup Variables")] public RectTransform promptHolder;
    
    private SpriteRenderer spriteRenderer;
    private bool canInteract = false;
    private PlayerControls playerControls;
    private bool activated = false;
    
    private void Awake()
    {
        //Turn off the sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color tempColor = spriteRenderer.color;
        tempColor.a = 0f;
        spriteRenderer.color = tempColor;
        boxCollider2D.enabled = false;
        promptHolder.gameObject.SetActiveFast(false);
        
        playerControls = new PlayerControls();
        playerControls.PlayerInput.Enable();
        playerControls.PlayerInput.Interact.started += InteractStarted;
    }

    public void ActivateCollider()
    {
        boxCollider2D.enabled = true;
    }
    
    public void FadeIn()
    {
        fade.FadeIn();
    }
    
    private void OnTriggerEnter2D(Collider2D _collider2D)
    {
        if (activated)
            return;
        //Hacky way to check if player. I don't have much time, evidently.
        if (_collider2D.gameObject.TryGetComponent(out PlayerAwake _))
        {
            promptHolder.gameObject.SetActiveFast(true);
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

    public void BossKilled()
    {
        Debug.Log("BOSS KILLED INSTEAD!");
        endGamePopup.GameOver();
    }

    private void Activate()
    {
        activated = true;
        spriteRenderer.sprite = onSwitch;
        Debug.Log("END THE WORLD!!!!!");
        endGamePopup.WorldEnd();
    }
}
