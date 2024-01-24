using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ZoneManager _currentZone;

    [SerializeField] private Animator _animator;
    private CharacterController controller;

    [Header("MOVEMENT")]
    public float walkspeed = 5f;
    public float sprintSpeed = 10f;
    private float speed;
    private float gravityModifier = -10f;
    private Vector3 movement;

    [Header("HEALTH")]
    public float maxHealth;
    private float currentHealth;

    private bool isWakling;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        UIManager.Instance.SetSlider(maxHealth, currentHealth);
    }

    void Update()
    {
        GetInput();
        Move();
        HeadBobing();

        _animator.SetBool("Walk", isWakling);
    }

    Vector3 GetInput()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        input.Normalize();
        input = transform.TransformDirection(input);
        Vector3 movement = (input * speed) + (Vector3.up * gravityModifier);

        return movement;
    }

    void Move()
    {
        Vector3 movement = GetInput() * Time.deltaTime;
        
        controller.Move(movement);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkspeed;
        }
    }

    void HeadBobing()
    {
        if (controller.velocity.magnitude > 0.1f)
        {
            isWakling = true;
        }
        else
        {
            isWakling = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si c'est un tag "Projectile"
        if (collision.collider.gameObject.CompareTag("ProjectilePolicemen"))
        {
            // On détruit le projectile
            Destroy(collision.collider.gameObject);

            currentHealth--;
            UIManager.Instance.UpdateSlider(currentHealth);

            if (currentHealth <= 0)
            {
                Debug.LogWarning("GAME OVER");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si c'est un tag "Projectile"
        if (other.CompareTag("ProjectilePolicemen"))
        {
            Debug.LogWarning("");

            // On détruit le projectile
            Destroy(other.gameObject);

            currentHealth--;
            UIManager.Instance.UpdateSlider(currentHealth);

            if (currentHealth <= 0)
            {
                Debug.LogWarning("GAME OVER");
            }
        }
    }
}


