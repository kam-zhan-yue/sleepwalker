using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public FloatReference volume;
    [SerializeField] private AudioSource[] audioSources;
    private int sourcePlaying = 0;

    private void Awake()
    {
        //only one should exist in the scene
        if (GameObject.FindGameObjectsWithTag("SFX Manager").Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

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

    public void Play(AudioClip clip)
    {
        Debug.Log("Playing audio");
        AudioSource source = audioSources[sourcePlaying];
        source.clip = clip;
        source.time = 0;
        source.Play();

        sourcePlaying++;
        if (sourcePlaying >= audioSources.Length)
        {
            sourcePlaying = 0;
        }
    }
}
