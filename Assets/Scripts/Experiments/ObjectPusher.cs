using System.Collections;
using UnityEngine;

public class ObjectPusher : MonoBehaviour
{
    public float raycastDistance = 1f; // Length of the raycast line
    public float pushRaycastDistance = 0.5f; // Distance within which to push the object
    public float pushCooldown = 0.5f; // Cooldown time between pushes

    public GameObject targetObject; // The object to be pushed, if any
    private Collider targetCollider; // Collider of the target object

    public float distance = 0;

    public enum PushStatus
    {
        Idle,
        Pushing
    }

    public PushStatus pushStatus = PushStatus.Idle;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (pushStatus == PushStatus.Pushing) return;

        // Draw a debug line to visualize the raycast
        Vector3 origin = transform.position + new Vector3(0, 0.5f, 0);
        Debug.DrawRay(origin, transform.forward * raycastDistance, Color.red);

        CheckForPushableObject(origin);
    }

    private void CheckForPushableObject(Vector3 origin)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, transform.forward, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("PushableObject"))
            {
                targetObject = hit.collider.gameObject;
                targetCollider = hit.collider;
                distance = Vector3.Distance(transform.position, targetObject.transform.position);
                StartCoroutine(TransitionWeight(1f, 0, 1));

                if (Vector3.Distance(transform.position, targetCollider.ClosestPoint(transform.position)) < pushRaycastDistance)
                {
                    TryToPushObject();
                }
            }
        }
        else
        {
            targetObject = null;
            targetCollider = null;
            StartCoroutine(TransitionWeight(0.5f, 1, 0));
        }
    }

    private void TryToPushObject()
    {
        pushStatus = PushStatus.Pushing;
        anim.SetBool("pushing", true);

        Vector3 direction = DeterminePushDirection();
        float objectSize = GetObjectSizeInDirection(direction);
        Vector3 targetPosition = targetObject.transform.position + direction * objectSize;

        if (IsPathBlocked(direction, objectSize))
        {
            StopPushing();
            return;
        }

        StartCoroutine(SmoothTransition(0.7f, targetObject.transform.position, targetPosition));
    }

    private Vector3 DeterminePushDirection()
    {
        Vector3 direction = transform.forward.normalized;
        Vector3 absDirection = new Vector3(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));

        if (absDirection.x > absDirection.z && absDirection.x > absDirection.y)
        {
            direction = new Vector3(Mathf.Sign(direction.x), 0, 0);
        }
        else if (absDirection.z > absDirection.x && absDirection.z > absDirection.y)
        {
            direction = new Vector3(0, 0, Mathf.Sign(direction.z));
        }
        else
        {
            direction = new Vector3(0, Mathf.Sign(direction.y), 0);
        }

        return direction;
    }

    private float GetObjectSizeInDirection(Vector3 direction)
    {
        if (direction.x != 0) return targetCollider.bounds.size.x;
        if (direction.y != 0) return targetCollider.bounds.size.y;
        return targetCollider.bounds.size.z;
    }

    private bool IsPathBlocked(Vector3 direction, float objectSize)
    {
        RaycastHit hit;
        if (Physics.Raycast(targetObject.transform.position, direction, out hit, objectSize))
        {
            Debug.Log("Cannot push. Path is blocked by " + hit.collider.gameObject.name);
            return true;
        }
        return false;
    }

    private void StopPushing()
    {
        anim.SetBool("pushing", false);
        pushStatus = PushStatus.Idle;
    }

    private IEnumerator SmoothTransition(float duration, Vector3 startPos, Vector3 targetPos)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            targetObject.transform.position = Vector3.Lerp(startPos, targetPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.transform.position = targetPos;
        StopPushing();
    }

    private IEnumerator TransitionWeight(float duration, float start, float end)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            anim.SetLayerWeight(1, Mathf.Lerp(start, end, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        anim.SetLayerWeight(1, end);
    }
}
