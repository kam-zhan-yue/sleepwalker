using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DialogueEvent : MonoBehaviour
{
    [BoxGroup("Setup Variables")]
    public List<DialogueActor> actors = new();

    [BoxGroup("Setup Variables")] 
    public DialogueScript script;
    
    private BoxCollider2D dialogueZone;
}
