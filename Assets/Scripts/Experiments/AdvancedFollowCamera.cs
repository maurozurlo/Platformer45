using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AdvancedFollowCamera : MonoBehaviour
{
    public Transform target;
    public float targetHeight = 2.0f;
    public float distance = 4.0f;
    public float maxDistance = 10f;
    public float minDistance = 1.5f;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    public int yMinLimit = -20;
    public int yMaxLimit = 80;

    public float zoomRate = 20f;
    public float rotationDampening = 5.0f;
    public float followSmoothness = 5f;

    private float x = 0.0f;
    private float y = 0.0f;
    private float currentDistance;

    private Vector3 lastTargetPosition;

    void Start()
    {
        Vector2 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        currentDistance = distance;

        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;

        lastTargetPosition = target.position;
    }

    void LateUpdate()
    {
        if (!target) return;

        bool isMoving = Vector3.Distance(target.position, lastTargetPosition) > 0.01f;
        lastTargetPosition = target.position;

        bool mousePressed = Input.GetMouseButton(0) || Input.GetMouseButton(1);
        if (mousePressed)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            //userRotatingCamera = true;
        }
        else if (isMoving)
        {
            float targetRotation = target.eulerAngles.y;
            x = Mathf.LerpAngle(x, targetRotation, rotationDampening * Time.deltaTime);
            //userRotatingCamera = false;
        }

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        // Zoom con rueda del mouse
        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomRate * Time.deltaTime * Mathf.Abs(distance);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        currentDistance = Mathf.Lerp(currentDistance, distance, followSmoothness * Time.deltaTime);

        // Rotación y posición deseada
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 offset = rotation * new Vector3(0.0f, 0.0f, -currentDistance);
        Vector3 desiredPosition = target.position + new Vector3(0, targetHeight, 0) + offset;

        // Colisión con entorno
        Vector3 trueTargetPos = target.position + new Vector3(0, targetHeight, 0);
        if (Physics.Linecast(trueTargetPos, desiredPosition, out RaycastHit hit))
        {
            float adjustedDistance = Vector3.Distance(trueTargetPos, hit.point) - 0.3f;
            desiredPosition = trueTargetPos + rotation * new Vector3(0.0f, 0.0f, -adjustedDistance);
        }

        // Suavizado de movimiento y rotación
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSmoothness * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, followSmoothness * Time.deltaTime);
    }

    float ClampAngle(float angle, float min, float max)
    {
        angle %= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
