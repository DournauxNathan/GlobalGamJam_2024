using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    public ZoneManager _currentZone;
    public VisualEffect _rainEffect;
    [HideInInspector] public PlayerController _playerController;

    public AudioSource _audioSourceRain;
    public AudioSource _audioSourceSteps;
    public AudioClip[] _audioClips;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateRain()
    {
        //Debug.Log("Update rain !");
        if (_rainEffect != null)
        {
            float rainIntensity = Mathf.Abs(_playerController._currentZone._completion * 800 - 800);

            //Debug.Log("Rain effect modifié (" + _playerController._currentZone.name + " : " + _playerController._currentZone._completion + ") : " + rainIntensity);

            _rainEffect.SetFloat("Rain Intensity", rainIntensity);
        }

        if (_audioSourceRain != null)
        {
            //Debug.Log("Rain audio modifié (" + Mathf.Abs(_playerController._currentZone._completion - 1) + ")");
            _audioSourceRain.volume = Mathf.Abs(_playerController._currentZone._completion - 1);
        }
        
    }
}
