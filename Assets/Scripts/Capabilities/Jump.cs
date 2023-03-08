using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CollisionCheck))]
public class Jump : Capability
{
    //[Header("References")]
    [SerializeField] private InputController inputController = null;
    private Rigidbody2D body;
    private CollisionCheck ground;

    [Header("Jump Values")]
    [SerializeField, Range(0f, 30f)] private float jumpHeight = 0f;
    public bool IsJumpingThisFrame { get; private set; }
    private float jumpHeightOnLastCalculation;
    private float jumpForce;

    [SerializeField, Range(0f, 2f)] private float jumpBuffer = 0.2f;
    private float jumpBufferLeft;

    [SerializeField, Range(0f, 2f)] private float coyoteTime = 0.2f;
    private float coyoteTimeLeft;

    [Header("Gravity Multipliers")]
    [SerializeField, Range(0f, 20f)] private float downwardGravityMultiplier = 2f;
    [SerializeField, Range(0f, 20f)] private float upwardGravityMultiplier = 0.8f;
    [SerializeField, Range(0f, 1f)] private float jumpReleaseMultiplier = 0.5f;
    private bool isRising;
    private float defaultGravityScale;

    private Vector2 velocity;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<CollisionCheck>();

        defaultGravityScale = body.gravityScale;

        CalculateJumpForce();
    }

    private void Update()
    {
        if (inputController.GetJumpPressed())
        {
            jumpBufferLeft = jumpBuffer;
        }
        else
        {
            jumpBufferLeft -= Time.deltaTime;
        }


        if (ground.Ground)
        {
            coyoteTimeLeft = coyoteTime;
        }
        else
        {
            coyoteTimeLeft -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        IsJumpingThisFrame = false;

        if (jumpHeightOnLastCalculation != jumpHeight)
        {
            Debug.Log("Jump Height Value Changed. Recalculating Jump Force...");
            CalculateJumpForce();
        }

        if (IsActive)
        {
            velocity = body.velocity;

            CalculateGravity();

            if (jumpBufferLeft > 0f && coyoteTimeLeft > 0f)
            {
                DoJump();
                jumpBufferLeft = 0f;
                coyoteTimeLeft = 0f;
            }

            if (isRising && !inputController.GetJumpHeld())
            {
                velocity.y *= jumpReleaseMultiplier;
                isRising = false;
            }

            body.velocity = velocity;
        }
    }

    private void DoJump()
    {
        velocity.y = jumpForce;
        isRising = true;
        IsJumpingThisFrame = true;
    }

    private void CalculateJumpForce()
    {    
        jumpForce = Mathf.Max(Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight), 0f);
        jumpHeightOnLastCalculation = jumpHeight;
    }

    private void CalculateGravity()
    {
        if (body.velocity.y > 0f)
        {
            body.gravityScale = defaultGravityScale * upwardGravityMultiplier;
        }
        else if (body.velocity.y < 0f)
        {
            body.gravityScale = defaultGravityScale * downwardGravityMultiplier;
            isRising = false;
        }
        else if (body.velocity.y == 0f)
        {
            body.gravityScale = defaultGravityScale;
        }
    }


    // This Capability overrides the Disable() method so that it can run certain code while dormant, such as jumpBuffer and coyoteTime
    public override void Disable()
    {
        IsActive = false;
        isRising = false;
    }
}
