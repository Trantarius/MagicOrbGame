using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefBehavior : MonoBehaviour
{
    private bool isHoldingOrb = false;
    private GameObject heldOrb;

    public float movementSpeed = 5f;

    void Update()
    {
        if (isHoldingOrb && heldOrb != null)
        {
            // Move forward while holding the orb
            transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
        }
        else
        {
            // Find the nearest water orb
            GameObject nearestOrb = FindNearestWaterOrb();
            if (nearestOrb != null)
            {
                // Move towards the nearest orb
                Vector3 moveDirection = (nearestOrb.transform.position - transform.position).normalized;
                transform.Translate(moveDirection * movementSpeed * Time.deltaTime);
            }
        }
    }

    GameObject FindNearestWaterOrb()
    {
        GameObject[] waterOrbs = GameObject.FindGameObjectsWithTag("WaterOrb");
        GameObject nearestOrb = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject orb in waterOrbs)
        {
            float distance = Vector3.Distance(transform.position, orb.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestOrb = orb;
            }
        }

        return nearestOrb;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isHoldingOrb && collision.gameObject.CompareTag("WaterOrb"))
        {
            // Steal the water orb
            isHoldingOrb = true;
            heldOrb = collision.gameObject;

            // Make the water orb a child of the thief
            heldOrb.transform.parent = transform;

            // Disable the water orb's Rigidbody
            Rigidbody orbRb = heldOrb.GetComponent<Rigidbody>();
            if (orbRb != null)
            {
                orbRb.isKinematic = true;
            }

            // Destroy the water orb
            Destroy(heldOrb);
        }
    }

    public void HitByMagicBall(float damage)
    {
        // Destroy the thief
        Destroy(gameObject);
    }
}

