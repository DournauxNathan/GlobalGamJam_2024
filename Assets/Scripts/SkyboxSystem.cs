using UnityEngine;
using System.Collections;

public class SkyboxSystem : MonoBehaviour
{
    public float _transitionTime = 0.8f;

    public PlayerController _playerController;

    [Range(0f, 1f)] public float testCompletion = 0f;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox.SetFloat("_t", 0);
        DynamicGI.UpdateEnvironment();
    }

    public void LerpSkybox()
    {
        float targetCompletion = 0f;
        if (_playerController._currentZone._completion > 0 && _playerController._currentZone._completion < 0.5f)
        {
            targetCompletion = 0.5f;
        }
        else if (_playerController._currentZone._completion == 1f)
        {
            targetCompletion = 1f;
        }

        if (targetCompletion != RenderSettings.skybox.GetFloat("_t"))
        {
            StartCoroutine(LerpSkyboxCoroutine(targetCompletion));
        }
    }

    IEnumerator LerpSkyboxCoroutine(float targetCompletion)
    {
        float elapsedTime = 0f;

        float startT = RenderSettings.skybox.GetFloat("_t");
        float endT = _playerController._currentZone._completion;

        while (elapsedTime < _transitionTime)
        {
            float t = elapsedTime / _transitionTime;

            RenderSettings.skybox.SetFloat("_t", Mathf.Lerp(startT, endT, t));

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        RenderSettings.skybox.SetFloat("_t", endT);

        DynamicGI.UpdateEnvironment();

        yield return null;
    }
}
