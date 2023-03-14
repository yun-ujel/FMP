using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Dash : Capability
{
    private Vector2 dashDirection;
    private bool desiredDash;

    [Header("References")]
    private Rigidbody2D body;
    private GravityMultiplier gravityMultiplier;

    [SerializeField] private GroundCheck groundCheck;

    public bool IsDashingThisFrame { get; private set; }

    [Header("Values")]
    [SerializeField, Range(0f, 50f)] private float dashSpeed = 32f;
    [SerializeField, Range(0f, 10f)] private float dashDrag = 8f;
    private float defaultDrag;

    [Space]

    [SerializeField] private float timeSpentImmobile = 0.15f;// Time spent being unable to move/alter trajectory while dashing, counts downwards
    private bool isImmobile;
    private float timeSpentNoGravity = 0.2f;
    private float timeSinceLastDash;

    [Space]

    [SerializeField, Range(0f, 5f)] private int numberOfDashes = 1;
    private int dashesSpent = 0;
    [SerializeField, Range(0f, 1f)] private float dashCooldown = 0.4f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        TryGetComponent(out gravityMultiplier);

        dashDirection = new Vector2(1f, 0f);
        defaultDrag = body.drag;
    }

    private void Update()
    {
        CalculateDashDirection();

        if (inputController.GetAttackPressed())
        {
            desiredDash = true;
        }

        if (isImmobile && (timeSinceLastDash >= timeSpentImmobile || groundCheck.AnyCollision))
        {
            DisableImmobility();
        }

        if (groundCheck.OnGround)
        {
            dashesSpent = 0;
        }


        if (!gravityMultiplier.enabled && timeSinceLastDash >= timeSpentNoGravity)
        {
            EnableGravity();
        }

        timeSinceLastDash += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        IsDashingThisFrame = false;

        if (desiredDash && dashesSpent < numberOfDashes && timeSinceLastDash > dashCooldown)
        {
            DoDash();
        }
        else
        {
            desiredDash = false;
        }
    }

    private void DoDash()
    {
        timeSinceLastDash = 0f;

        EnableImmobility();

        body.velocity = dashDirection * dashSpeed;

        dashesSpent += 1;

        desiredDash = false;
        IsDashingThisFrame = true;
    }

    private void CalculateDashDirection()
    {
        if (new Vector2(inputController.GetHorizontalInput(), inputController.GetVerticalInput()) != Vector2.zero)
        {
            dashDirection = new Vector2(inputController.GetHorizontalInput(), inputController.GetVerticalInput());
        }

        if (dashDirection.sqrMagnitude > 1f)
        {
            dashDirection.Normalize();
        }
    }

    private void DisableImmobility()
    {
        isImmobile = false;
        EnableOtherCapabilities();
    }

    private void EnableImmobility()
    {
        isImmobile = true;

        gravityMultiplier.enabled = false;
        body.gravityScale = 0f;
        body.drag = dashDrag;

        DisableOtherCapabilities();
    }

    void EnableGravity()
    {
        gravityMultiplier.enabled = true;
        body.drag = defaultDrag;
    }

    public override void DisableCapability()
    {
        EnableGravity();
        DisableImmobility();
        base.DisableCapability();
    }
}
