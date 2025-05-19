using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unity.Cinemachine;

public class Player_Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float jumpForce = 5f;

    [Header("Ground Check Settings")]
    public float raycastLength = 1.1f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private float moveSpeed;
    private Vector2 input;
    private bool isGrounded;
    bool isRunning = false;
    [SerializeField] CinemachineCamera fpsCamera;

    //

    Manager_Collector managerCollector;
    Audio_Manager audioManager;
    Fps_Camera_Handler fpsCameraHandler;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked; // Hide and lock the cursor = CursorLockMode.Locked;

        managerCollector = GameObject.FindWithTag("ManagerCollector").GetComponent<Manager_Collector>();
        audioManager = managerCollector.audioManager;
        fpsCameraHandler = managerCollector.fpsCameraHandler;

    }

    private void Update()
    {
        // Movement input
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // Stand
        if (input.x == 0 || input.y == 0)
        {
            fpsCameraHandler.SetHeadBop_Stand();
        }

        // Walk
        if ((input.x != 0 || input.y != 0) && isGrounded && !isRunning)
        {
            fpsCameraHandler.SetHeadBop_Walk();
            audioManager.PlayWalkSound();
        }
        else
        {
            audioManager.StopWalkSound();
        }

        // Raycast ground check from player’s position straight down
        isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastLength, groundMask);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            audioManager.PlayJumpSound();
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && (input.x != 0 || input.y != 0))
        {
            fpsCameraHandler.SetHeadBop_Run();
            audioManager.PlayRunSound();
            isRunning = true;
        }
        else
        {
            audioManager.StopRunSound();
            isRunning = false;
        }

        // Deciding movespeed
        moveSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        FollowCameraRotation();
    }

    private void FixedUpdate()
    {
        // Convert input to movement based on player rotation
        Vector3 move = transform.right * input.x + transform.forward * input.y;

        Vector3 targetVelocity = move.normalized * moveSpeed;
        Vector3 currentVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Vector3 velocityChange = targetVelocity - currentVelocity;

        // Apply velocity change for smooth movement
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    // Optional: visualize the raycast in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastLength);
    }

    void FollowCameraRotation()
    {
        // Get the camera's current Y rotation (yaw)
        float yRotation = fpsCamera.transform.eulerAngles.y;

        // Apply the camera's Y rotation to the player, while keeping the player's X and Z rotation the same
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
    public void StopPlayer()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isRunning = false;
        audioManager.StopWalkSound();
        audioManager.StopRunSound();
    }
}
