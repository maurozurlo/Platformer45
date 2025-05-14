using System.Collections;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public GameObject player;
    public float rotationSpeed = 2f; // Speed of the smooth rotation

    private Vector3 offset = new Vector3(0, 6, 6);
    private Vector3 targetOffset;
    private bool isRotating = false;

    public static event System.Action<Quaternion> OnCameraRotated; // 🔥 EVENT

    void Start()
    {
        targetOffset = offset;
    }

    void Update()
    {
        // Start rotation if L is pressed and not already rotating
        if (Input.GetKeyDown(KeyCode.L) && !isRotating)
        {
            StartCoroutine(RotateOffset90Degrees());
        }

        // Smoothly move camera to new offset position
        offset = Vector3.Lerp(offset, targetOffset, Time.deltaTime * rotationSpeed);
        transform.position = player.transform.position + offset;

        // Always look at the player
        transform.LookAt(player.transform.position);
    }

    IEnumerator RotateOffset90Degrees()
    {
        
        isRotating = true;

        // Determine new offset by rotating 90 degrees around Y axis
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        targetOffset = rotation * targetOffset;
        // 🔥 Notify listeners with the rotation applied
        OnCameraRotated?.Invoke(rotation);

        // Wait until the offset is close enough to the target
        while (Vector3.Distance(offset, targetOffset) > 0.01f)
        {
            yield return null;
        }

        offset = targetOffset;
        isRotating = false;


    }
}
