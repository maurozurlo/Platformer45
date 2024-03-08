using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollHead : MonoBehaviour
{
    public float speed;
    public float maxSpeed = 40f;

    private Rigidbody rb;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate ()
    {
        float moveHorizontal = -Input.GetAxis ("Horizontal2");
        float moveVertical = Input.GetAxis ("Vertical2");

        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

        rb.AddForce (movement * speed);

        if(rb.velocity.magnitude > maxSpeed){
             rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
         }
    }
}
