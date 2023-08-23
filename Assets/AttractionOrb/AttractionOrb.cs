using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionOrb : MonoBehaviour
{
    public float magneticForce = 10f; // Adjust this value to control the strength of the magnetic force

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WaterOrb"))
        {
            Debug.Log("OnTriggerStay");
            // Calculate the force direction from the water orb to the attraction orb
            Vector3 forceDirection = transform.position - other.transform.position;
            forceDirection.Normalize();

            // Apply the magnetic force to the water orb's Rigidbody
            Rigidbody waterOrbRb = other.GetComponent<Rigidbody>();
            waterOrbRb.AddForce(forceDirection * magneticForce * Time.fixedDeltaTime);
        }
    }
}
