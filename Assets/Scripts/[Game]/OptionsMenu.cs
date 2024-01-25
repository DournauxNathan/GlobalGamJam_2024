using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sensitivitySlider;

    private void Start()
    {
        // Initialize sliders with current settings
        volumeSlider.value = SettingsManager.instance.volume;
        sensitivitySlider.value = SettingsManager.instance.mouseSensitivity;
    }

    public void OnVolumeChanged(float value)
    {
        SettingsManager.instance.volume = value;
        // Implement code to adjust volume in your game
    }

    public void OnSensitivityChanged(float value)
    {
        SettingsManager.instance.mouseSensitivity = value;
        // Implement code to adjust mouse sensitivity in your game
    }
}
