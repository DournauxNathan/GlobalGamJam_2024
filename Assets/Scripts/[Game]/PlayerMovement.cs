using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool isWakling;

    [SerializeField] private Animator canAnimator;
    
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


