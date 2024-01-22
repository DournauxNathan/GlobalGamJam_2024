using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [Range(0f, 1f)] public float _completion = 0f;

    public Bounds _zoneBounds;

    private void Awake()
    {
        // On récupère la boundingBox de la zone en cherchant dans ses enfants le composant BoxCollider
        foreach (BoxCollider boxCollider in gameObject.GetComponentsInChildren<BoxCollider>())
        {
            if (_zoneBounds == null)
            {
                _zoneBounds = boxCollider.bounds;
            }
            else
            {
                _zoneBounds.Encapsulate(boxCollider.bounds);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateCompletion()
    {
        _completion = 0f;
    }
}
