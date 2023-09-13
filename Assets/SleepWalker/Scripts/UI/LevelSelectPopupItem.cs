using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectPopupItem : MonoBehaviour
{
    [NonSerialized, ShowInInspector, ReadOnly]
    private LevelSelectData data;

    public TMP_Text text;
    public Button button;

    public void Init(LevelSelectData _data)
    {
        data = _data;
        bool unlocked = data.autoUnlock || PlayerPrefs.HasKey(data.playerPrefsKey);
        button.interactable = unlocked;
        text.SetText(data.id);
    }

    public void Pressed()
    {
        Debug.Log($"Go to build index {data.buildIndex}");
    }
}
