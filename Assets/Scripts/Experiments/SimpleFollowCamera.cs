using UnityEngine;

public class SimpleFollowCamera : MonoBehaviour
{
    public Transform target;        // El jugador
    public Vector3 offset = new Vector3(0, 3, -6); // Altura y distancia detrás del jugador
    public float smoothSpeed = 5f;  // Qué tan rápido sigue la cámara

    void LateUpdate()
    {
        if (target == null) return;

        // Calcula posición deseada en base a la posición y rotación del jugador
        Vector3 desiredPosition = target.position + target.rotation * offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        // Mira siempre al jugador (puede ajustarse si molesta)
        transform.LookAt(target.position + Vector3.up * 1.5f); // mira un poco más arriba que el pivote
    }
}
