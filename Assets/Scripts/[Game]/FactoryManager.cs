using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using HelloGameDev;

public class FactoryManager : MonoBehaviour
{
    public static Action gameWin;
    public Animator gate;
    bool isGameOver;

    public UnityEvent onGateOpen, onEnd;

    private Factory[] factories;

    public int currentClearedFactories = 0;
    public int maxFactories = 5;

    [SerializeField] private GameObject _fireworks;
    [SerializeField] private AudioSource _fireworksAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.allZoneCleared += OnGateOpen;
    }

    // Update is called once per frame
    void Update()
    {
        if ((currentClearedFactories >= maxFactories) && !isGameOver)
        {
            isGameOver = true;
            _fireworks.SetActive(true);
            _fireworksAudioSource.Play();
            _fireworksAudioSource.PlayDelayed(0.5f);
            _fireworksAudioSource.PlayDelayed(1f);

            onEnd?.Invoke();
            Invoke("GameEnd", 10f);
        }
    }

    public void AddClearedFactory()
    {
        currentClearedFactories++;
    }

    public void OnGateOpen()
    {
        gate.SetBool("Open", true);
        onGateOpen?.Invoke();
    }

    public void GameEnd()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneTransitionManager.LoadScene(SceneTransitionManager.Scene.Credits);
    }
}
