using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class PolicemenAI : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private ZoneManager _zoneManager;

    // Movement
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private float _waitDuration;
    [SerializeField] private Vector3 _destination;

    // Shooting
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float fireRate;
    [SerializeField] private bool isShooting;
    [SerializeField] private bool isPlayerOnSight;

    // Rotation
    [SerializeField] private float rotationSpeed;

    public int _hp = 3;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _zoneManager = GetComponentInParent<ZoneManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // On sélectionne un spot aléatoire dans la zone
        SetRandomDestinationInBounds();
        StartCoroutine(MoveToDestination());
    }

    // Update is called once per frame
    private void Update()
    {
        DetectPlayer();

        _animator.SetBool("Shoot", isShooting);

        if (!_isMoving && !isPlayerOnSight)
        {
            SetRandomDestinationInBounds();
            StartCoroutine(MoveToDestination());
        }
    }

    private void SetRandomDestinationInBounds()
    {
        // On récupère un point aléatoire dans la boundingBox
        _destination = new Vector3(
            Random.Range(_zoneManager._boxCollider.bounds.min.x, _zoneManager._boxCollider.bounds.max.x),
            transform.position.y,
            Random.Range(_zoneManager._boxCollider.bounds.min.z, _zoneManager._boxCollider.bounds.max.z)
        );

        // On corrige la destination pour qu'elle soit atteignable par l'agent
        NavMeshHit hit;
        if (NavMesh.SamplePosition(_destination, out hit, 1f, NavMesh.AllAreas))
        {
            _destination = hit.position;
        }

        // On corrige la destination pour qu'elle soit à la hauteur de l'agent
        _destination.y = transform.position.y;
    }

    private void FollowTarget(Vector3 position)
    {
        if (isPlayerOnSight && !isShooting)
        {
            StopCoroutine(MoveToDestination());

            StartCoroutine(Fire());
        }
    }

    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private IEnumerator Fire()
    {
        isShooting = true;

        while (isPlayerOnSight)
        {
            _navMeshAgent.isStopped = true;
            ShootProjectile();
            
            yield return new WaitForSeconds(1f / fireRate); // Adjust the delay between shots as needed
        }
        //isShooting = false;
    }

    void DetectPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        if (colliders.Length > 0)
        {
            if (Vector3.Distance(transform.position, colliders[0].transform.position) <= detectionRadius)
            {
                isPlayerOnSight = true;

                RotateTowardsTarget(colliders[0].transform.position);

                FollowTarget(colliders[0].transform.position);
            }
            else
            {
                isShooting = false;
                isPlayerOnSight = false;
                _navMeshAgent.isStopped = false;
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
            projectileRb.AddForce(shootingPoint.forward * 1200, ForceMode.Force);
        }
        else
        {
            Debug.LogError("Projectile prefab does not have a Rigidbody component!");
        }
    }

    private IEnumerator MoveToDestination()
    {
        _isMoving = true;

        if (!isPlayerOnSight)
        {
            _navMeshAgent.SetDestination(_destination);
        }
        else
        {
            yield return null;
        }


        while (_navMeshAgent.velocity.Equals(Vector3.zero))
        {
            yield return null;
        }

        // Wait
        yield return new WaitForSeconds(_waitDuration);

        _isMoving = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // Calculate a direction vector from an angle in degrees
    public Vector3 DirFromAngle(float angleInDegrees, bool isGlobal)
    {
        if (!isGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si c'est un tag "Projectile"
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // On détruit le projectile
            Destroy(collision.gameObject);

            _hp--;

            if (_hp <= 0)
            {
                Destroy(this.gameObject);

                // On change sa couleur pour du rose
                //GetComponent<Renderer>().material.color = Color.magenta;

                _navMeshAgent.isStopped = true;

                _zoneManager.UpdateCompletion();
            }
        }
    }
}
