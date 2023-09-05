using System;
using Sirenix.OdinInspector;

[Serializable]
public class DialogueAction
{
    public DialogueType type = DialogueType.Message;
    
    [ShowIf("type", DialogueType.Message)]
    public bool hideName = false;
    
    [ShowIf("type", DialogueType.Message)]
    public string actor = string.Empty;
    
    [ShowIf("type", DialogueType.Message)]
    public string message = string.Empty;

    [ShowIf("type", DialogueType.Pause)] 
    public float pauseTime = 1f;
    
    [ShowIf("type", DialogueType.Animation)]
    public string animationName;
}
