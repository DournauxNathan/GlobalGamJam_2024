using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 1.5f;
    public float smoothing = 10f;

    private float smoothedMousePos;
    private float currentLookingPos;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get input
        float mouseX = Input.GetAxis("Mouse X");

        // Modifiy input
        mouseX *= sensitivity * smoothing;
        smoothedMousePos = Mathf.Lerp(smoothedMousePos, mouseX, 1f / smoothing);

        //Rotate Player
        currentLookingPos += smoothedMousePos;
        transform.localRotation = Quaternion.AngleAxis(currentLookingPos, transform.up);
    }
}

