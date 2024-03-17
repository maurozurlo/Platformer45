using UnityEngine;

public class PingPongPlatform : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float speed = 1.0f;
    public LayerMask playerMask; // Layer on which the player GameObject is present
    public bool isMoving { get; private set; }

    private Vector3 currentTarget;
    private bool movingToEnd;
    private Transform playerTransform; // Reference to the player's Transform

    private void Start()
    {
        currentTarget = endPosition;
        movingToEnd = true;
        isMoving = false;

        // Find the player GameObject and store its Transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (playerTransform == null){
            Debug.LogError("No GameObject with 'Player' tag found in the scene.");
        }
    }

    private void Update()
    {
        // Raycast to check if the player is under the platform
        RaycastHit hit;
        if (playerTransform != null && Physics.Raycast(playerTransform.position, Vector3.up, out hit, 10f, playerMask))
        {
            if (hit.collider.gameObject == gameObject)
            {
                return;
            }
        }

        // Move the platform towards the current target position if it is currently moving
        if (isMoving)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, currentTarget, speed * Time.deltaTime);

            // Check if we've reached the target position
            if (transform.localPosition == currentTarget)
            {
                // Swap the target position
                if (movingToEnd)
                {
                    currentTarget = startPosition;
                }
                else
                {
                    currentTarget = endPosition;
                }
                movingToEnd = !movingToEnd;
                isMoving = false; // Platform has reached target position, so stop moving
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        // If the colliding object has a Rigidbody and isn't already a child of this platform
        if (rb != null && collision.gameObject.transform.parent != transform)
        {
            // Make the colliding object a child of this platform
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // If the object leaving the platform was a child
        if (collision.gameObject.transform.parent == transform)
        {
            // Detach the object from this platform
            collision.gameObject.transform.parent = null;
        }
    }

    // New method to move the platform from one position to the other
    public void MovePlatform()
    {
        if (isMoving) return;
        isMoving = true;
    }
}
