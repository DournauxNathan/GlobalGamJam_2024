using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransitor : MonoBehaviour
{
    public AudioSource sadAudioSource;
    public AudioSource happyAudioSource;

    public AudioClip sadMusic;
    public AudioClip happyMusic;

    public bool test;

    private void Update()
    {
        if (test)
        {
            test = false;
            SwitchAudiosource();
        }
    }

    public void SwitchAudiosource()
    {
        sadAudioSource.volume = 0;
        happyAudioSource.volume = 1;

    }
}
