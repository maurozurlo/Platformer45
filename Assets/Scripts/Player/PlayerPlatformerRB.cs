using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerPlatformerRB : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float groundDrag = 5f; // Helps stop character quickly when input stops

    [Header("Jumping")]
    public float jumpHeight = 2f;
    public float minJumpHeight = 1f;
    public float gravity = -9.81f;
    public int maxJumps = 2;
    public float coyoteTime = 0.2f;

    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public float groundCheckOffset = 1.1f; // Adjust based on collider height
    public float groundCheckRadius = 0.3f;

    [Header("Variable Jump")]
    public float maxJumpTime = 0.35f;
    public float jumpHoldForce = 25f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 3f;

    [Header("Dashing")]
    public float dashSpeed = 15f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody rb;
    private Camera mainCamera;
    private Animator animator;

    // Inputs
    private float horizontal;
    private float vertical;
    private bool jumpInputDown;
    private bool jumpInputHeld;
    private bool dashInput;

    // Logic States
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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        animator = GetComponentInChildren<Animator>();

        // RB Setup for Platformer feel
        rb.useGravity = false; // We apply custom gravity
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Keep upright
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smooth visuals
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        // 1. Process Input in Update (to not miss button presses)
        HandleInput();

        // 2. Timers
        HandleTimers();

        // 3. Animation
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        // 1. Check Ground
        CheckGround();

        // 2. Physics Logic
        if (isDashing)
        {
            HandleDashPhysics();
        }
        else
        {
            HandleMovementPhysics();
            HandleGravityAndJumpPhysics();
        }
    }

    private void HandleInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump")) jumpInputDown = true;
        jumpInputHeld = Input.GetButton("Jump");

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownCounter <= 0) dashInput = true;
    }

    private void HandleTimers()
    {
        // Dash Cooldown
        if (dashCooldownCounter > 0) dashCooldownCounter -= Time.deltaTime;

        // Coyote Time
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void CheckGround()
    {
        // Simple sphere check at the feet
        Vector3 spherePosition = transform.position + Vector3.down * (groundCheckOffset - groundCheckRadius);
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(spherePosition, groundCheckRadius, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            // Just landed
            jumpCount = 0;
            isJumping = false;
            jumpTimeCounter = 0f;
        }
    }

    private void HandleMovementPhysics()
    {
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;
        currentSpeed = 0;

        if (inputDirection.magnitude >= 0.1f)
        {
            // Rotation
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(Quaternion.Euler(0f, smoothAngle, 0f));

            // Movement Direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Apply Velocity (Preserve Y)
            Vector3 targetVelocity = moveDir * moveSpeed;
            rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);

            currentSpeed = moveDir.magnitude;
        }
        else
        {
            // Stop horizontal movement immediately (snappy) or use Drag
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        // Handle Dash Start
        if (dashInput)
        {
            StartDash(inputDirection);
            dashInput = false; // Consume input
        }
    }

    private void HandleGravityAndJumpPhysics()
    {
        // --- Jump Initiation ---
        if (jumpInputDown)
        {
            if (coyoteTimeCounter > 0f || (jumpCount < maxJumps && !isGrounded))
            {
                float minJumpVelocity = Mathf.Sqrt(minJumpHeight * -2f * gravity);
                rb.velocity = new Vector3(rb.velocity.x, minJumpVelocity, rb.velocity.z);

                jumpCount++;
                coyoteTimeCounter = 0f;
                isJumping = true;
                jumpTimeCounter = 0f;
            }
            jumpInputDown = false; // Consume input
        }

        // --- Variable Jump Height (Holding Button) ---
        if (isJumping && jumpInputHeld && jumpTimeCounter < maxJumpTime)
        {
            jumpTimeCounter += Time.fixedDeltaTime;
            float jumpProgress = jumpTimeCounter / maxJumpTime;
            float forceMultiplier = 1f - (jumpProgress * jumpProgress);

            // Add upward velocity directly
            rb.velocity += Vector3.up * (jumpHoldForce * forceMultiplier * Time.fixedDeltaTime);
        }

        // --- Stop Variable Jump ---
        if ((isJumping && !jumpInputHeld) || jumpTimeCounter >= maxJumpTime)
        {
            isJumping = false;
        }

        // --- Custom Gravity ---
        if (rb.velocity.y < 0)
        {
            // Falling
            rb.velocity += Vector3.up * (gravity * fallMultiplier * Time.fixedDeltaTime);
        }
        else if (rb.velocity.y > 0 && !jumpInputHeld)
        {
            // Short Hop (released button while going up)
            rb.velocity += Vector3.up * (gravity * lowJumpMultiplier * Time.fixedDeltaTime);
        }
        else
        {
            // Normal Gravity
            rb.velocity += Vector3.up * (gravity * Time.fixedDeltaTime);
        }

        // Terminal velocity safety (optional)
        if (rb.velocity.y < -50f) rb.velocity = new Vector3(rb.velocity.x, -50f, rb.velocity.z);
    }

    private void StartDash(Vector3 inputDirection)
    {
        if (inputDirection.magnitude < 0.1f)
        {
            dashDirection = transform.forward;
        }
        else
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            dashDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        isDashing = true;
        dashTimeCounter = dashTime;
        dashCooldownCounter = dashCooldown;

        // Reset vertical velocity for a straight dash
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    private void HandleDashPhysics()
    {
        rb.velocity = dashDirection * dashSpeed;
        dashTimeCounter -= Time.fixedDeltaTime;

        if (dashTimeCounter <= 0)
        {
            isDashing = false;
            rb.velocity = Vector3.zero; // Stop momentum after dash (optional)
        }
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalVelocity", rb.velocity.y);
            animator.SetFloat("InputVertical", currentSpeed, 0.1f, Time.deltaTime); // Using speed magnitude for simplicity
            animator.SetFloat("InputHorizontal", horizontal, 0.1f, Time.deltaTime);
            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("IsDashing", isDashing);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Jump heights
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * minJumpHeight, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * jumpHeight, 0.1f);

        // Ground Check Visual
        Gizmos.color = Color.yellow;
        Vector3 spherePosition = transform.position + Vector3.down * (groundCheckOffset - groundCheckRadius);
        Gizmos.DrawWireSphere(spherePosition, groundCheckRadius);
    }
}