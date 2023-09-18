using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelect : MonoBehaviour
{
    //selects music for when the scene is opened, does not change if currently on right track
    [SerializeField] AudioClip thisLevelMusic;
    private void Start()
    {
        if (MusicPlayer.instance != null)
        {
            if (MusicPlayer.instance.TryGetComponent(out AudioSource source))
            {
                AudioClip currentMusic = source.clip;
                if (currentMusic != thisLevelMusic)
                {
                    MusicPlayer.instance.ChangeTrack(thisLevelMusic);
                }
            }
            else
            {
                Debug.LogWarning("Couldn't find an audio source on MusicPlayer!");
            }
        }
        else
        {
            Debug.LogWarning("No Music Player in the scene.");
        }
    }

    public void PassToMusicPlayer (AudioClip clip)
    {
        //finds the active music manager and changes the track
        MusicPlayer.instance.ChangeTrack(clip);
        // Debug.Log("Passing music now");
    }

    public void PassToSFXPlayer(AudioClip clip)
    {
        //finds the active music manager and changes the track
        SoundEffects.instance.Play(clip);
        // Debug.Log("Passing audio now");
    }
}
