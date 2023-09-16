using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public static SoundEffects instance;
    public FloatReference volume;
    [SerializeField] private AudioSource[] audioSources;
    private int sourcePlaying = 0;

    private void Awake()
    {
        if (instance && instance != this)
            Destroy(gameObject);
        else
            instance = this;
        
        //it should exist across scenes
        DontDestroyOnLoad(this.gameObject);
        sourcePlaying = 0;

        //get the audio sources (children of this prefab)
        audioSources = new AudioSource[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            audioSources[i] = transform.GetChild(i).GetComponent<AudioSource>();
        }
    }
    
    public void Play(AudioClip _clip)
    {
        Debug.Log("Playing audio");
        AudioSource source = audioSources[sourcePlaying];
        source.volume = volume.Value;
        source.clip = _clip;
        source.time = 0;
        source.Play();

        sourcePlaying++;
        if (sourcePlaying >= audioSources.Length)
        {
            sourcePlaying = 0;
        }
    }
}
