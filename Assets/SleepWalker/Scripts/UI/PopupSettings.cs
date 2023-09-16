using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PopupSettings")]
public class PopupSettings : ScriptableObject
{
    public const string PLAYER_PREFS_VOLUME = "VOLUME";
    public AudioClip button;
    public void PlayButton()
    {
        if(SoundEffects.instance)
            SoundEffects.instance.Play(button);
    }
}
