using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Dash : Capability
{
    [SerializeField] private InputController inputController;
    [SerializeField] private GravityMultiplier gravityMultiplier;
    private Rigidbody2D body;
    private bool desiredDash;
    public bool IsDashingThisFrame { get; private set; }

    [Header("Values")]
    [SerializeField] private float dashSpeed = 10f;

    private Vector2 dashDirection;

    [Space]
    [SerializeField] private float dashCooldown = 0.35f;
    private float dashCooldownCounter;
    // Counts downwards

    [SerializeField] private float dashImmobility = 0.15f;
    private float dashImmobilityCounter;
    private bool hasDashImmobility;

    // Time spent being unable to move/alter trajectory while dashing, counts downwards

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        dashCooldown += dashImmobility;

        dashDirection = new Vector2(1f, 0f);
    }

    private void Update()
    {
        CalculateDashDirection();

        if (inputController.GetAttackPressed())
        {
            desiredDash = true;
        }

        if (dashImmobilityCounter > 0f && hasDashImmobility)
        {
            DisableOtherCapabilities();
            gravityMultiplier.enabled = false;
            dashImmobilityCounter -= Time.deltaTime;
        }
        else if (hasDashImmobility)
        {
            Debug.Log("Dash Immobility Disabled");
            gravityMultiplier.enabled = true;
            hasDashImmobility = false;
            EnableOtherCapabilities();
        }
    }

    private void FixedUpdate()
    {
        IsDashingThisFrame = false;
        if (desiredDash && dashCooldownCounter < 0f)
        {
            DoDash();
        }
        else
        {
            dashCooldownCounter -= Time.deltaTime;
        }
    }

    private void DoDash()
    {
        Debug.Log("Triggered Dash");
        dashCooldownCounter = dashCooldown;

        dashImmobilityCounter = dashImmobility;
        hasDashImmobility = true;

        body.velocity = dashDirection * dashSpeed;

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
}
