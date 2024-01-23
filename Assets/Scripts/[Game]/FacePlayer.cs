using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private void Update()
    {
        // Ensure the Camera.main is not null before trying to access it
        if (Camera.main != null)
        {
            // Rotate the canvas to face the player
            transform.LookAt(Camera.main.transform.position, Vector3.up);

            // Optionally, you can constrain the rotation to only affect the y-axis
            // transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }
}