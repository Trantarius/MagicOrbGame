using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private float move;

    public float jumpForce;

    public bool isJumping;
    
    private Rigidbody rb;
    

    public GameObject nose;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Horizontal"); // A/D or Left/Right
        rb.velocity = new Vector3(move * speed, rb.velocity.y); // Move left/right

        if (move > 0)
        {
            // Moving right, rotate player to face right
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face right (no Y-axis rotation)
        }
        else if (move < 0)
        {
            // Moving left, rotate player to face left
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face left (180 degrees Y-axis rotation)
        }


        // Apply jump only if the player is grounded (not jumping)
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse); // Jump with upward force
            isJumping = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player touches the "Floor" tagged object
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = false; // Set isJumping to false when grounded
        }
    }
}
