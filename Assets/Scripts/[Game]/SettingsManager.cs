using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    // Mouse sensitivity settings
    public float mouseSensitivity { get; set; }

    public AudioMixer audioMixer;
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
    private void Start()
    {
        LoadVolumeSettings();
        mouseSensitivity = .5f;
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

    private void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey(musicVolumeParameter))
        {
            float musicVolume = PlayerPrefs.GetFloat(musicVolumeParameter);
            audioMixer.SetFloat(musicVolumeParameter, musicVolume);
        }

        if (PlayerPrefs.HasKey(sfxVolumeParameter))
        {
            float sfxVolume = PlayerPrefs.GetFloat(sfxVolumeParameter);
            audioMixer.SetFloat(sfxVolumeParameter, sfxVolume);
        }
    }
}

