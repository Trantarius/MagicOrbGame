using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Tooltip("Maximum horizontal target speed")]
    public float speed;
    [Tooltip("Force used to bring player to target speed")]
    public float groundAcceleration;
    [Tooltip("Force used to bring player to target speed")]
    public float airAcceleration;
    [Tooltip("Impulse applied when player jumps")]
    public float jumpForce;
    [Tooltip("Time that space is held to reach max jump height")]
    public float jumpDuration;


    private float move;
    private bool isGrounded;
    private bool isFacingRight = true;
    
    private Rigidbody rb;
    public AudioSource jumpAudioSource;
    public HealthBar healthBar;

    //Used for finding the average floor normal
    private Vector3 floornormTotal;
    private int floornormCount;

    private bool isControlsDisabled = false;

    public GameObject magicBallPrefab;
    public float shootingForce = 10f;
    public float spawnOffset = 0.5f; // Adjust this value to set the desired offset from the nose
    public float maxTravelDistance = 30f; // The maximum distance the magic ball can travel before disappearing
    public AudioSource shootingAudioSource;
    private float gravityMultiplierFalling = 2.5f;
    private float gravityMultiplierHangTime = 0.5f;
    private float hangTimeThreshold = 0.1f;
    private float jumpPower = 15.0f;
    private MeshRenderer mr;
    private float timeSinceHit = 0;
    private Material originalMaterial;
    void Start()
    {
        jumpAudioSource = GetComponent<AudioSource>();
        //disables sleeping. If the body sleeps, we will stop receiving OnCollsionStay events.
        rb.sleepThreshold=0.0f;
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        timeSinceHit += Time.deltaTime;   
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
            right = Vector3.right;
            forceMul = 0.25f;

            // apply gravity
            // Let the player hang mid-jump
            var gravityScale = Math.Abs(rb.velocity.y) < hangTimeThreshold ?
                gravityMultiplierHangTime :
                gravityMultiplierFalling;

            Vector3 gravity = -9.8f * gravityScale * Vector3.up;
            rb.AddForce(gravity, ForceMode.Acceleration);            
        }

        float currentSpeed = Vector3.Dot(rb.velocity, right);
        float targetSpeed = move * speed;
        float moveForce = (targetSpeed - currentSpeed) * (isGrounded ? groundAcceleration : airAcceleration);
        
        // Turn immediately for less floaty feel
        if (rb.velocity.x < 0 && move > 0 || rb.velocity.x > 0 && move < 0) {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
       
        rb.AddForce(right * moveForce * forceMul);
        
        isGrounded = false;

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

                this.rb.velocity = new Vector2(this.rb.velocity.x, this.jumpPower);
            }
        } else if (context.canceled) {
            // Half speed if jump button is released as the player ascends
            this.rb.velocity = new Vector2(this.rb.velocity.x, Mathf.Min(this.rb.velocity.y, this.rb.velocity.y / 2, 3.0f));
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
            rb.velocity = (isFacingRight ? rb.transform.right : -rb.transform.right) * shootingForce;

            // Destroy the magic ball after a certain time or distance
            Destroy(magicBall, maxTravelDistance / shootingForce);
        }
    }

    void OnEnable()
    {
        EventBus.onLevelCompleted += StopAndDisableControls;
        EventBus.onLevelFailed += StopAndDisableControls;
        EventBus.onGameFailed += StopAndDisableControls;
        EventBus.onPlayerHit += OnPlayerHit;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void OnDisable()
    {
        EventBus.onLevelCompleted -= StopAndDisableControls;
        EventBus.onLevelFailed -= StopAndDisableControls;
        EventBus.onPlayerHit -= OnPlayerHit;
    }

    private void StopAndDisableControls()
    {
        move = 0;
        isControlsDisabled = true;
    }

    private void OnPlayerHit(float damage)
    {
        if (timeSinceHit > 0.5f)
        {
            DamageAnimation();
            EventBus.RaiseOnDamageTaken(damage);
            timeSinceHit = 0;
        }
    }
    private void DamageAnimation()
    {
        var mat = new Material(mr.material);
        originalMaterial = mr.material;
        mat.SetColor("_BaseColor", Color.red);
        mr.material = mat;
        InvokeRepeating("ToggleVisable", 0, 0.05f);
        Invoke("StopDamageAnimation", 0.5f);
    }

    private void StopDamageAnimation()
    {
        CancelInvoke("ToggleVisable");
        mr.material = originalMaterial;
    }

    private void ToggleVisable()
    {
        mr.enabled = !mr.enabled;
    }
}
