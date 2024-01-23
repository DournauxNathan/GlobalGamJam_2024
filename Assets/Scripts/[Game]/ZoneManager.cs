using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [Range(0f, 1f)] public float _completion = 0f;
    private List<MrSmith> _mrSmiths = new List<MrSmith>();
    private int _nbMrSmiths = 0;

    public BoxCollider _boxCollider;
    public string _districtName;

    private Player _player;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
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
            collider.GetComponent<Player>()._currentZone = this;

            UIManager.Instance?.SetDistrictInfo(_districtName, _completion);
        }
    }
}
