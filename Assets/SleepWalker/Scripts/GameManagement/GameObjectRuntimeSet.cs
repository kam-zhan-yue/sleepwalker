using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameObject Runtime Set")]
public class GameObjectRuntimeSet : RuntimeSet<GameObject>
{
    [SerializeField] private GameObjectEvent itemAddedEvent;
    [SerializeField] private GameObjectEvent itemRemovedEvent;
    public override GameEvent<GameObject> OnItemAdded => itemAddedEvent;
    public override GameEvent<GameObject> OnItemRemoved => itemRemovedEvent;
}