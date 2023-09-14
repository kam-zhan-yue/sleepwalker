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

    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        slider.value = volume.Value;
    }
}
