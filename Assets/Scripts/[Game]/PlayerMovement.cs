using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkspeed = 5f;
    public float sprintSpeed = 10f;

    public bool isWakling;
    public bool isSprinting;

    [SerializeField] private Animator canAnimator;
    
    private float speed;
    private CharacterController controller;
    private Vector3 inputVector;
    private float gravityModifier = -10f;
    private Vector3 movement;

    void Start()
    {
        controller = GetComponent<CharacterController>();
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

        return movement = (input * speed) + (Vector3.up * gravityModifier);
    }

    void Move()
    {
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
}


