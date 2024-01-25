using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float smoothing = 10f;

    private float smoothedMousePos;
    private float smoothedMousePosY;
    private float currentLookingPos;
    private float currentLookingPosY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float sensitivity = SettingsManager.Instance.mouseSensitivity;

        // Get input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Modifiy input
        mouseX *= sensitivity * smoothing;

        // Modify input
        mouseX *= sensitivity * smoothing;
        mouseY *= sensitivity * smoothing;

        smoothedMousePos = Mathf.Lerp(smoothedMousePos, mouseX, 1f / smoothing);
        smoothedMousePosY = Mathf.Lerp(smoothedMousePosY, mouseY, 1f / smoothing);

        // Rotate Player on the X-axis (horizontal)
        currentLookingPos += smoothedMousePos;
        transform.localRotation = Quaternion.AngleAxis(currentLookingPos, Vector3.up);

        // Rotate Camera on the Y-axis (vertical)
        currentLookingPosY += smoothedMousePosY;
        currentLookingPosY = Mathf.Clamp(currentLookingPosY, -90f, 90f); // Limit vertical rotation to avoid flipping

        Camera.main.transform.localRotation = Quaternion.AngleAxis(-currentLookingPosY, Vector3.right); // Invert the rotation for the camera
    }
}

