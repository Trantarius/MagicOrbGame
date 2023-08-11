using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOrbBehavior : MonoBehaviour
{
    public float slowdownFactor = 0.5f; // Adjust this to control how much the orb slows down upon collision
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Calculate the new velocity by reducing the current velocity based on the slowdownFactor
            Vector3 newVelocity = rb.velocity * slowdownFactor;

            // Apply the new velocity to the Rigidbody
            rb.velocity = newVelocity;
        }
    }
}
