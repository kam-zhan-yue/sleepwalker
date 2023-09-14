using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyTrackerPopup : Popup
{
    public GameObjectRuntimeSet enemyRuntimeSet;
    public TMP_Text text;
    private int maxEnemies = 0;
    
    protected override void InitPopup()
    {
        
    }

    public void OnEnemyAdded(GameObject _gameObject)
    {
        if (enemyRuntimeSet.items.Count > maxEnemies)
        {
            maxEnemies = enemyRuntimeSet.items.Count;
            UpdateText();
        }
    }

    public void OnEnemyRemoved(GameObject _gameObject)
    {
        UpdateText();
    }

    private void UpdateText()
    {
        text.SetText($"{enemyRuntimeSet.items.Count} / {maxEnemies}");
    }
}
