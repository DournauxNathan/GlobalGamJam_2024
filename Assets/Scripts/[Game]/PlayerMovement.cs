using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator canAnimator;

    [Header("MOVEMENT")]
    public float walkspeed = 5f;
    public float sprintSpeed = 10f;
    private float speed;
    private float gravityModifier = -10f;

    [Header("HEALTH")]
    public float maxHealth;
    private float currentHealth;

    private bool isWakling;
    private bool isSprinting;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        //UIManager.
    }

    void Update()
    {
        GetInput();
        Move();
        HeadBobing();

        canAnimator.SetBool("Walk", isWakling);
    }

    Vector3 GetInput()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        input.Normalize();
        input = transform.TransformDirection(input);

        return (input * speed) + (Vector3.up * gravityModifier);
    }

    void Move()
    {
        Vector3 movement = GetInput() * Time.deltaTime;

        controller.Move(movement * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
            speed = sprintSpeed;
        }
        else
        {
            isSprinting = false;
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
        /*// Si c'est un tag "Projectile"
        if (collision.collider.gameObject.CompareTag("Projectile") && !_isDead)
        {
            // On détruit le projectile
            Destroy(collision.collider.gameObject);

            _hp--;

            if (_hp <= 0)
            {
                // On change sa couleur pour du rose
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.magenta;

                _mrSmithMovement._navMeshAgent.SetDestination(transform.position);

                _zoneManager.UpdateCompletion();

                _isDead = true;
                _mrSmithMovement._animator.SetBool("isDead", true);
                _mrSmithMovement._animator.SetFloat("velocity", 0f);
            }
        }*/
    }
}


