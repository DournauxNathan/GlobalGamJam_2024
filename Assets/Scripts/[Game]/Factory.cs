using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HelloGameDev;


public class Factory : MonoBehaviour
{
    private FactoryManager factoryManager;
    private bool _isDead = false;

    private ZoneManager zoneManager;
    private Saturation saturation;

    public float currentHealth;
    public float maxHealth;

    private void Awake()
    {
        saturation = GetComponent<Saturation>();
        factoryManager = GetComponentInParent<FactoryManager>();
    }
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si c'est un tag "Projectile"
        if (
            (
                collision.collider.gameObject.CompareTag("Projectile") ||
                collision.collider.gameObject.CompareTag("Projectile_B") ||
                collision.collider.gameObject.CompareTag("Projectile_C")
            ) && !_isDead )
        {
            Debug.Log("Touché la tour !");
            // On détruit le projectile
            Destroy(collision.collider.gameObject);

            currentHealth -= 10;
            Debug.Log("Current health / maxHealth = " + (1 - (currentHealth / maxHealth)));
            saturation.SetSaturation(1 - Mathf.Clamp01(currentHealth / maxHealth));

            if (currentHealth <= 0)
            {
                _isDead = true;
                factoryManager.AddClearedFactory();
            }
        }
    }

    
}
