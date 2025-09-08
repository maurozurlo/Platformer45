using UnityEngine;

public class PingPongPlatform : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float speed = 1.0f;

    public bool alwaysMove = true;
    public bool alwaysParentPlayer = true;

    public float raycastDistance = 0.2f; // how far below player to check for platform
    public LayerMask groundMask; // should include your platform layer
    public bool DEBUG;

    private Vector3 currentTarget;
    private bool movingToEnd;
    private bool isMoving;

    private Transform player;
    private bool playerIsOnPlatform;

    private void Start()
    {
        currentTarget = endPosition;
        movingToEnd = true;
        isMoving = alwaysMove;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        else Debug.LogError("No GameObject with 'Player' tag found in scene");

        if (DEBUG)
            Debug.Log($"Start: {startPosition}, End: {endPosition}, Current: {transform.localPosition}");
    }

    private void Update()
    {
        // Move the platform
        if (isMoving)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                currentTarget,
                speed * Time.deltaTime
            );

            if (transform.localPosition == currentTarget)
            {
                currentTarget = movingToEnd ? startPosition : endPosition;
                movingToEnd = !movingToEnd;
                isMoving = alwaysMove;
            }
        }

        // Raycast from player downward to detect this platform
        if (alwaysParentPlayer && player != null)
        {
            bool hitThisPlatform = false;
            if (Physics.Raycast(player.position, Vector3.down, out RaycastHit hit, raycastDistance, groundMask))
            {
                Debug.Log(hit);
                if (hit.collider != null && hit.collider.transform == transform)
                {
                    hitThisPlatform = true;
                }
            }

            if (hitThisPlatform && !playerIsOnPlatform)
            {
                if (DEBUG) Debug.Log("Raycast hit: Parenting player to platform");
                player.parent = transform;
                playerIsOnPlatform = true;
            }
            else if (!hitThisPlatform && playerIsOnPlatform)
            {
                if (DEBUG) Debug.Log("Raycast miss: Unparenting player");
                player.parent = null;
                playerIsOnPlatform = false;
            }
        }
    }

    public void MovePlatform() { }
}
