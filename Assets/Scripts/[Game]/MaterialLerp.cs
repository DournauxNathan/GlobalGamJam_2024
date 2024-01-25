using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;

public class MaterialLerp : MonoBehaviour
{
    private List<Material> _sadMaterials = new List<Material>();
    [SerializeField] private List<Material> _happyMaterials = new List<Material>();
    public float _lerpDuration = 1f;
    [SerializeField] private AnimationCurve _lerpCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    private bool _isLerping = false;

    // Slider de 0 à 1
    [SerializeField] [Range(0f, 1f)] private float _lerpValue = 0f;

    private MeshRenderer _meshRenderer;
    private Shader _standardShader;

    private bool _canLerp = true;
    ZoneManager _zoneManager;

    // Start is called before the first frame update
    void Start()
    {
        // On cherche le ZoneManager dans les parents du GameObject
        _zoneManager = gameObject.GetComponentInParent<ZoneManager>();

        _lerpValue = 0f;
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();

        // Récupérez les matériaux initiaux du Renderer
        _sadMaterials = _meshRenderer.materials.ToList();

        // Assurez-vous que les deux ensembles de matériaux ont la même longueur
        if (_sadMaterials.Count != _happyMaterials.Count)
        {
            Debug.LogError("Les ensembles de matériaux n'ont pas la même longueur.");
            _canLerp = false;
        }
        
        _standardShader = Shader.Find("Universal Render Pipeline/Lit");
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canLerp) { return; }

        if (_lerpValue != _zoneManager._completion && !_isLerping)
        {
            StartCoroutine(LerpMaterials());
        }
    }

    IEnumerator LerpMaterials()
    {
        _isLerping = true;

        // On fait une copie des matériaux du Renderer
        Material[] lerpedMaterials = new Material[_sadMaterials.Count];

        float elapsedTime = 0f;

        float startValue = _lerpValue;
        float endValue = _zoneManager._completion;

        while(elapsedTime < _lerpDuration)
        {
            elapsedTime += Time.deltaTime;

            _lerpValue = Mathf.Lerp(startValue, endValue, _lerpCurve.Evaluate(elapsedTime / _lerpDuration));

            for (int i = 0; i < _sadMaterials.Count; i++)
            {
                lerpedMaterials[i] = new Material(_standardShader);

                lerpedMaterials[i].Lerp(_sadMaterials[i], _happyMaterials[i], _lerpValue);
            }

            _meshRenderer.materials = lerpedMaterials;

            yield return null;
        }

        _isLerping = false;
    }
}
