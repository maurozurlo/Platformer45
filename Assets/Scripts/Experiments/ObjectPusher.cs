using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPusher : MonoBehaviour
{
    public float raycastDistance = 1f; // Length of the raycast line
    public float pushRaycastDistance = 0.5f; // Distance within which to push the object
    public float pushCooldown = 0.5f; // Cooldown time between pushes

    public GameObject targetObject; // The object to be pushed, if any
    private Collider targetCollider; // Collider of the target object

    private float lastPushTime; // Time when the last push occurred

    public float distance = 0;

    bool canPush = true;
	private float perc;

	void Update()
    {
        // Draw a line forwards in debug
        Vector3 origin = transform.position + new Vector3(0, .5f, 0);
        Debug.DrawRay(origin, transform.forward * raycastDistance, Color.red);

        // Check for pushable object within range
        RaycastHit hit;
        if (Physics.Raycast(origin, transform.forward, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("PushableObject"))
            {
                // Set the target object and its collider
                targetObject = hit.collider.gameObject;
                targetCollider = hit.collider;
                distance = Vector3.Distance(transform.position, targetObject.transform.position);
            }
        }
        else
        {
            if (canPush == false) return;
            // If out of range, set targetObject to null
            targetObject = null;
            targetCollider = null;
        }



        // Check if we can push the object
        if (targetObject != null && targetCollider != null && Vector3.Distance(transform.position, targetCollider.ClosestPoint(transform.position)) < pushRaycastDistance)
        {
            // Check if enough time has passed since last push
            if (canPush)
            {
                canPush = false;
                Vector3 direction = transform.forward.normalized;
                Vector3 pushDirection = new Vector3(
                    Mathf.Round(direction.x),
                    Mathf.Round(direction.y),
                    Mathf.Round(direction.z)
                );

                float objectSize = targetCollider.bounds.size.z; // Assuming movement along z-axis
                
                Vector3 dist = pushDirection * objectSize;
                Vector3 target = targetObject.transform.position + dist;
                StartCoroutine(SmoothTransition(1500, targetObject.transform.position, target));
            }
        }
    }

    IEnumerator SmoothTransition(float duration, Vector3 startPos, Vector3 target)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            targetObject.transform.position = Vector3.Lerp(startPos, target, t);
            elapsedTime += Time.time;
            yield return null;
        }

        // Ensure the target position is reached exactly
        targetObject.transform.position = target;
        canPush = true; // Assuming canPush is a variable controlling push availability
    }
}
