using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;
    public FloatReference volume;
    [SerializeField] AudioClip defaultMusic;
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        if (instance && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        if (PlayerPrefs.HasKey(PopupSettings.PLAYER_PREFS_VOLUME))
        {
            volume.Value = PlayerPrefs.GetFloat(PopupSettings.PLAYER_PREFS_VOLUME);
        }
        source = GetComponent<AudioSource>();
        source.volume = volume.Value;
        DontDestroyOnLoad(this.gameObject);
        ChangeTrack(defaultMusic);
    }

    public void ChangeTrack(AudioClip clip)
    {
        source.clip = clip;
        source.volume = volume.Value;
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

    public void OnVolumeChanged(float _volume)
    {
        if(source != null)
            source.volume = _volume;
    }
}
