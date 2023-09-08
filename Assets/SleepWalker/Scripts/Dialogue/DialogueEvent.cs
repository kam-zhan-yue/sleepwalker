using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class DialogueEvent : MonoBehaviour
{
    [FoldoutGroup("Game Events")] public GameEvent dialogueEventStarted;
    [FoldoutGroup("Game Events")] public GameEvent dialogueEventEnded;
    [FoldoutGroup("Scene Variables")] public List<DialogueActor> actors = new();

    [FoldoutGroup("Setup Variables")] [InlineEditor()]
    public DialogueScript script;

    private BoxCollider2D dialogueZone;
    
    private readonly Dictionary<string, DialogueActor> actorDictionary = new();

    private readonly Queue<DialogueGroup> dialogueQueue = new();

    private DialogueGroup currentGroup;
    
    private Tween typeWriterTween;

    private int finishedEvents = 0;
    private int totalEvents = 0;

    private void Awake()
    {
        for (int i = 0; i < actors.Count; ++i)
        {
            actorDictionary.Add(actors[i].key, actors[i]);
        }
    }

    [Button]
    public void StartEvent()
    {
        dialogueQueue.Clear();
        for (int i = 0; i < script.groups.Count; ++i)
            dialogueQueue.Enqueue(script.groups[i]);
        DequeueDialogue();
        dialogueEventStarted.Raise();
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
        Debug.Log("End Event");
        dialogueEventEnded.Raise();
    }
}
