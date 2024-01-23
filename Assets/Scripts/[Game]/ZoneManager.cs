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

    [SerializeField] private TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (_player._currentZone == this)
        {
            Debug.Log("On met à jour l'UI !");
            _textMeshPro.text = "District : " + _districtName + " (" + Mathf.Floor(_completion) * 100 + "%)";
        }

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

        if (_player._currentZone == this)
        {
            _textMeshPro.text = "District : " + _districtName + " (" + ((int)Mathf.Round(_completion * 100f)) + "%)";
            Debug.Log("On met à jour l'UI (fin d'un agent) : " + _districtName + " = " + _completion);
        }
        else
        {
            Debug.Log("Le player n'est pas dans la zone : " + _districtName);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Player>()._currentZone = this;

            _textMeshPro.text = "District : " + _districtName + " (" + ((int)Mathf.Round(_completion * 100f)) + "%)";

            Debug.Log("On met à jour l'UI (changement de zone) : " + _districtName + " = " + _completion);
        }
    }
}
