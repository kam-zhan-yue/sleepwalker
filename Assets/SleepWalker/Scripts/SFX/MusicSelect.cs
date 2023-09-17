using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelect : MonoBehaviour
{
    //selects music for when the scene is opened, does not change if currently on right track
    [SerializeField] AudioClip thisLevelMusic;
    private void Start()
    {
        AudioClip currentMusic = MusicPlayer.instance.GetComponent<AudioSource>().clip;

        if (currentMusic != thisLevelMusic)
        {
            MusicPlayer.instance.GetComponent<MusicPlayer>().ChangeTrack(thisLevelMusic);
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
