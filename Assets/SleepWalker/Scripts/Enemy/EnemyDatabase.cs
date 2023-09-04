using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "ScriptableObjects/EnemyDatabase", order = 100)]
public class EnemyDatabase : ScriptableObject
{
    [Serializable]
    public class EnemyDataEntry
    {
        public EnemyType type;
        public Stats stats;
    }
    [TableList] public List<EnemyDataEntry> enemyList = new();
}