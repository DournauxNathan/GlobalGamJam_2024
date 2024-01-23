using System;
using System.Collections;
using UnityEngine;

public class MusicTransitor : MonoBehaviour
{
    public AudioSource sadAS, happyAS;

    float defaultVolume = 1f;
    float transitionTime = 1.25f;

    private void Start()
    {
        UIManager.zoneCleared += ChangeClip;
    }

    public void ChangeClip()
    {
        AudioSource nowPlaying = sadAS;
        AudioSource target = happyAS;

        if (nowPlaying.isPlaying == false)
        {
            nowPlaying = happyAS;
            target = sadAS;

        }

        StartCoroutine(MixSource(nowPlaying, target));

        Debug.Log("Currently playing" + nowPlaying.clip);
    }

    public void Mute()
    {
        sadAS.enabled = true;
        happyAS.enabled = true;
    }

    private IEnumerator MixSource(AudioSource nowPlaying, AudioSource target)
    {
        float percentage = 0;

        while (nowPlaying.volume >0)
        {
            nowPlaying.volume = Mathf.Lerp(defaultVolume, 0, percentage);
            percentage += Time.deltaTime / transitionTime;
            yield return null;
        }

        nowPlaying.Pause();
        if (target.isPlaying == false)
        {
            target.Play();
        }
        target.UnPause();
        percentage = 0;

        while (target.volume < defaultVolume)
        {
            target.volume = Mathf.Lerp(0, defaultVolume, percentage);
            percentage += Time.deltaTime / transitionTime;
            yield return null;
        }

;    }
}
