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
    private bool _stopSaturation = false;


    private void Awake()
    {
        if (gameObject.GetComponent<MeshRenderer>() != null)
        {
            _meshRenderers.Add(gameObject.GetComponent<MeshRenderer>());
        } else
        {
            _meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
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
        if (_zoneManager != null && _saturation != _zoneManager._completion && !_stopSaturation)
        {
            StartCoroutine(ChangeSaturation());
        }
        
    }

    IEnumerator ChangeSaturation(float to = -1f)
    {
        /*float time = 0f;
        float duration = 0.8f;*/

        float startSaturation = _saturation;
        float endSaturation = (to == -1f) ? _zoneManager._completion : to;

        Debug.Log("Changement saturation de " + startSaturation + " à " + endSaturation);

        for (int i = 0; i < _meshRenderers.Count; i++)
        {
            for (int j = 0; j < _meshRenderers[i].materials.Length; j++)
            {
                _meshRenderers[i].materials[j].SetFloat("_Saturation", endSaturation);
            }
        }

        yield return null;

        if (endSaturation == 1)
        {
            _stopSaturation = true;
        }
        

        /*while (time < duration)
        {
            _saturation = Mathf.Lerp(startSaturation, endSaturation, _lerpSaturationCurve.Evaluate(time / duration));

            for (int i = 0; i < _meshRenderers.Count; i++)
            {
                for (int j = 0; j < _meshRenderers[i].materials.Length; j++)
                {
                    _meshRenderers[i].materials[j].SetFloat("_Saturation", _saturation);
                }

                yield return null;
            }

            time += Time.deltaTime;

            yield return null;
        }*/
    }

    public void SetSaturation(float saturation)
    {
        Debug.Log("Changement saturation à " + saturation + " demandé");
        StartCoroutine(ChangeSaturation(saturation));
    }
}
