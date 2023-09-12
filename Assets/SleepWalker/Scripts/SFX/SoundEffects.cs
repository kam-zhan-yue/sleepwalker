using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    private AudioSource[] audioSources;
    private int sourcePlaying = 0;

    public void Play(AudioClip clip)
    {
        AudioSource source = audioSources[sourcePlaying];
        source.clip = clip;
        source.Play();

        sourcePlaying++;
        if (sourcePlaying >= audioSources.Length)
        {
            sourcePlaying = 0;
        }
    }
}
