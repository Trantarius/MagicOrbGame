using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
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
    private bool isFacingRight = true;
    
    private Rigidbody rb;
    private PlayerInput pi;
    public AudioSource jumpAudioSource;
    public HealthBar healthBar;
    private float jumpTime=0;

    //Used for finding the average floor normal
    private Vector3 floornormTotal;
    private int floornormCount;

    private bool isControlsDisabled = false;

    public GameObject magicBallPrefab;
    public float shootingForce = 10f;
    public float spawnOffset = 0.5f; // Adjust this value to set the desired offset from the nose
    public float maxTravelDistance = 30f; // The maximum distance the magic ball can travel before disappearing
    public AudioSource shootingAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        pi = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        jumpAudioSource = GetComponent<AudioSource>();
        //disables sleeping. If the body sleeps, we will stop receiving OnCollsionStay events.
        rb.sleepThreshold=0.0f;
    }

    void FixedUpdate()
    {   
        if (move > 0)
        {
            isFacingRight = true;
            // Moving right, rotate player to face right
            transform.rotation = Quaternion.Euler(0, -60, 0); // Face right (-60 Y-axis rotation)
        }
        else if (move < 0)
        {
            isFacingRight = false;
            // Moving left, rotate player to face left
            transform.rotation = Quaternion.Euler(0, 60, 0); // Face left (60 degrees Y-axis rotation)
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

        if (Input.GetKeyDown(KeyCode.H))
        {
            healthBar.RestoreHealth(); // cheeeeaating ༼ つ ◕_◕ ༽つ 
        }
    }

    void OnCollisionStay(Collision collision) {
        ContactPoint contact = collision.GetContact(0);
        float dir = Vector3.Dot(Vector3.up,contact.normal);

        if(dir > 0.707){// surface is < 45 degrees from horizontal
            isGrounded = true;
            //nullify horizontal correction force, so that the player doesn't slide down shallow slopes.
            rb.AddForce(-Vector3.right*collision.impulse.x,ForceMode.Impulse);
            floornormTotal += contact.normal;
            floornormCount++;
        }
    }


    public void OnMove(InputAction.CallbackContext context){
        if (isControlsDisabled) {
            return;
        }

        move = context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (isControlsDisabled) {
            return;
        }

        if (context.performed) {
            //button is pressed
            if (isGrounded) {
                // Play the jump sound
                jumpAudioSource.Play();

                jumpTime = jumpDuration;
            }
        } else if (context.canceled){
            //button is released
            jumpTime = 0;
        }
    }

    public void OnFire(InputAction.CallbackContext context) {
        if (isControlsDisabled) {
            return;
        }

        if (context.performed) {
            // Calculate the spawn offset in the shooting point's local space
            Vector3 localSpawnOffset = new Vector3(isFacingRight ? spawnOffset : -spawnOffset, 0.4f, 0f);

            // Transform the local offset to world space using the shooting point's orientation
            Vector3 spawnPosition = transform.TransformPoint(localSpawnOffset);

            // Instantiate the magic ball with the correct position and rotation
            GameObject magicBall = Instantiate(
                magicBallPrefab,
                spawnPosition,
                Quaternion.identity
            );

            // Play the shooting sound from the shooting AudioSource
            shootingAudioSource.Play();

            // Get the magic ball's Rigidbody component to apply initial velocity
            Rigidbody rb = magicBall.GetComponent<Rigidbody>();

            // Apply the shooting force in the X direction of the shooting point
            rb.velocity = (isFacingRight ? rb.transform.right : Quaternion.Euler(0, 60, 0) * -rb.transform.right * 2.0f) * shootingForce;
            Debug.Log(rb.velocity);

            // Destroy the magic ball after a certain time or distance
            Destroy(magicBall, maxTravelDistance / shootingForce);
        }
    }

    void OnEnable()
    {
        EventBus.onLevelCompleted += StopaAndDisableControls;
        EventBus.onLevelFailed += StopaAndDisableControls;
    }

    void OnDisable()
    {
        EventBus.onLevelCompleted -= StopaAndDisableControls;
        EventBus.onLevelFailed -= StopaAndDisableControls;
    }

    private void StopaAndDisableControls()
    {
        move = 0;
        isControlsDisabled = true;
    }
}
