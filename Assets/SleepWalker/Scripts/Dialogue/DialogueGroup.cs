using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class DialogueGroup
{
    [Title("Action List")]
    public List<DialogueAction> actions;
    
    [HorizontalGroup(Width = 0.25f)]
    [Button, GUIColor(0.4f, 0.8f, 1)]
    public void AddMessage()
    {
        DialogueAction action = new();
        action.type = DialogueType.Message;
        actions.Add(action);
    }

    [HorizontalGroup(Width = 0.25f)]
    [Button, GUIColor(0.4f, 1f, 0.8f)]
    public void AddAnimation()
    {
        DialogueAction action = new();
        action.type = DialogueType.Animation;
        actions.Add(action);
    }
    
    [HorizontalGroup(Width = 0.25f)]
    [Button, GUIColor(0.8f, 0.8f, 0.8f)]
    public void AddPause()
    {
        DialogueAction action = new();
        action.type = DialogueType.Pause;
        actions.Add(action);
    }
}
