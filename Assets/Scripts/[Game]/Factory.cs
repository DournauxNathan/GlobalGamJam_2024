using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HelloGameDev;

public class Factory : MonoBehaviour
{
    private ZoneManager zoneManager;

    public float currentHealth;
    public float maxHealth;

    bool isGameOver;

    public UnityEvent onGateOpen, onEnd;

    private void Start()
    {
        UIManager.Instance.SetDistrictTracker();
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

            if (currentHealth <= 0 && !isGameOver)
            {
                isGameOver = true;
                onEnd?.Invoke();
                Invoke("GameEnd", 10f);
            }
        }
    }

    public void GameEnd()
    {
        SceneTransitionManager.LoadScene(SceneTransitionManager.Scene.Credits);
    }
}
