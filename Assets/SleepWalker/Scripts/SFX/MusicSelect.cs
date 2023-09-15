using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelect : MonoBehaviour
{
    //selects music for when the scene is opened, does not change if currently on right track
    [SerializeField] AudioClip thisLevelMusic;
    private void Start()
    {
        GameObject musicPlayerObj = GameObject.FindGameObjectWithTag("Music");
        AudioClip currentMusic = musicPlayerObj.GetComponent<AudioSource>().clip;

        if (currentMusic != thisLevelMusic)
        {
            musicPlayerObj.GetComponent<MusicPlayer>().ChangeTrack(thisLevelMusic);
        }
    }

    public void PassToMusicPlayer (AudioClip clip)
    {
        //finds the active music manager and changes the track
        MusicPlayer musicPlayer = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>();
        musicPlayer.ChangeTrack(clip);
        Debug.Log("Passing music now");
    }

    public void PassToSFXPlayer(AudioClip clip)
    {
        //finds the active music manager and changes the track
        SoundEffects sfxPlayer = GameObject.FindGameObjectWithTag("SFX Manager").GetComponent<SoundEffects>();
        sfxPlayer.Play(clip);
        Debug.Log("Passing audio now");
    }
}
