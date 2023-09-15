using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelClearedPopup : Popup
{
    public GameObjectRuntimeSet enemyRuntimeSet;

    protected override void InitPopup()
    {
        HidePopup();
    }

    public void OnEnemyRemoved(GameObject _enemy)
    {
        if (enemyRuntimeSet.items.Count <= 0)
        {
            ShowPopup();
        }
    }
}
