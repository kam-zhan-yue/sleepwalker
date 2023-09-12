using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] GameObject musicPrefab;
    [SerializeField] GameObject musicObj;
    [SerializeField] AudioClip defaultMusic;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Music").Length != 0)
        {
            Destroy(this);
            return;
        }

        musicObj = Instantiate(musicPrefab);
        DontDestroyOnLoad(this.gameObject);
        ChangeTrack(defaultMusic);
    }

    public void ChangeTrack(AudioClip clip)
    {
        AudioSource source = musicObj.GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
    }

    public void PauseMusic()
    {
        AudioSource source = musicObj.GetComponent<AudioSource>();
        source.Pause();
    }

    public void ResumeMusic()
    {
        AudioSource source = musicObj.GetComponent<AudioSource>();
        source.Play();
    }
}
