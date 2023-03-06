using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Jump : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputController inputController = null;
    private Rigidbody2D body;
    private CheckGround ground;

    [Header("Jump Values")]
    [SerializeField, Range(0f, 30f)] private float jumpHeight = 0f;
    [SerializeField, Range(0f, 2f)] private float jumpBuffer = 0.2f;
    private float jumpForce;
    private float jumpHeightOnLastCalculation;
    private float jumpBufferCounter;

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
        ground = GetComponent<CheckGround>();

        defaultGravityScale = body.gravityScale;

        CalculateJumpForce();
    }

    private void Update()
    {
        if (inputController.GetJumpPressed())
        {
            jumpBufferCounter = jumpBuffer;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (jumpHeightOnLastCalculation != jumpHeight)
        {
            Debug.Log("Jump Height Value Changed. Recalculating Jump Force...");
            CalculateJumpForce();
        }
        velocity = body.velocity;

        if (jumpBufferCounter > 0f && ground.isOnGround)
        {
            DoJump();
            jumpBufferCounter = 0f;
        }

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

        if (isRising && !inputController.GetJumpHeld())
        {
            velocity.y *= jumpReleaseMultiplier;
            isRising = false;
        }

        body.velocity = velocity;
    }

    private void DoJump()
    {
        velocity.y += jumpForce;
        isRising = true;
    }

    private void CalculateJumpForce()
    {    
        jumpForce = Mathf.Max(Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight), 0f);
        jumpHeightOnLastCalculation = jumpHeight;
    }
}
