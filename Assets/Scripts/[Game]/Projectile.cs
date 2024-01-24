using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 2f;

    public float speed;

    void Start()
    {
        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // You can customize the logic for what happens when the projectile collides with something
        // For now, we'll just destroy the projectile
        //Destroy(gameObject);
    }
}
