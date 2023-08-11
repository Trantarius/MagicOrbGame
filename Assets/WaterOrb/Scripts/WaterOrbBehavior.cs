using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOrbBehavior : MonoBehaviour
{
    public float slowdownFactor = 0.5f;
    public float magneticForce = 10f; // Adjust the strength of the magnetic force
    public float attractionRadius = 5f; // Adjust the radius for detecting attraction orbs

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        DetectAndRespondToAttractionOrbs();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 newVelocity = rb.velocity * slowdownFactor;
            rb.velocity = newVelocity;
        }
    }

    void DetectAndRespondToAttractionOrbs()
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, attractionRadius);

        foreach (Collider collider in nearbyColliders)
        {
            if (collider.CompareTag("AttractionOrb"))
            {
                Vector3 directionToAttractionOrb = collider.transform.position - transform.position;
                directionToAttractionOrb.Normalize();

                rb.AddForce(directionToAttractionOrb * magneticForce * Time.fixedDeltaTime);
            }
        }
    }
}
