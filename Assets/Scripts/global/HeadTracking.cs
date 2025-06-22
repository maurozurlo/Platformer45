using UnityEngine;

public class HeadTracking : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public float rotationSpeed = 5f; // Speed at which the head rotates
    public float maxVerticalAngle = 30f; // Maximum vertical angle for head rotation
    public float maxHorizontalAngle = 60f; // Maximum horizontal angle for head rotation
    public float detectionRadius = 10f; // Radius within which the head will track the player

    public Transform headTransform; // Reference to the head's transform


    private void LateUpdate()
    {
        // Check if the player is within the detection radius
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Calculate the direction from the head to the player
            Vector3 directionToPlayer = (playerTransform.position - headTransform.position).normalized;

            // Clamp the direction within the desired angle limits
            directionToPlayer = Vector3.ClampMagnitude(directionToPlayer, maxVerticalAngle);

            // Calculate the desired rotation for the head
            Quaternion desiredRotation = Quaternion.LookRotation(directionToPlayer);

            // Interpolate between the current head rotation and the desired clamped rotation
            headTransform.localRotation = Quaternion.Slerp(headTransform.localRotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}