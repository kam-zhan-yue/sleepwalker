using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class DialogueEvent : MonoBehaviour
{
    [FoldoutGroup("Scene Variables")]
    public List<DialogueActor> actors = new();

    [FoldoutGroup("Setup Variables")] 
    [InlineEditor()]
    public DialogueScript script;
    
    private BoxCollider2D dialogueZone;

    private Sequence dialogueSequence;

    private int currentGroup = 0;

    [Button]
    public void StartEvent()
    {
        currentGroup = 0;
    }

    private void QueueDialogue()
    {
        List<DialogueAction> actions = script.GetActions(currentGroup);
        for (int i = 0; i < actions.Count; ++i)
        {
            QueueAction(actions[i]);
        }
    }

    private void QueueAction(DialogueAction _action)
    {
        switch (_action.type)
        {
            case DialogueType.Message:
                break;
            case DialogueType.Pause:
                break;
            case DialogueType.Animation:
                break;
        }
    }
}
