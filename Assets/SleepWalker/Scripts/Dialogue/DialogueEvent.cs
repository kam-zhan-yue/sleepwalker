using System.Collections;
using System.Collections.Generic;
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
}
