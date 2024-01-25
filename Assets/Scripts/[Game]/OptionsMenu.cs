using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider sensitivitySlider;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChange);
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }

    private void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChange);
        sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChange);
        sensitivitySlider.onValueChanged.RemoveListener(OnSensitivityChanged);
    }

    private void OnMusicVolumeChange(float volume)
    {
        SettingsManager.Instance.SetMusicVolume(volume);
    }

    private void OnSFXVolumeChange(float volume)
    {
        SettingsManager.Instance.SetSFXVolume(volume);
    }

    public void OnSensitivityChanged(float value)
    {
        SettingsManager.Instance.mouseSensitivity = value;
    }
}
