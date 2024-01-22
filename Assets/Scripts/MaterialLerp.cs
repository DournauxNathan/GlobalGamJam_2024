using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialLerp : MonoBehaviour
{
    private List<Material> _sadMaterials = new List<Material>();
    [SerializeField] private List<Material> _happyMaterials = new List<Material>();

    // Slider de 0 à 1
    [SerializeField] [Range(0f, 1f)] private float _lerpValue = 0f;

    private MeshRenderer _meshRenderer;
    private Shader _standardShader;

    private bool _canLerp = true;


    // Start is called before the first frame update
    void Start()
    {
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

        LerpMaterials();
    }

    void LerpMaterials()
    {
        // On fait une copie des matériaux du Renderer
        Material[] lerpedMaterials = new Material[_sadMaterials.Count];

        for (int i = 0; i < _sadMaterials.Count; i++)
        {
            lerpedMaterials[i] = new Material(_standardShader);

            // Interpolez entre les matériaux de chaque ensemble
            lerpedMaterials[i].Lerp(
                _sadMaterials[i],
                _happyMaterials[i],
                _lerpValue
            );
        }

        // Appliquez les matériaux lerpés au Renderer du GameObject
        _meshRenderer.materials = lerpedMaterials;
    }
}
