using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerPlatformer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Jumping")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public int maxJumps = 2;
    public float coyoteTime = 0.2f;

    [Header("Better Jump Feel")]
    public float fallMultiplier = 2.5f;       // faster fall
    public float lowJumpMultiplier = 2f;      // shorter if button released early

    private CharacterController controller;
    private Camera mainCamera;
    private Animator animator;

    private Vector3 velocity;
    private bool isGrounded;
    private int jumpCount;
    private float coyoteTimeCounter;

    private float currentSpeed;

    public Vector3 displace = new Vector3();

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
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;


        currentSpeed = 0;
        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg
                                + mainCamera.transform.eulerAngles.y;
            float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * moveSpeed * Time.deltaTime);
            currentSpeed = moveDir.magnitude;
        }

        // Jump (with coyote + double jump)
        if (Input.GetButtonDown("Jump"))
        {
            if (coyoteTimeCounter > 0f || jumpCount < maxJumps && !isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpCount++;
                coyoteTimeCounter = 0f;
            }
        }

        // Apply gravity
        if (velocity.y < 0)
        {
            // Falling faster
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (velocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Short hop if button released
            velocity.y += gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Always apply base gravity
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        // Animator
        if (animator != null)
        {
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalVelocity", velocity.y);
            animator.SetFloat("InputVertical", currentSpeed, 0.1f, Time.deltaTime);
            animator.SetFloat("InputHorizontal", horizontal, 0.1f, Time.deltaTime);
        }
    }
}
