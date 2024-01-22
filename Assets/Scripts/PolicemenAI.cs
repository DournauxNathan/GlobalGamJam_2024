using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class PolicemenAI : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    public bool _isMoving = false;
    private Vector3 _destination;
    private ZoneManager _zoneManager;
    public float _waitDuration;

    public LayerMask playerLayer;

    public float detectionRadius = 5f;

    private bool isPlayerOnSight;

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
    void Update()
    {
        DetectPlayer();

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
            Random.Range(_zoneManager._zoneBounds.min.x, _zoneManager._zoneBounds.max.x),
            transform.position.y,
            Random.Range(_zoneManager._zoneBounds.min.z, _zoneManager._zoneBounds.max.z)
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
        if (isPlayerOnSight)
        {
            _navMeshAgent.SetDestination(position);
        }
    }

    private IEnumerator MoveToDestination()
    {
        _isMoving = true;

        _navMeshAgent.SetDestination(_destination);

        while (_navMeshAgent.velocity.Equals(Vector3.zero))
        {
            yield return null;
        }

        // Wait
        yield return new WaitForSeconds(_waitDuration);

        _isMoving = false;
    }

    void DetectPlayer()
    {
        // OverlapSphere to detect colliders within the specified radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        // Check if any colliders belong to the player layer
        if (colliders.Length > 0)
        {
            if (Vector3.Distance(transform.position, colliders[0].transform.position) <= detectionRadius)
            {
                isPlayerOnSight = true;
                FollowTarget(colliders[0].transform.position);
            }
            else
            {
                isPlayerOnSight = false;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a wire sphere in the editor for better visualization
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
}
