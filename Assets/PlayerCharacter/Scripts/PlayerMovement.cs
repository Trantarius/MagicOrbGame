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

    //Used for finding the average floor normal
    private Vector3 floornormTotal;
    private int floornormCount;

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

        if(jumpTime>0){
            if(pi.actions["Jump"].IsPressed()){
                rb.AddForce(Vector3.up*jumpForce/jumpDuration);
            }
            jumpTime-=Time.fixedDeltaTime;
        }

        // If on the ground, movement will be considered tangent to the ground surface.
        // This prevents the movement force from pushing the player into the air.
        Vector3 right;
        float forceMul=1.0f;
        if(isGrounded){
            Vector3 floornorm=(floornormTotal/floornormCount).normalized;
            right=new Vector3(floornorm.y,-floornorm.x,floornorm.z);

            floornormTotal=Vector3.zero;
            floornormCount=0;
        }
        else{
            right=Vector3.right;
            forceMul=0.25f;
        }

        float currentSpeed=Vector3.Dot(rb.velocity,right);
        float targetSpeed=move*speed;
        float moveForce=(targetSpeed-currentSpeed)*acceleration;
        rb.AddForce(right*moveForce*forceMul);
        
        isGrounded=false;
    }

    void OnCollisionStay(Collision collision){

        ContactPoint contact=collision.GetContact(0);
        float dir=Vector3.Dot(Vector3.up,contact.normal);

        if(dir>0.707){// surface is < 45 degrees from horizontal
            isGrounded=true;
            //nullify horizontal correction force, so that the player doesn't slide down shallow slopes.
            rb.AddForce(-Vector3.right*collision.impulse.x,ForceMode.Impulse);
            floornormTotal+=contact.normal;
            floornormCount++;
        }
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
