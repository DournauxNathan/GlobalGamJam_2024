using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HelloGameDev;
using System;

public class Factory : MonoBehaviour
{

    public Animator gate;

    private ZoneManager zoneManager;

    public float currentHealth;
    public float maxHealth;

    bool isGameOver;

    public UnityEvent onGateOpen, onEnd;

    private void Start()
    {
        UIManager.allZoneComplete += OnGateOpen;
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si c'est un tag "Projectile"
        if (collision.collider.gameObject.CompareTag("Projectile"))
        {
            // On détruit le projectile
            Destroy(collision.collider.gameObject);

            currentHealth -= 10;

            if (currentHealth < 0 && !isGameOver)
            {
                isGameOver = true;
                Invoke("GameEnd", 10f);
            }
        }
    }

    public void OnGateOpen()
    {
        gate.SetBool("Open", true);
        onGateOpen?.Invoke();
        
    }

    public void GameEnd()
    {
        SceneTransitionManager.LoadScene(SceneTransitionManager.Scene.Credits);
    }
}
