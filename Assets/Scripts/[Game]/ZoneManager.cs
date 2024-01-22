using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [Range(0f, 1f)] public float _completion = 0f;
    private List<MrSmith> _mrSmiths = new List<MrSmith>();
    private int _nbMrSmiths = 0;

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

    public void AddMrSmith(MrSmith mrSmith)
    {
        Debug.Log("Add MrSmith : " + mrSmith.name);
        _mrSmiths.Add(
            mrSmith
        );
        _nbMrSmiths++;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCompletion()
    {
        int totalAgents = _mrSmiths.Count;
        int deadAgents = 0;
        foreach (MrSmith mrSmith in _mrSmiths)
        {
            if (mrSmith._hp <= 0)
            {
                deadAgents++;
            }
        }

        _completion = (float)deadAgents / (float)totalAgents;
    }
}
