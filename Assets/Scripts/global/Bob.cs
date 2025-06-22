using UnityEngine;

public class Bob : MonoBehaviour
{
    public float speed = 1.0f;   // Speed of the bobbing motion
    public float amplitude = 1.0f; // How much the object will bob up and down

    private Vector3 startPosition; // Initial position of the object
    private float timeElapsed = 0.0f; // Track elapsed time for smooth bobbing

    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
    }

    void Update()
    {
        // Increment the elapsed time
        timeElapsed += Time.deltaTime * speed;

        // Calculate the new position using a sine wave for smooth bobbing motion
        float newY = startPosition.y + Mathf.Sin(timeElapsed) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    public void UpdateStartPosition(Vector3 newStartPos)
    {
        startPosition = newStartPos;
    }
}
