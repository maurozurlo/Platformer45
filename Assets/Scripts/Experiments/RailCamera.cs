using UnityEngine;

public class RailCamera : MonoBehaviour
{
    public Transform target;
    public Transform fromWaypoint;
    public Transform toWaypoint;

    public float transitionFactor = 0.5f; // 0 = desde fromWaypoint, 1 = desde toWaypoint
    public float smoothSpeed = 2f;
    public float cameraDistance = 5f;
    public float cameraHeight = 2f;

    void LateUpdate()
    {
        if (target == null || fromWaypoint == null || toWaypoint == null) return;

        // Interpolamos posición y rotación entre los waypoints
        Vector3 interpolatedPosition = Vector3.Lerp(fromWaypoint.position, toWaypoint.position, transitionFactor);
        Quaternion interpolatedRotation = Quaternion.Slerp(fromWaypoint.rotation, toWaypoint.rotation, transitionFactor);

        // Usamos la rotación interpolada para calcular la dirección de la cámara
        ;
        Vector3 direction = -(interpolatedRotation * Vector3.forward);

        // Calculamos posición deseada a cierta distancia y altura desde esa dirección
        Vector3 desiredPosition = target.position + direction * cameraDistance;
        desiredPosition.y += cameraHeight;

        // Interpolamos suavemente hacia esa posición
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // La cámara siempre mira al jugador
        transform.LookAt(target);
    }
}
