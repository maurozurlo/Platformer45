using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollHead : MonoBehaviour
{
    public float speed;
    public float maxSpeed = 40f;
    public float jumpForce = 10f;
    public int maxJumps = 2; // Maximum number of jumps including the initial jump
    public LayerMask groundLayer;
    public float debugLineLength = 0.1f;
    bool isGrounded = false;

    private Rigidbody rb;
    private int jumpsRemaining; // Number of jumps left

    private Quaternion cameraRotation = Quaternion.identity; // 🌐 Starts aligned with world axes

    void OnEnable()
    {
        CamControl.OnCameraRotated += HandleCameraRotated;
    }

    void OnDisable()
    {
        CamControl.OnCameraRotated -= HandleCameraRotated;
    }

    private void HandleCameraRotated(Quaternion rotation)
    {
        cameraRotation *= rotation; // Accumulate rotation
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        // Check for jump input
        if (Input.GetKeyDown(KeyCode.Keypad0) && jumpsRemaining > 0)
        {
            Jump();
        }

        isGrounded = CheckGroundCollision();
    }

    void FixedUpdate()
    {
        float moveHorizontal = -Input.GetAxis("Horizontal2");
        float moveVertical = Input.GetAxis("Vertical2");

        Vector3 rawInput = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 movement = cameraRotation * rawInput; // 🎯 Rotate input to match camera

        if (movement.magnitude > 0.01f)
        {
            rb.AddForce(movement * speed);

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }
        }
        else
        {
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            Vector3 easedVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, 0.1f);
            rb.velocity = new Vector3(easedVelocity.x, rb.velocity.y, easedVelocity.z);
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, 0.1f);
        }
    }

    void Jump()
    {
        if (jumpsRemaining == maxJumps && isGrounded) // Initial jump
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else // Double jump
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Clear vertical velocity
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        jumpsRemaining--;
    }

    bool CheckGroundCollision()
    {
        float raycastDistance = 1.5f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            jumpsRemaining = maxJumps;
            return true;
        }
        return false;
    }
}
