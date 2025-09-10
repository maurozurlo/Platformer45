using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerPlatformer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Jumping")]
    public float jumpHeight = 2f;
    public float minJumpHeight = 1f; // Minimum jump height when quickly releasing
    public float gravity = -9.81f;
    public int maxJumps = 2;
    public float coyoteTime = 0.2f;

    [Header("Variable Jump")]
    public float maxJumpTime = 0.35f; // Max time you can hold jump for full height
    public float jumpHoldForce = 25f; // Additional upward force while holding jump
    public float fallMultiplier = 2.5f; // Faster fall when moving down
    public float lowJumpMultiplier = 3f; // Even faster fall when releasing jump early

    [Header("Dashing")]
    public float dashSpeed = 15f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;

    private CharacterController controller;
    private Camera mainCamera;
    private Animator animator;

    private Vector3 velocity;
    private bool isGrounded;
    private int jumpCount;
    private float coyoteTimeCounter;
    private bool isJumping;
    private float jumpTimeCounter;

    private float currentSpeed;
    private bool isDashing = false;
    private float dashTimeCounter;
    private float dashCooldownCounter;
    private Vector3 dashDirection;

    private float horizontal,
        vertical;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        animator = GetComponentInChildren<Animator>();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        {
            transform.position = hit.point + Vector3.up * 0.01f;
        }
    }

    void Update()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpCount = 0;
            coyoteTimeCounter = coyoteTime;
            isJumping = false;
            jumpTimeCounter = 0f;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Handle dash cooldown
        if (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
        }

        // Handle dashing
        if (isDashing)
        {
            // Move character in dash direction
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            dashTimeCounter -= Time.deltaTime;

            if (dashTimeCounter <= 0)
            {
                isDashing = false;
            }
        }
        else
        {
            // Input
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

            currentSpeed = 0;
            if (inputDirection.magnitude >= 0.1f)
            {
                float targetAngle =
                    Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg
                    + mainCamera.transform.eulerAngles.y;
                float smoothAngle = Mathf.LerpAngle(
                    transform.eulerAngles.y,
                    targetAngle,
                    Time.deltaTime * rotationSpeed
                );
                transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir * moveSpeed * Time.deltaTime);
                currentSpeed = moveDir.magnitude;
            }

            // Jump initiation
            if (Input.GetButtonDown("Jump"))
            {
                if (coyoteTimeCounter > 0f || (jumpCount < maxJumps && !isGrounded))
                {
                    // Calculate initial jump velocity for minimum jump height
                    float minJumpVelocity = Mathf.Sqrt(minJumpHeight * -2f * gravity);
                    velocity.y = minJumpVelocity;

                    jumpCount++;
                    coyoteTimeCounter = 0f;
                    isJumping = true;
                    jumpTimeCounter = 0f;
                }
            }

            // Variable jump height - Mario style!
            if (isJumping && Input.GetButton("Jump") && jumpTimeCounter < maxJumpTime)
            {
                // Apply additional upward force while holding jump
                jumpTimeCounter += Time.deltaTime;

                // Calculate how much of the jump time we've used (0 to 1)
                float jumpProgress = jumpTimeCounter / maxJumpTime;

                // Apply diminishing force over time (stronger at beginning, weaker toward end)
                float forceMultiplier = 1f - (jumpProgress * jumpProgress); // Quadratic falloff
                velocity.y += jumpHoldForce * forceMultiplier * Time.deltaTime;

                // Cap the maximum velocity to prevent infinite height
                float maxJumpVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                velocity.y = Mathf.Min(velocity.y, maxJumpVelocity);
            }

            // Stop variable jump when button is released or max time reached
            if ((isJumping && !Input.GetButton("Jump")) || jumpTimeCounter >= maxJumpTime)
            {
                isJumping = false;
            }

            // Apply gravity with different multipliers
            if (velocity.y < 0)
            {
                // Falling - apply fall multiplier
                velocity.y += gravity * fallMultiplier * Time.deltaTime;
            }
            else if (velocity.y > 0 && !Input.GetButton("Jump"))
            {
                // Rising but jump button released - fall faster (short hop)
                velocity.y += gravity * lowJumpMultiplier * Time.deltaTime;
            }
            else
            {
                // Normal gravity (when rising and holding jump)
                velocity.y += gravity * Time.deltaTime;
            }

            // Dash initiation
            if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownCounter <= 0)
            {
                // Get the current movement direction
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
                Vector3 inputDirectionDash = new Vector3(horizontal, 0f, vertical).normalized;

                // If no input, dash in the direction the player is facing
                if (inputDirectionDash.magnitude < 0.1f)
                {
                    dashDirection = transform.forward;
                }
                else
                {
                    // Dash in the direction of input
                    float targetAngle =
                        Mathf.Atan2(inputDirectionDash.x, inputDirectionDash.z) * Mathf.Rad2Deg
                        + mainCamera.transform.eulerAngles.y;
                    dashDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                }

                isDashing = true;
                dashTimeCounter = dashTime;
                dashCooldownCounter = dashCooldown;
                velocity.y = 0; // Cancel any vertical velocity while dashing
            }

            controller.Move(velocity * Time.deltaTime);
        }

        // Animator
        if (animator != null)
        {
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalVelocity", velocity.y);
            animator.SetFloat("InputVertical", currentSpeed, 0.1f, Time.deltaTime);
            animator.SetFloat("InputHorizontal", horizontal, 0.1f, Time.deltaTime);
            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("IsDashing", isDashing);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize jump heights in scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * minJumpHeight, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * jumpHeight, 0.1f);
    }
}
