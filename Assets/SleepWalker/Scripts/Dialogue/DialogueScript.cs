using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScript", menuName = "ScriptableObjects/DialogueScript", order = 100)]
public class DialogueScript : ScriptableObject
{
    public List<string> actors = new();
    public List<DialogueAction> actions = new();
}
