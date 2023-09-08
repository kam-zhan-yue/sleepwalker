using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class DialogueObject : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public DialoguePopup dialoguePopupPrefab;

    private DialoguePopup dialoguePopup;

    [BoxGroup("Setup Variables")] public Vector3 offset;
    private Collider2D sizeCollider;

    private void Awake()
    {
        sizeCollider = GetComponent<Collider2D>();
    }
    
    private void Start()
    {
        dialoguePopup = Instantiate(dialoguePopupPrefab);
        dialoguePopup.gameObject.SetActiveFast(false);
    }

    [Button]
    public void ShowMessage(string _name, string _text, Action _onComplete = null)
    {
#if UNITY_EDITOR
        if(dialoguePopup == null)
            dialoguePopup = Instantiate(dialoguePopupPrefab);
        if(sizeCollider == null)
            sizeCollider = GetComponent<Collider2D>();
#endif
        
        Bounds bounds = sizeCollider.bounds;
        Vector3 dialoguePosition = bounds.center;
        dialoguePosition.y = bounds.center.y + (bounds.extents.y);
        dialoguePopup.Show(dialoguePosition + offset, _name, _text, _onComplete);
    }

    public void ForceEndMessage()
    {
        dialoguePopup.Stop();
    }

    public void HideMessage()
    {
        dialoguePopup.Hide();
    }

    public void PlayAnimation(string _animationName, Action _onComplete = null)
    {
        
    }

    public void ForceEndAnimation()
    {
        
    }

    public void StopAnimation(string _animationName)
    {
        
    }
}
