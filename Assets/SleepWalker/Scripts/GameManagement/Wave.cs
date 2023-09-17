using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Wave : MonoBehaviour
{
    [BoxGroup("Setup Variables")] public GameObjectRuntimeSet enemyRuntimeSet;
    [BoxGroup("Setup Variables")] public GameObject enemyLightPrefab;
    [BoxGroup("Setup Variables")] public GameObject enemyHeavyPrefab;
    [BoxGroup("Setup Variables")] public List<WaveObject> enemies;
    [BoxGroup("Setup Variables")] public UnityEvent onComplete;

    private bool activated = false;
    
    [Serializable]
    public class WaveObject
    {
        public Transform transform;
        public EnemyType enemyType;
    }
    
    [Serializable]
    public enum EnemyType
    {
        Light = 0,
        Heavy = 1
    }

    [Button]
    public void Activate()
    {
        activated = true;
        for (int i = 0; i < enemies.Count; ++i)
        {
            Spawn(enemies[i]);
        }
    }


    private void Spawn(WaveObject _waveObject)
    {
        GameObject enemy;
        switch (_waveObject.enemyType)
        {
            case EnemyType.Light: 
                enemy = Instantiate(enemyLightPrefab, _waveObject.transform);
                enemy.SetActiveFast(true);
                break;
            case EnemyType.Heavy:
                enemy = Instantiate(enemyHeavyPrefab, _waveObject.transform);
                enemy.SetActiveFast(true);
                break;
        }
    }

    public void OnEnemyRemoved(GameObject _enemy)
    {
        if (!activated)
            return;
        // Debug.Log($"Enemy Removed! Count is {enemyRuntimeSet.items.Count}");
        if (enemyRuntimeSet.items.Count <= 0)
        {
            OnComplete();
            activated = false;
        }
    }

    private void OnComplete()
    {
        onComplete?.Invoke();
    }
}
