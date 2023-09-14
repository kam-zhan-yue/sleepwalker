using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class VolumePopup : Popup
{
    public Slider slider;
    public FloatReference volume;

    private const string PLAYER_PREFS_VOLUME = "VOLUME";
    private float value = 1f;

    protected override void InitPopup()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_VOLUME))
        {
            volume.Value = PlayerPrefs.GetFloat(PLAYER_PREFS_VOLUME);
        }
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        slider.value = volume.Value;
    }

    public void OnSliderChanged(float _value)
    {
        volume.Value = _value;
        PlayerPrefs.SetFloat(PLAYER_PREFS_VOLUME, _value);
    }
}
