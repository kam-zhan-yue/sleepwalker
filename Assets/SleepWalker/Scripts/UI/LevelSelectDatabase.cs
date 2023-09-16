using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

    [Button]
    private void ClearLocalSave()
    {
        for (int i = 0; i < levelSelectDataList.Count; ++i)
        {
            PlayerPrefs.DeleteKey(levelSelectDataList[i].playerPrefsKey);
        }
    }
}
