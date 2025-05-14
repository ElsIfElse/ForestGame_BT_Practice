using UnityEngine;

public class Player_Camera : MonoBehaviour
{
 [Header("Mouse Settings")]
    public float sensitivity = 100f;
    public Transform playerBody;

    private float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Hide and lock the cursor
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Vertical look
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent flipping over

        // Apply rotations
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Camera up/down
        playerBody.Rotate(Vector3.up * mouseX);                        // Body left/right
    }
}
