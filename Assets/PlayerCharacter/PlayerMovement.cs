using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Maximum horizontal target speed")]
    public float speed;
    [Tooltip("Force used to bring player to target speed")]
    public float acceleration;
    [Tooltip("Impulse applied when player jumps")]
    public float jumpForce;


    private float move;
    private bool isGrounded;
    
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold=0.0f;
    }

    void FixedUpdate()
    {
        float currentSpeed=rb.velocity.x;
        float targetSpeed=move*speed;
        float moveForce=(targetSpeed-currentSpeed)*acceleration;
        rb.AddForce(new Vector3(moveForce,0,0),ForceMode.Force);
        isGrounded=false;
    }

    void OnCollisionStay(Collision collision){
        ContactPoint contact=collision.GetContact(0);
        float dir=Vector3.Dot(Vector3.up,contact.normal);
        isGrounded |= dir>0.707;
    }


    private void OnMove(InputValue inputValue){
        move=inputValue.Get<Vector2>().x;
    }

    private void OnJump(){
        if(isGrounded){
            rb.AddForce(Vector3.up*jumpForce,ForceMode.Impulse);
        }
    }
}
