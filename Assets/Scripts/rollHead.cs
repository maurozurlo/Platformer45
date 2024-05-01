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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        // Check for jump input
        if (Input.GetButtonDown("Jump") && jumpsRemaining > 0)
        {
            Jump();
        }

        isGrounded = CheckGroundCollision();
    }

    void FixedUpdate()
    {
        float moveHorizontal = -Input.GetAxis("Horizontal2");
        float moveVertical = Input.GetAxis("Vertical2");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
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
