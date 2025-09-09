using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public GameObject vc;
    public float rotationSpeed = 2f;

    private Quaternion targetRotation;
    private bool transitioning = false;

    private void Start()
    {
        targetRotation = vc.transform.rotation;
    }

    private void Update()
    {
        if (transitioning)
        {
            vc.transform.rotation = Quaternion.Slerp(
                vc.transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );

            // Optional: stop when close enough
            if (Quaternion.Angle(vc.transform.rotation, targetRotation) < 0.1f)
            {
                vc.transform.rotation = targetRotation;
                transitioning = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered");
            targetRotation = Quaternion.Euler(new Vector3(30, -90, 0));
            transitioning = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exit");
            targetRotation = Quaternion.Euler(new Vector3(30, 0, 0));
            transitioning = true;
        }
    }
}
