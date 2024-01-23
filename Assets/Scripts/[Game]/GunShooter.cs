using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooter : MonoBehaviour
{
    public GameObject projectilePrefab;

    public Transform shootingPoint;
    public float projectileSpeed = 10f;
    public float fireRate = 0.5f;

    private float nextFireTime;

    public AudioSource _audioSource;
    public AudioClip onShootSFX;

    void Update()
    {
        // Check if it's time to shoot again
        if (Time.time >= nextFireTime)
        {
            // Check for the fire input (you can customize this based on your input system)
            if (Input.GetButtonDown("Fire1"))
            {
                if (!_audioSource.isPlaying)
                {
                    _audioSource?.PlayOneShot(onShootSFX);
                }

                ShootProjectile();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    void ShootProjectile()
    {
        // Instantiate the projectile at the shooting point's position and rotation
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, shootingPoint.rotation);

        // Access the rigidbody of the projectile and apply a forward force
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        if (projectileRb != null)
        {
            projectileRb.AddForce(shootingPoint.forward * projectileSpeed, ForceMode.Force);
        }
        else
        {
            Debug.LogError("Projectile prefab does not have a Rigidbody component!");
        }
    }
}

