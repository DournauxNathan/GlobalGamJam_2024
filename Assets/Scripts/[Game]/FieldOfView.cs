using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    private float fov;
    private float viewDistance;
    private Vector3 origin;
    private float startingAngle;
    public Collider hitCollider;

    public bool playerDetected = false;

    public Transform player;

    private void Start()
    {
        
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > 0)
        {

        }

        if (playerDetected)
        {
        }
    }


}