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
    [SerializeField, Range(0f, 1f)] private float jumpReleaseMultiplier = 0.5f;
    public bool IsJumpingThisFrame { get; private set; }

    private float jumpHeightOnLastCalculation;
    private float jumpForce;
    private bool isRising;

    [SerializeField, Range(0f, 2f)] private float jumpBuffer = 0.2f;
    private float jumpBufferLeft;

    [SerializeField, Range(0f, 2f)] private float coyoteTime = 0.2f;
    private float coyoteTimeLeft;

    private Vector2 velocity;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<CollisionCheck>();

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


        if (ground.Ground && !IsJumpingThisFrame)
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

            if (jumpBufferLeft > 0f && coyoteTimeLeft > 0f)
            {
                DoJump();
                jumpBufferLeft = 0f;
                coyoteTimeLeft = 0f;
            }

            if (body.velocity.y < 0f)
            {
                isRising = false;
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

    // This Capability overrides the Disable() method so that it can run certain code while dormant, such as jumpBuffer and coyoteTime
    public override void Disable()
    {
        IsActive = false;
        isRising = false;
    }
}
