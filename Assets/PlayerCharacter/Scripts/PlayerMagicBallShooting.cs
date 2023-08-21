using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicBallShooting : MonoBehaviour
{
    public GameObject magicBallPrefab;
    public Transform shootingPoint;
    public float shootingForce = 10f;
    public float spawnOffset = 0.5f; // Adjust this value to set the desired offset from the nose
    public float maxTravelDistance = 10f; // The maximum distance the magic ball can travel before disappearing

    void Update()
    {
        
        //Press 'E' to shoot magic ball

        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootMagicBall();
        }
        
        if (Input.GetButtonDown("Fire1")) // Change "Fire1" to your preferred shoot input
        {
            ShootMagicBall();
        }
    }

    void ShootMagicBall()
    {
        // Calculate the spawn offset in the shooting point's local space
        Vector3 localSpawnOffset = new Vector3(spawnOffset, 0f, 0f);

        // Transform the local offset to world space using the shooting point's orientation
        Vector3 spawnPosition = shootingPoint.TransformPoint(localSpawnOffset);

        // Instantiate the magic ball with the correct position and rotation
        GameObject magicBall = Instantiate(magicBallPrefab, spawnPosition, shootingPoint.rotation);

        // Get the magic ball's Rigidbody component to apply initial velocity
        Rigidbody rb = magicBall.GetComponent<Rigidbody>();

        // Apply the shooting force in the X direction of the shooting point
        rb.velocity = shootingPoint.right * shootingForce;

        // Destroy the magic ball after a certain time or distance
        Destroy(magicBall, maxTravelDistance / shootingForce);
    }
}
