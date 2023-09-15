using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    [SerializeField] AudioClip defaultMusic;
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Music").Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        source = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        ChangeTrack(defaultMusic);
    }

    public void ChangeTrack(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public void PauseMusic()
    {
        source.Pause();
    }

    public void ResumeMusic()
    {
        source.Play();
    }
}
