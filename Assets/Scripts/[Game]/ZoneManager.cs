using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [Range(0f, 1f)] public float _completion = 0f;
    private List<MrSmith> _mrSmiths = new List<MrSmith>();
    private int _nbMrSmiths = 0;

    public Bounds _zoneBounds;
    public string _districtName;

    private Player _player;

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
        _mrSmiths.Add(
            mrSmith
        );
        _nbMrSmiths++;
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

        UIManager.Instance?.SetDistrictInfo(_districtName, _completion);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerController>()._currentZone = this;

            UIManager.Instance?.SetDistrictInfo(_districtName, _completion);
        }
    }
}
