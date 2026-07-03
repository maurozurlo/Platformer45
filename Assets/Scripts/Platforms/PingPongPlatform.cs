using UnityEngine;

public class PingPongPlatform : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float speed = 1.0f;
    public bool alwaysMove = true;

    [Header("Interaction")]
    public bool alwaysParentPlayer = true;
    public LayerMask groundMask;

    [Header("Debug")]
    public bool DEBUG;

    private Vector3 currentTarget;
    private bool movingToEnd;
    private bool isMoving;

    // We store the Rigidbody to move it physically
    private Rigidbody platformRb;

    private Transform playerTransform;
    private Collider playerCollider;
    private bool playerIsOnPlatform;

    private void Start()
    {
        // Try to add a Rigidbody if one doesn't exist, to ensure smooth physics collisions
        if (!TryGetComponent(out platformRb))
        {
            platformRb = gameObject.AddComponent<Rigidbody>();
            platformRb.isKinematic = true; // Crucial: Moving platforms must be Kinematic
            platformRb.useGravity = false;
            platformRb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        else
        {
            platformRb.isKinematic = true;
        }

        currentTarget = endPosition;
        movingToEnd = true;
        isMoving = alwaysMove;

        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerCollider = playerObj.GetComponent<Collider>();
        }
    }

    // Use FixedUpdate for anything involving Rigidbodies
    private void FixedUpdate()
    {
        HandleMovement();
        HandlePlayerDetection();
    }

    private void HandleMovement()
    {
        if (!isMoving) return;

        // Calculate the new position based on Local or World space? 
        // Note: MoveTowards with Vectors usually implies Local if used with localPosition, 
        // but Rigidbody.MovePosition works best in World Space.

        // Let's convert the local target to world space for the MovePosition calculation
        Vector3 targetWorldPos = transform.parent != null ?
            transform.parent.TransformPoint(currentTarget) : currentTarget;

        // If start/end positions were intended to be local coordinates:
        Vector3 nextPos = Vector3.MoveTowards(transform.position, targetWorldPos, speed * Time.fixedDeltaTime);

        // Move the Rigidbody (Physical movement pushes the player better than Transform movement)
        platformRb.MovePosition(nextPos);

        // Check if reached destination (using small distance check due to float imprecision)
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
        {
            currentTarget = movingToEnd ? startPosition : endPosition;
            movingToEnd = !movingToEnd;
            // Optional: Add a pause logic here if needed
        }
    }

    private void HandlePlayerDetection()
    {
        if (!alwaysParentPlayer || playerTransform == null || playerCollider == null) return;

        // FIX 1: Find the actual bottom of the player (the feet), regardless of pivot point
        Vector3 feetPosition = new Vector3(
            playerTransform.position.x,
            playerCollider.bounds.min.y + 0.1f, // Start slightly inside the feet
            playerTransform.position.z
        );

        // Raycast down from the feet
        bool hitThisPlatform = false;
        // Increase distance slightly to account for the bounds check
        float checkDistance = 0.3f;

        if (Physics.Raycast(feetPosition, Vector3.down, out RaycastHit hit, checkDistance, groundMask))
        {
            if (hit.collider.transform == transform)
            {
                hitThisPlatform = true;
            }
        }

        if (hitThisPlatform && !playerIsOnPlatform)
        {
            if (DEBUG) Debug.Log("Platform: Player entered");
            playerTransform.SetParent(transform);
            playerIsOnPlatform = true;
        }
        else if (!hitThisPlatform && playerIsOnPlatform)
        {
            // Only unparent if we are currently the parent 
            // (Prevents clearing parent if player jumped to another platform)
            if (playerTransform.parent == transform)
            {
                if (DEBUG) Debug.Log("Platform: Player exited");
                playerTransform.SetParent(null);

                // OPTIONAL: Reset Player Scale if platform was scaled
                // playerTransform.localScale = Vector3.one; 
            }
            playerIsOnPlatform = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (!DEBUG) return;

        // Draw the path
        Gizmos.color = Color.cyan;
        Vector3 p1 = transform.parent != null ? transform.parent.TransformPoint(startPosition) : startPosition;
        Vector3 p2 = transform.parent != null ? transform.parent.TransformPoint(endPosition) : endPosition;
        Gizmos.DrawLine(p1, p2);
    }

    public void MovePlatform() { }
}