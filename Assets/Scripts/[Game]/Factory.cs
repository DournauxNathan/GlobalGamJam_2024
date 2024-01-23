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

    public UnityEvent onEnd;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si c'est un tag "Projectile"
        if (collision.collider.gameObject.CompareTag("Projectile"))
        {
            // On détruit le projectile
            Destroy(collision.collider.gameObject);

            currentHealth--;

            if (currentHealth <= 0)
            {
                onEnd?.Invoke();
                Invoke("GamEnd", 20f);
            }
        }
    }

    public void GameEnd()
    {
        SceneTransitionManager.LoadScene(SceneTransitionManager.Scene.Credits);
    }
}
