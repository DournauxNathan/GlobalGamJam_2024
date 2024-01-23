using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    public ZoneManager _currentZone;
    public VisualEffect _rainEffect;

    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateRain()
    {
        Debug.Log("Update rain !");
        if (_rainEffect != null)
        {
            Debug.Log("Rain effect modifié (" + _currentZone.name + ") : " + Mathf.Abs(_currentZone._completion * 800 - 800));
            _rainEffect.SetFloat("Rain Intensity", Mathf.Abs(_currentZone._completion * 800 - 800));
        } else
        {
            Debug.Log("Rain effect est introuvable !");
        }
        
    }
}
