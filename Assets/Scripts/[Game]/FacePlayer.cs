using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private void LateUpdate()
    {
        // Ensure the Camera.main is not null before trying to access it
        if (Camera.main != null)
        {
            // Rotate the canvas to face the player
            transform.LookAt(Camera.main.transform.position, -Vector3.up);

        }
    }
}