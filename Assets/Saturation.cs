using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Saturation : MonoBehaviour
{
    [Range(0f, 1f)] public float _saturation = 0f;
    private List<MeshRenderer> _meshRenderers = new List<MeshRenderer>();
    ZoneManager _zoneManager;
    [SerializeField] private AnimationCurve _lerpSaturationCurve;


    private void Awake()
    {
        if (gameObject.GetComponent<MeshRenderer>() != null)
        {
            _meshRenderers.Add(gameObject.GetComponent<MeshRenderer>());
        } else
        {
            _meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
            Debug.Log("_meshRenderer found in children : " + _meshRenderers);
        }

        _zoneManager = gameObject.GetComponentInParent<ZoneManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Shader saturationShader = Shader.Find("Shader Graphs/ObjectLitGrayScale");

        for (int j = 0; j < _meshRenderers.Count; j++)
        {
            // On récupère les matériaux du MeshRenderer
            Material[] materials = _meshRenderers[j].materials;

            // On remplace le shader de tous les matériaux par le shader "Shader Graphs/ObjectGrayScaleGraph"
            for (int i = 0; i < _meshRenderers[j].materials.Length; i++)
            {
                materials[i] = new Material(saturationShader);
                materials[i].SetColor("_Color", _meshRenderers[j].materials[i].GetColor("_Color"));
                materials[i].SetTexture("_MainTex", _meshRenderers[j].materials[i].GetTexture("_MainTex"));
            }

            _meshRenderers[j].materials = materials;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_saturation != _zoneManager._completion)
        {
            StartCoroutine(ChangeSaturation());
        }
        
    }

    IEnumerator ChangeSaturation()
    {
        float time = 0f;
        float duration = 0.8f;

        float startSaturation = _saturation;
        float endSaturation = _zoneManager._completion;

        if (gameObject.name == "Building_J_prefab")
        {
            Debug.Log("_meshRenderers : ");
            foreach (MeshRenderer meshRenderer in _meshRenderers)
            {
                Debug.Log(meshRenderer.name);
            }
        }

        while (time < duration)
        {
            _saturation = Mathf.Lerp(startSaturation, endSaturation, _lerpSaturationCurve.Evaluate(time / duration));

            for (int i = 0; i < _meshRenderers.Count; i++)
            {
                for (int j = 0; j < _meshRenderers[i].materials.Length; j++)
                {
                    _meshRenderers[i].materials[j].SetFloat("_Saturation", _saturation);
                }
            }

            time += Time.deltaTime;

            yield return null;
        }

        /*_saturation = endSaturation;
        for (int i = 0; i < _meshRenderers.Count; i++)
        {
            _meshRenderers[i].material.SetFloat("_Saturation", _saturation);
        }*/
        // _meshRenderer.material.SetFloat("_Saturation", _saturation);
    }
}
