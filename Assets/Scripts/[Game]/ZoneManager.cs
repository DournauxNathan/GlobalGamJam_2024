using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ZoneManager : MonoBehaviour
{
    [Range(0f, 1f)] public float _completion = 0f;
    private List<MrSmith> _mrSmiths = new List<MrSmith>();

    [HideInInspector] public BoxCollider _boxCollider;
    public string _districtName;

    public UnityEvent _onCompletionChange;
    bool updateMusic = false;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        UIManager.Instance.SubscribeZoneManager(this);
    }

    public void AddMrSmith(MrSmith mrSmith)
    {
        _mrSmiths.Add(
            mrSmith
        );
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


        if (deadAgents == totalAgents)
        {
            UIManager.Instance.DistrictComplete();
        }

        UIManager.Instance?.SetDistrictInfo(_districtName, _completion);

        // On trigger l'event de completion de la zone
        _onCompletionChange.Invoke();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerController>()._currentZone = this;

            UIManager.Instance?.SetDistrictInfo(_districtName, _completion);

            if (_completion < 1 && UIManager.Instance.updateMusic)
            {
                UIManager.Instance.updateMusic = false;
                UIManager.zoneCleared?.Invoke();
            }
            else if (_completion >= 1 && !UIManager.Instance.updateMusic)
            {
                UIManager.Instance.updateMusic = true;
                UIManager.zoneCleared?.Invoke();
            }

            _onCompletionChange.Invoke();
        }
    }
}
