using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    // Mouse sensitivity settings
    public float mouseSensitivity = 2.0f;

    public AudioMixer audioMixer;
    public string masterVolumeParameter = "masterVolume";
    public string musicVolumeParameter = "musicVolume";
    public string sfxVolumeParameter = "sfxVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat(masterVolumeParameter, volume);
        SaveVolumeSettings();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(musicVolumeParameter, volume);
        SaveVolumeSettings();
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(sfxVolumeParameter, volume);
        SaveVolumeSettings();
    }

    private void SaveVolumeSettings()
    {
        float value;
        
        if (audioMixer.GetFloat(musicVolumeParameter, out value))
        {
            PlayerPrefs.SetFloat(musicVolumeParameter, value);
        }
        if (audioMixer.GetFloat(sfxVolumeParameter, out value))
        {
            PlayerPrefs.SetFloat(sfxVolumeParameter, value);
        }
    }
}

