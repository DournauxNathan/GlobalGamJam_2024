using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransitor : MonoBehaviour
{
    public AudioSource sadAudioSource;
    public AudioSource happyAudioSource;

    public AudioClip sadMusic;
    public AudioClip happyMusic;

    private void Start()
    {
        _audioSource.clip = sadMusic;
        _audioSource.Play();
    }

    public void SwitchAudiosource()
    {
        _audioSource.clip = sadMusic;
        _audioSource.Play();
    }
}
