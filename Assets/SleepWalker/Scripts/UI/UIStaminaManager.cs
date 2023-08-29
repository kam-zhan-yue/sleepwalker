using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStaminaManager : MonoBehaviour
{
    public Slider staminaSlider;

    public void UpdateDisplayValue(float value)
    {
        staminaSlider.value = value;
    }

    public void UpdateMaxValue(float max)
    {
        staminaSlider.maxValue = max;
    }
}
