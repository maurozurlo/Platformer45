using UnityEngine;

public class CameraPlatform : MonoBehaviour
{
    public Transform target;         // El jugador
    public Vector3 offset = new Vector3(0f, 10f, -10f); // Distancia de cámara
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null) return;

        // Posición deseada
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        // Mantener mirando al jugador
        transform.LookAt(target);
    }


}
