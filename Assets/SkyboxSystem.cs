using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using static System.TimeZoneInfo;

public class SkyboxSystem : MonoBehaviour
{
    public float _transitionTime = 2f;

    public PlayerController _playerController;

    [Range(0f, 1f)] public float testCompletion = 0f;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox.SetFloat("_t", _playerController._currentZone._completion);
        DynamicGI.UpdateEnvironment();
    }

    // Update is called once per frame
    void Update()
    {

        // RenderSettings.skybox = Mathf.Lerp(_skyboxSad, _skyboxHappy, testCompletion);// _playerController._currentZone._completion);
        // LerpSkyboxMaterials(_skyboxSad, _skyboxHappy, testCompletion);// _playerController._currentZone._completion);
        /*if (testCompletion < 1)
        {
            testCompletion += Time.deltaTime / _transitionTime;
            RenderSettings.skybox.Lerp(_skyboxSad, _skyboxHappy, testCompletion);
            DynamicGI.UpdateEnvironment(); // Mettez à jour l'éclairage global si nécessaire
        }*/
    }

    public void LerpSkybox()
    {
        float elapsedTime = 0f;

        float startT = RenderSettings.skybox.GetFloat("_t");
        float endT = _playerController._currentZone._completion;

        while (elapsedTime < _transitionTime)
        {
            float t = elapsedTime / _transitionTime;

            RenderSettings.skybox.SetFloat("_t", Mathf.Lerp(startT, endT, t));

            elapsedTime += Time.deltaTime;
        }

        RenderSettings.skybox.SetFloat("_t", endT);

        DynamicGI.UpdateEnvironment();
    }
}
