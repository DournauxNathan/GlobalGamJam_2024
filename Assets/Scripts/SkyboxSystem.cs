using UnityEngine;

public class SkyboxSystem : MonoBehaviour
{
    public float _transitionTime = 2f;

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
        if (_playerController._currentZone._completion == 0.5f || _playerController._currentZone._completion == 1f)
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
}
