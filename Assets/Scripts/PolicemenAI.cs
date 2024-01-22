using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

// Custom editor for the AIController script in the Unity Editor
[CustomEditor(typeof(PolicemenAI))]
public class FOVEditor : Editor
{
    void OnSceneGUI()
    {
        PolicemenAI fov = (PolicemenAI)target;

        // Store the AI's position
        Vector3 fromPosition = fov.eyes.position;

        // Calculate the endpoint of the FOV cone
        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
        Vector3 toPositionA = fromPosition + viewAngleA * fov.viewDistance;

        // Draw the filled cone to represent FOV
        Handles.color = new Color(1f, 0f, 0f, .5f); // Red with transparency
        Handles.DrawSolidArc(fromPosition, Vector3.up, viewAngleA, fov.viewAngle, fov.viewDistance);

        // Optionally, you can also draw a line to the endpoint for reference
        Handles.DrawLine(fromPosition, toPositionA);
    }
}

public class PolicemenAI : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    public bool _isMoving = false;
    private Vector3 _destination;
    private ZoneManager _zoneManager;
    public float _waitDuration;

    // Field of View (FOV) parameters
    [Header("Field Of View Parameters")]
    public Transform eyes;
    public LayerMask layerMask;
    [Space(5)]
    [Range(0f, 360f)] public float viewAngle;
    public float viewDistance;

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

    // Update is called once per frame
    void Update()
    {
        if (!_isMoving && !isPlayerOnSight)
        {
            SetRandomDestinationInBounds();
            StartCoroutine(MoveToDestination());
        }
    }

    private void DetectPlayer()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewDistance, layerMask);

        foreach (Collider target in targetsInViewRadius)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                isPlayerOnSight = true;

                // The target is within the AI's field of view
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                // You can add more conditions here to filter the targets, e.g., check if the target is visible, within a certain distance, etc.

                // React to the target
                // For example, you might want to follow or attack the target.

                // If the Target is a Hiding Spot && if its position is not in the hiding list
                if (target.CompareTag("Player"))
                {
                    FollowTarget(target.transform.position);
                }
            }
        }
    }

    private void FollowTarget(Vector3 position)
    {
        if (isPlayerOnSight)
        {
            isPlayerOnSight = false;
            _navMeshAgent.SetDestination(position);
        }
        else
        {

        }
    }

    private IEnumerator MoveToDestination()
    {
        _isMoving = true;
        DetectPlayer();
        _navMeshAgent.SetDestination(_destination);
        while (_navMeshAgent.velocity.Equals(Vector3.zero))
        {
            yield return null;
        }

        // Wait
        yield return new WaitForSeconds(_waitDuration);

        _isMoving = false;
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
