using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class AudioData
{
    public string id = String.Empty;
    public AudioClip clip;
    public float volume = 1f;
}
[CreateAssetMenu(menuName = "ScriptableObjects/AudioDatabase")]
public class AudioDatabase : ScriptableObject
{
    [TableList]
    public List<AudioData> audioDataList = new();

    public bool TryGetAudio(string _id, out AudioData _data)
    {
        for (int i = 0; i < audioDataList.Count; ++i)
        {
            if (audioDataList[i].id == _id)
            {
                _data = audioDataList[i];
                return true;
            }
        }

        _data = null;
        return false;
    }

    public void PlaySFX(string _id)
    {
        if (TryGetAudio(_id, out AudioData data))
        {
            if (SoundEffects.instance != null)
            {
                SoundEffects.instance.Play(data.clip, data.volume);
            }
        }
    }
}
