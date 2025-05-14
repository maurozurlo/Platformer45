using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform closedPivot;
    public Transform openPivot;
    public float duration = 1.0f;

    private bool isOpen = false;
    private Coroutine moveCoroutine;

    public void OpenDoor()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveDoor(isOpen ? closedPivot : openPivot));
        isOpen = !isOpen;
    }

    private IEnumerator MoveDoor(Transform target)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        transform.rotation = endRot;
    }
}
