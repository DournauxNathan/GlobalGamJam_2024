using System;
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
        if (_zoneManager != null)
        {
            _zoneManager.AddSaturationChildren(this);
        }

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
                materials[i].SetFloat("_Saturation", 0f);
            }

            _meshRenderers[j].materials = materials;
        }
    }

    public void ChangeSaturation(float to = -1f)
    {
        float endSaturation = (to == -1f) ? _zoneManager._completion : to;

        for (int i = 0; i < _meshRenderers.Count; i++)
        {
            for (int j = 0; j < _meshRenderers[i].materials.Length; j++)
            {
                // _meshRenderers[i].materials[j].SetFloat("_Saturation", endSaturation);
                StartCoroutine(LerpSaturation(_meshRenderers[i].materials[j], endSaturation));
            }
        }

        _saturation = endSaturation;

        if (endSaturation == 1)
        {
            _stopSaturation = true;
        }
    }

    private IEnumerator LerpSaturation(Material material, float saturation)
    {
        float elapsedTime = 0f;
        float duration = 1f;

        float startSaturation = _saturation;
        float endSaturation = saturation;

        float halfBlendColorTextEnd = 1f;
        float startBlendColorText = 0f;

        bool goDown = false;
        while (elapsedTime < duration)
        {
            // On lerp la couleur blended avec la texture sur la première moitié de la durée
            if (elapsedTime < (duration / 2f))
            {
                material.SetFloat("_BlendColorText", Mathf.Lerp(startBlendColorText, halfBlendColorTextEnd, _lerpSaturationCurve.Evaluate(elapsedTime / duration)));
            }
            // On revient à la couleur de base sur la deuxième moitié de la durée
            else
            {
                if (!goDown)
                {
                    material.SetFloat("_Saturation", endSaturation);
                    goDown = true;
                }

                material.SetFloat("_BlendColorText", Mathf.Lerp(halfBlendColorTextEnd, startBlendColorText, _lerpSaturationCurve.Evaluate(elapsedTime / duration)));
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        material.SetFloat("_Saturation", endSaturation);
        material.SetFloat("_BlendColorText", startBlendColorText);

        yield return null;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _meshRenderers.Count; i++)
        {
            for (int j = 0; j < _meshRenderers[i].materials.Length; j++)
            {
                _meshRenderers[i].materials[j].SetFloat("_Saturation", 0f);
            }
        }
    }

    public void SetSaturation(float saturation)
    {
        ChangeSaturation(saturation);
    }
}
