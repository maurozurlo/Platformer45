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


    public bool alwaysMove;
    public bool alwaysParentPlayer = true;

    [SerializeField] public bool DEBUG;

    private void Start()
    {
        currentTarget = endPosition;
        movingToEnd = true;
        isMoving = alwaysMove;

        // Find the player GameObject and store its Transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (playerTransform == null){
            Debug.LogError("No GameObject with 'Player' tag found in the scene.");
        }

        if (DEBUG)
        {
            Debug.Log($"Start: {startPosition}, End: {endPosition}, Current: {transform.localPosition}");
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
            if (DEBUG)
            {
                Debug.Log("Platform moving toward " + currentTarget);
            }
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, currentTarget, speed * Time.deltaTime);
            
            // Check if we've reached the target position
            if (transform.localPosition == currentTarget)
            {
                if (movingToEnd)
                {
                    currentTarget = startPosition;
                }
                else
                {
                    currentTarget = endPosition;
                }
                movingToEnd = !movingToEnd;
                isMoving = alwaysMove;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!alwaysParentPlayer) return;
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null && collision.gameObject.transform.parent != transform)
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!alwaysParentPlayer) return;
        if (collision.gameObject.transform.parent == transform)
        {
            collision.gameObject.transform.parent = null;
        }
    }

    public void MovePlatform()
    {
        if (DEBUG)
            Debug.Log("MovePlatform called");

        if (isMoving)
        {
            if (DEBUG)
                Debug.Log("Already moving, skipping");
            return;
        }
        isMoving = true;
    }
}
