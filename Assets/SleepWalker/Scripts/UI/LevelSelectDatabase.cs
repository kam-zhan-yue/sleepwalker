using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public class LevelSelectData
{
    public bool autoUnlock = false;
    public string id;
    public int buildIndex;
    public string playerPrefsKey;
}
[CreateAssetMenu(menuName = "ScriptableObjects/LevelSelectDatabase")]
[Serializable]
public class LevelSelectDatabase : ScriptableObject
{

    public List<LevelSelectData> levelSelectDataList = new();
}
