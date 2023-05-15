using UnityEngine;

namespace Platforming.Capabilities
{
    using Collision;

    [RequireComponent(typeof(Rigidbody2D))]
    public class Jump : Capability
    {
        [Header("References")]
        private Rigidbody2D body;
        [SerializeField] private GroundCheck groundCheck;

        public bool IsJumpingThisFrame { get; private set; }
        public bool CanJump { get; set; } = true;

        private Vector2 velocity;

        [Header("Height Values")]
        [SerializeField, Range(0f, 30f)] private float jumpHeight = 7f;
        private float jumpHeightOnLastCalculation;
        private float jumpForce;

        [SerializeField, Range(0f, 1f)] private float jumpReleaseMultiplier = 0.6f;
        private bool isRising;

        [Header("Leniency")]
        [SerializeField, Range(0f, 2f)] private float jumpBuffer = 0.2f;
        private float jumpBufferLeft;

        [SerializeField, Range(0f, 2f)] private float coyoteTime = 0.2f;
        private float coyoteTimeLeft;

        [Space]

        [SerializeField, Range(0, 10)] private int numberOfMidairJumps = 1;
        private int jumpsSpent = 0;

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
                jumpsSpent = 0;
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

                if (jumpBufferLeft > 0f && (coyoteTimeLeft > 0f || jumpsSpent < numberOfMidairJumps))
                {
                    if (CanJump)
                    {
                        DoJump();
                        jumpBufferLeft = 0f;
                        coyoteTimeLeft = 0f;
                    }
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
            jumpsSpent += 1;
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

}