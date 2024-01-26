using UnityEngine;

public class MrSmith : MonoBehaviour
{
    public ZoneManager _zoneManager;
    private MrSmithMovement _mrSmithMovement;
    public int _hp = 3;
    private bool _isDead = false;
    public string projectileTag;
    [SerializeField] private GameObject _deathVFX;
    [SerializeField] private GameObject _hitVFX;

    private void Awake()
    {
        _zoneManager = gameObject.GetComponentInParent<ZoneManager>();
        _mrSmithMovement = gameObject.GetComponent<MrSmithMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _zoneManager.AddMrSmith(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si c'est un tag "Projectile"
        if (collision.gameObject.CompareTag(projectileTag) && !_isDead)
        {
            // On détruit le projectile
            Destroy(collision.collider.gameObject);

            Instantiate(_hitVFX, transform.position, Quaternion.identity);

            _hp--;

            if (_hp <= 0)
            {
                // On change sa couleur pour du rose
                UIManager.Instance._soundManager.PlaySmithConversionSound();
                UIManager.Instance._soundManager.PlayPoofSound();
                Instantiate(_deathVFX, transform.position, Quaternion.identity);

                _mrSmithMovement._navMeshAgent.isStopped = true;

                GetComponentInChildren<Canvas>().gameObject.SetActive(false);
                _zoneManager.UpdateCompletion();

                _isDead = true;
                _mrSmithMovement._animator.SetBool("isDead", true);
                _mrSmithMovement._animator.SetFloat("velocity", 0f);
            }
        }
    }
}
