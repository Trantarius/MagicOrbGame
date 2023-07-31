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
    [Tooltip("Time that space is held to reach max jump height")]
    public float jumpDuration;


    private float move;
    private bool isGrounded;
    
    private Rigidbody rb;
    private PlayerInput pi;
    private float jumpTime=0;

    // Start is called before the first frame update
    void Start()
    {
        pi = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        //disables sleeping. If the body sleeps, we will stop receiving OnCollsionStay events.
        rb.sleepThreshold=0.0f;
    }

    void FixedUpdate()
    {
        float currentSpeed=rb.velocity.x;
        float targetSpeed=move*speed;
        float moveForce=(targetSpeed-currentSpeed)*acceleration;
        rb.AddForce(new Vector3(moveForce,0,0),ForceMode.Force);
        isGrounded=false;

        if(jumpTime>0){
            if(pi.actions["Jump"].IsPressed()){
                rb.AddForce(Vector3.up*jumpForce/jumpDuration);
            }
            jumpTime-=Time.fixedDeltaTime;
        }

    }

    void OnCollisionStay(Collision collision){
        ContactPoint contact=collision.GetContact(0);
        float dir=Vector3.Dot(Vector3.up,contact.normal);
        isGrounded |= dir>0.707;
    }


    public void OnMove(InputAction.CallbackContext context){
        move=context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context){
        if(context.performed){
            //button is pressed
            if(isGrounded){
                jumpTime=jumpDuration;
            }
        }else if(context.canceled){
            //button is released
            jumpTime=0;
        }
    }
}
