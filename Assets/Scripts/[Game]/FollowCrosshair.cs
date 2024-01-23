using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCrosshair : MonoBehaviour
{
    void Update()
    {
        // Get the screen position of the crosshair
        Vector3 screenPos = UIManager.Instance.crosshair.position;

        // Convert the screen position to a world position
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));

        // Calculate the direction from the gun to the crosshair
        Vector3 directionToCrosshair = worldPos - transform.position;

        // Calculate the rotation to face the crosshair
        Quaternion rotationToCrosshair = Quaternion.LookRotation(directionToCrosshair, Vector3.up);

        // Smoothly rotate the gun towards the crosshair
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToCrosshair, UIManager.Instance.crossHairFollowFactor * Time.deltaTime);
    }
}