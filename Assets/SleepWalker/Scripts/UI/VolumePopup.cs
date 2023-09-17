using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public class VolumePopup : Popup
{
    public Slider slider;
    public FloatReference volume;
    
    protected override void InitPopup()
    {
        if (PlayerPrefs.HasKey(PopupSettings.PLAYER_PREFS_VOLUME))
        {
            volume.Value = PlayerPrefs.GetFloat(PopupSettings.PLAYER_PREFS_VOLUME);
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
        PlayerPrefs.SetFloat(PopupSettings.PLAYER_PREFS_VOLUME, _value);
    }
}
