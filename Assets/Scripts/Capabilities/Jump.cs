using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Jump : Capability
{
    [Header("References")]
    private Rigidbody2D body;
    [SerializeField] private GroundCheck groundCheck;

    public bool IsJumpingThisFrame { get; private set; }

    public float TimeSinceLastJump { get; private set; }

    private Vector2 velocity;

    [Header("Height Values")]
    [SerializeField, Range(0f, 30f)] private float jumpHeight = 7f;
    private float jumpHeightOnLastCalculation;
    private float jumpForce;

    [SerializeField, Range(0f, 1f)] private float jumpReleaseMultiplier = 0.6f;
    private bool isRising;
    [SerializeField] private bool doubleJumpIsVariable;

    [Header("Leniency")]

    [SerializeField, Range(0f, 2f)] private float jumpBuffer = 0.2f;
    private float jumpBufferLeft;

    [SerializeField, Range(0f, 2f)] private float coyoteTime = 0.2f;
    private float coyoteTimeLeft;
    private bool hasCoyoteTime;

    [Space]

    [SerializeField, Range(0, 10)] private int numberOfJumps = 1;
    public int JumpsSpent { get; private set; } = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        IsActive = true;

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

        if (groundCheck.OnGround && !IsJumpingThisFrame)
        {
            coyoteTimeLeft = coyoteTime;
            hasCoyoteTime = true;
            JumpsSpent = 0;
        }
        else if (coyoteTimeLeft > 0f)
        {
            coyoteTimeLeft -= Time.deltaTime;
        }
        else if (hasCoyoteTime)
        {
            hasCoyoteTime = false;
            JumpsSpent += 1;
        }

        

        TimeSinceLastJump += Time.deltaTime;
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

            if (jumpBufferLeft > 0f && (coyoteTimeLeft > 0f || JumpsSpent < numberOfJumps))
            {
                DoJump();
            }

            if (body.velocity.y < 0f)
            {
                isRising = false;
            }

            if (isRising && !inputController.GetJumpHeld() && (doubleJumpIsVariable || JumpsSpent <= 1) && !IsJumpingThisFrame)
            {
                velocity.y *= jumpReleaseMultiplier;
                isRising = false;
            }

            body.velocity = velocity;
        }
    }

    public void DoJump()
    {        
        velocity.y = jumpForce;
        isRising = true;
        IsJumpingThisFrame = true;
        JumpsSpent += 1;

        TimeSinceLastJump = 0f;

        jumpBufferLeft = 0f;
        coyoteTimeLeft = 0f;
        hasCoyoteTime = false;
    }

    private void CalculateJumpForce()
    {
        jumpForce = Mathf.Max(Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight), 0f);
        jumpHeightOnLastCalculation = jumpHeight;
    }

    // This Capability overrides the Disable() method so that it can run certain code while dormant, such as jumpBuffer and coyoteTime
    public override void DisableCapability()
    {
        IsActive = false;
        isRising = false;
    }
}
