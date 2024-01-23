using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MrSmith : MonoBehaviour
{
    public ZoneManager _zoneManager;
    private MrSmithMovement _mrSmithMovement;
    public int _hp = 3;
    private bool _isDead = false;
    public string projectileTag;

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
        if (collision.gameObject.CompareTag(projectileTag) && !_isDead)
        {
            Debug.Log("Touché !");
            // On détruit le projectile
            Destroy(collision.collider.gameObject);

            _hp--;

            if (_hp <= 0)
            {
                // On change sa couleur pour du rose
                UIManager.Instance._soundManager.PlaySmithConversionSound();

                _mrSmithMovement._navMeshAgent.isStopped = true;

                _zoneManager.UpdateCompletion();

                _isDead = true;
                _mrSmithMovement._animator.SetBool("isDead", true);
                _mrSmithMovement._animator.SetFloat("velocity", 0f);
            }
        }
    }
}
