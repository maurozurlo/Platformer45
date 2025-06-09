using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 180f; // grados por segundo

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Vertical");   // W/S
        float turnInput = Input.GetAxis("Horizontal"); // A/D

        // Rotación (gira sobre su propio eje Y)
        transform.Rotate(0f, turnInput * turnSpeed * Time.deltaTime, 0f);

        Vector3 p = Vector3.forward;
        // Movimiento hacia adelante (en base a rotación actual)
        Vector3 forwardMovement = transform.forward * moveInput * moveSpeed;
        controller.SimpleMove(forwardMovement);
    }
}
