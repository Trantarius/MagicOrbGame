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
