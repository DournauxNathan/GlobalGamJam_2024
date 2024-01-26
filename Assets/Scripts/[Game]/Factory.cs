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

    // VFX
    [SerializeField] private GameObject _hitVFX;

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
            // On détruit le projectile
            Destroy(collision.collider.gameObject);

            Instantiate(_hitVFX, collision.transform.position, Quaternion.identity);

            currentHealth -= 10;
            saturation.SetSaturation(1 - Mathf.Clamp01(currentHealth / maxHealth));

            if (currentHealth <= 0)
            {
                _isDead = true;
                factoryManager.AddClearedFactory();
            }
        }
    }

    
}
