using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _smithConversionSound;
    [SerializeField] private AudioClip _districtVictorySound;
    [SerializeField] private AudioClip _victorySound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySmithConversionSound()
    {
        _audioSource.PlayOneShot(_smithConversionSound);
    }

    public void PlayDistrictVictorySound()
    {
        _audioSource.PlayOneShot(_districtVictorySound);
    }

    public void PlayVictorySound()
    {
        _audioSource.PlayOneShot(_victorySound);
    }
}
