using UnityEngine;

[ExecuteInEditMode]
public class WaypointGizmo : MonoBehaviour
{
    public float arrowLength = 2f;
    public Color arrowColor = Color.cyan;
    public float lineThickness = 0.05f;

    void OnDrawGizmos()
    {
        Gizmos.color = arrowColor;
        Vector3 start = transform.position;
        Vector3 dir = transform.forward * arrowLength;
        Vector3 right = transform.right * lineThickness;
        Vector3 up = transform.up * lineThickness;

        // Línea central
        Gizmos.DrawLine(start, start + dir);

        // Líneas paralelas para simular grosor
        Gizmos.DrawLine(start + right, start + right + dir);
        Gizmos.DrawLine(start - right, start - right + dir);
        Gizmos.DrawLine(start + up, start + up + dir);
        Gizmos.DrawLine(start - up, start - up + dir);

        // Punta de la flecha (esfera)
        Gizmos.DrawSphere(start + dir, 0.1f);
    }
}