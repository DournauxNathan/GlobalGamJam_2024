using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MrSmithMovement : MonoBehaviour
{
    public NavMeshAgent _navMeshAgent;
    public bool _isMoving = false;
    public Vector3 _destination;
    private MrSmith _mrSmith;
    public float _waitDuration;
    private ZoneManager _zoneManager;

    private void Awake()
    {
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _mrSmith = gameObject.GetComponent<MrSmith>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _zoneManager = _mrSmith._zoneManager;

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
    void Update() {
        if (!_isMoving && _mrSmith._hp > 0)
        {
            SetRandomDestinationInBounds();

            StartCoroutine(MoveToDestination());
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
}
