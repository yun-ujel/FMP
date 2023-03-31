using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SteelSlope : Capability
{
    //[Header("Collision Checks")]
    [Space]
    [SerializeField] private SlopeCheck slopeCheck;
    [SerializeField] private WallCheck wallCheck;
    private Rigidbody2D body;

    private bool isSliding = false;
    private float slideFacing;
    private Vector2 moveDirection;

    [Header("References")]
    [SerializeField] private Capability[] abilitiesDuringSlide;

    [Header("Speed Values")]
    [SerializeField, Range(0f, 40f)] private float minSlideSpeed = 6f;
    [SerializeField, Range(0f, 40f)] private float maxSlideSpeed = 16f;

    [Space]

    [SerializeField, Range(1f, 100f)] private float slideAccelerationMultiplier = 30f;

    private float slideAcceleration;
    private float slideSpeed = 6f;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (slopeCheck.OnSlope && !isSliding)
        {
            InitiateSlide();
        }

        if (wallCheck.Wall && isSliding)
        {
            FinishSlide();
        }
    }
    private void FixedUpdate()
    {
        if (isSliding)
        {
            slideSpeed = Mathf.MoveTowards(slideSpeed, maxSlideSpeed, slideAcceleration * Time.deltaTime);

            body.velocity = new Vector2(moveDirection.x * slideSpeed, body.velocity.y);
        }
    }
    private void InitiateSlide()
    {
        Debug.Log("Initiated Slide");

        isSliding = true;
        slideFacing = slopeCheck.SlopeFacing;

        slideSpeed = Mathf.Clamp(Mathf.Abs(body.velocity.y), minSlideSpeed, maxSlideSpeed);
        CalculateMoveDirection();

        DisableOtherCapabilities();

        for (int i = 0; i < abilitiesDuringSlide.Length; i++)
        {
            abilitiesDuringSlide[i].enabled = true;
            abilitiesDuringSlide[i].EnableCapability();
        }

        body.velocity = new Vector2(0f, -minSlideSpeed);
    }

    private void FinishSlide()
    {
        Debug.Log("Ended Slide");
        isSliding = false;

        EnableOtherCapabilities();
    }

    private void CalculateMoveDirection()
    {
        Debug.Log("Recalculating Move Direction");
        moveDirection = slopeCheck.GetSlopeDirection() * slideFacing;

        if (moveDirection.y <= 0f)
        {
            slideAcceleration = Mathf.Abs(moveDirection.y) * slideAccelerationMultiplier;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isSliding)
        {
            CalculateMoveDirection();
        }
    }
}
