using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class DialogueAction
{
    public DialogueType type = DialogueType.Message;
    
    [ShowIf("type", DialogueType.Message)]
    [HorizontalGroup()]
    public string actor = string.Empty;
    
    [ShowIf("type", DialogueType.Message)]
    [HorizontalGroup(Width = 0.3f)]
    public bool custom = false;

    [ShowIf("custom")]
    public string customName = string.Empty;
    
    [ShowIf("type", DialogueType.Message)]
    [TextArea(4, 10)]
    public string message = string.Empty;

    [ShowIf("type", DialogueType.Pause)] 
    public float pauseTime = 1f;
    
    [ShowIf("type", DialogueType.Animation)]
    public string animationName;
}
