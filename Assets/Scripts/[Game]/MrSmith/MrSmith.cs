using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrSmith : MonoBehaviour
{
    public ZoneManager _zoneManager;
    private MrSmithMovement _mrSmithMovement;
    public int _hp = 3;
    private bool _isDead = false;

    private void Awake()
    {
        _zoneManager = gameObject.GetComponentInParent<ZoneManager>();
        _mrSmithMovement = gameObject.GetComponent<MrSmithMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _zoneManager.AddMrSmith(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si c'est un tag "Projectile"
        if (collision.collider.gameObject.CompareTag("Projectile") && !_isDead)
        {
            // On détruit le projectile
            Destroy(collision.collider.gameObject);

            _hp--;

            if (_hp <= 0)
            {
                // On change sa couleur pour du rose
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.magenta;

                _mrSmithMovement._navMeshAgent.SetDestination(transform.position);

                _zoneManager.UpdateCompletion();

                _isDead = true;
                _mrSmithMovement._animator.SetBool("isDead", true);
                _mrSmithMovement._animator.SetFloat("velocity", 0f);
            }
        }
    }
}
