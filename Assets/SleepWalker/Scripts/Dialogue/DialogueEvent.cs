using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueEvent : MonoBehaviour
{
    [Serializable]
    public class DialogueUnityEvent
    {
        public string id;
        public UnityEvent unityEvent;
    }
    [FoldoutGroup("Unity Event")] public UnityEvent onFinished; 
    [FoldoutGroup("Unity Event")] public List<DialogueUnityEvent> onDialogueList; 
    [FoldoutGroup("Game Events")] public GameEvent dialogueEventStarted;
    [FoldoutGroup("Game Events")] public GameEvent dialogueEventEnded;
    [FoldoutGroup("Game Events")] public TransformGameEvent transformDialogueEventStarted;
    [FoldoutGroup("Game Events")] public TransformGameEvent transformDialogueEventEnded;
    [FoldoutGroup("Scene Variables")] public List<DialogueActor> actors = new();

    // [FoldoutGroup("Setup Variables")] [InlineEditor()]
    // public bool useZone = false;
    //
    // [ShowIf("useZone")]
    // public BoxCollider2D dialogueZone;
    
        
    [FoldoutGroup("Setup Variables")] [InlineEditor()]
    public DialogueScript script;

    private readonly Dictionary<string, DialogueActor> actorDictionary = new();

    private readonly Queue<DialogueGroup> dialogueQueue = new();

    private DialogueGroup currentGroup;
    
    private Tween typeWriterTween;

    private int finishedEvents = 0;
    private int totalEvents = 0;

    private UIControls uiControls;
    private bool played = false;

    private void Awake()
    {
        for (int i = 0; i < actors.Count; ++i)
        {
            actorDictionary.Add(actors[i].key, actors[i]);
        }

        uiControls = new UIControls();
    }

    private void OnTriggerEnter2D(Collider2D _collider2D)
    {
        //If already played, then don't bother
        if (script.playOnce && played)
            return;
        if (_collider2D.gameObject.TryGetComponent(out DialogueTrigger _))
        {
            if (_collider2D.gameObject.TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = Vector2.zero;
            }
            StartEvent();
        }
    }

    [Button]
    public void StartEvent()
    {
        //If already played, then don't bother
        if (script.playOnce && played)
            return;
        uiControls.UIInput.Enable();
        uiControls.UIInput.Next.started += NextStarted;
        dialogueQueue.Clear();
        for (int i = 0; i < script.groups.Count; ++i)
            dialogueQueue.Enqueue(script.groups[i]);
        DequeueDialogue();
        dialogueEventStarted.Raise();
        if (script.useCamera)
        {
            transformDialogueEventStarted.Raise(transform);
        }
    }

    private void DequeueDialogue()
    {
        if (dialogueQueue.Count > 0)
        {
            finishedEvents = 0;
            totalEvents = 0;
            currentGroup = dialogueQueue.Dequeue();
            List<DialogueAction> actions = currentGroup.actions;
            for (int i = 0; i < actions.Count; ++i)
                PlayAction(actions[i]);
        }
        else
        {
            EndEvent();
        }
    }
    
    private void NextStarted(InputAction.CallbackContext _callbackContext)
    {
        MoveDialogue();
    }
    
    [Button]
    private void MoveDialogue()
    {
        //If not finished, then force finish
        if (!CurrentGroupFinished())
        {
            finishedEvents = totalEvents;
            List<DialogueAction> actions = currentGroup.actions;
            for (int i = 0; i < actions.Count; ++i)
                ForceEndAction(actions[i]);
        }
        //Else, start a new group
        else
        {
            List<DialogueAction> actions = currentGroup.actions;
            for (int i = 0; i < actions.Count; ++i)
                EndAction(actions[i]);
            DequeueDialogue();
        }
    }

    private void PlayAction(DialogueAction _action)
    {
        switch (_action.type)
        {
            case DialogueType.Message:
                if (actorDictionary.TryGetValue(_action.actor, out DialogueActor messageActor))
                {
                    totalEvents++;
                    messageActor.dialogueObject.ShowMessage(_action.GetName(), _action.message, OnEventComplete);
                }
                break;
            case DialogueType.Animation:
                if (actorDictionary.TryGetValue(_action.actor, out DialogueActor animationActor))
                {
                    totalEvents++;
                    animationActor.dialogueObject.PlayAnimation(_action.animationName, OnEventComplete);
                }
                break;
            case DialogueType.Pause:
            default:
                break;
        }

        for (int i = 0; i < onDialogueList.Count; ++i)
        {
            if (_action.id == onDialogueList[i].id)
                onDialogueList[i].unityEvent?.Invoke();
        }
    }
    
    private void OnEventComplete()
    {
        finishedEvents++;
    }

    private bool CurrentGroupFinished()
    {
        return finishedEvents >= totalEvents;
    }

    private void EndAction(DialogueAction _action)
    {
        switch (_action.type)
        {
            case DialogueType.Message:
                if (actorDictionary.TryGetValue(_action.actor, out DialogueActor messageActor))
                    messageActor.dialogueObject.HideMessage();
                break;
            case DialogueType.Animation:
                if (actorDictionary.TryGetValue(_action.actor, out DialogueActor animationActor)) 
                    animationActor.dialogueObject.StopAnimation(_action.animationName);
                break;
            case DialogueType.Pause:
            default:
                break;
        }
    }

    private void ForceEndAction(DialogueAction _action)
    {
        switch (_action.type)
        {
            case DialogueType.Message:
                if (actorDictionary.TryGetValue(_action.actor, out DialogueActor messageActor))
                    messageActor.dialogueObject.ForceEndMessage();
                break;
            case DialogueType.Animation:
                if (actorDictionary.TryGetValue(_action.actor, out DialogueActor animationActor)) 
                    animationActor.dialogueObject.ForceEndAnimation();
                break;
            case DialogueType.Pause:
            default:
                break;
        }
    }
        
    private void EndEvent()
    {
        played = true;
        uiControls.UIInput.Next.started -= NextStarted;
        uiControls.UIInput.Disable();
        dialogueEventEnded.Raise();
        if (script.useCamera)
        {
            transformDialogueEventEnded.Raise(transform);
        }
        onFinished?.Invoke();
    }
    
    private void OnDestroy()
    {
        uiControls.UIInput.Disable();
        uiControls.Dispose();
    }
}
