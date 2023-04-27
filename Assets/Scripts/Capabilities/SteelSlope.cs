using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SteelSlope : Capability
{
    //[Header("Collision Checks")]
    [Space]
    [SerializeField] private SlopeCheck slopeCheck;
    [SerializeField] private WallCheck wallCheck;
    private Rigidbody2D body;

    public bool IsSliding { get; private set; } = false;
    private float slideFacing;

    private Vector2 moveDirection;
    public Vector2 MoveDirection => moveDirection;

    public float LastMoveAngle { get; private set; }
    public float CurrentMoveAngle { get; private set; }


    [Header("References")]
    [SerializeField] private Capability[] abilitiesDuringSlide;
    private Move move;

    [Header("Speed Values")]
    [SerializeField, Range(0f, 40f)] private float minSlideSpeed = 6f;
    [SerializeField, Range(0f, 40f)] private float maxSlideSpeed = 16f;

    [Space]

    [SerializeField, Range(1f, 100f)] private float slideAccelerationMultiplier = 30f;

    [SerializeField]private float slideAcceleration;
    private float slideSpeed = 6f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        _ = TryGetComponent(out move);
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, MoveDirection * -slopeCheck.SlopeFacing, Color.red);
        Debug.DrawRay(transform.position, Vector2.up, Color.cyan);

        if (slopeCheck.OnSlope && !IsSliding)
        {
            InitiateSlide();
        }

        if (IsSliding && !slopeCheck.AnyCollision)
        {
            LastMoveAngle = -90f;
        }

        if (wallCheck.Wall && IsSliding)
        {
            FinishSlide();
        }
    }
    private void FixedUpdate()
    {
        if (IsSliding)
        {
            slideSpeed = Mathf.MoveTowards(slideSpeed, maxSlideSpeed, slideAcceleration * Time.fixedDeltaTime);

            body.velocity = new Vector2(moveDirection.x * slideSpeed, body.velocity.y);
        }        
    }
    private void InitiateSlide()
    {
        IsSliding = true;
        slideFacing = slopeCheck.SlopeFacing;
        if (move != null)
        {
            move.Facing = slideFacing;
        }

        slideSpeed = Mathf.Clamp(Mathf.Abs(body.velocity.y), minSlideSpeed, maxSlideSpeed);
        RecalculateMoveDirection();

        DisableOtherCapabilitiesExcept(abilitiesDuringSlide);
    }

    private void FinishSlide()
    {
        IsSliding = false;

        EnableOtherCapabilities();
    }

    private void RecalculateMoveDirection()
    {
        LastMoveAngle = CurrentMoveAngle;

        moveDirection = slopeCheck.GetSlopeDirection() * slideFacing;

        if (moveDirection.y <= 0f)
        {
            slideAcceleration = Mathf.Abs(moveDirection.y) * slideAccelerationMultiplier;
        }

        CurrentMoveAngle = Vector2.Angle(moveDirection / slideFacing, Vector2.up * -slopeCheck.SlopeFacing);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsSliding && slopeCheck.AnyCollision)
        {
            RecalculateMoveDirection();
        }
    }
}
