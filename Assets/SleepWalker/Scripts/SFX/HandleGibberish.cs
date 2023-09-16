using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGibberish : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] Vector2 timeStartRange = new Vector2(0f, 3f);

    public void PlayGibberish()
    {
        if (source == null)
        {
            return;
        }
        Debug.Log("Playing gibberish");
        source.time = Random.Range(timeStartRange.x, timeStartRange.y);
        source.Play();
    }

    public void PauseGibberish()
    {
        Debug.Log("Pausing gibberish");
        source.Pause();
    }
}
