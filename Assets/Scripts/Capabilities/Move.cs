using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Move : Capability
{
    //[Header("References")]
    private Rigidbody2D body;
    [SerializeField] private GroundCheck groundCheck;

    [Header("Speed Values")]
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 5f;

    [Header("Ground")]
    [SerializeField, Range(0f, 100f)] private float maxGroundAcceleration = 40f;
    [SerializeField, Range(0f, 100f)] private float maxGroundDeceleration = 80f;

    [Header("Air")]
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 60f;
    [SerializeField, Range(0f, 100f)] private float maxAirDeceleration = 80f;

    private Vector2 direction;
    public float Facing { get; set; } = 1f;

    public Vector2 DesiredVelocity { get; private set; }
    private Vector2 velocity;

    public Vector2 Direction => direction;

    private float maxSpeedChange;
    private float acceleration;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        IsActive = true;
    }

    private void Update()
    {
        direction.x = inputController.GetHorizontalInput();
        Facing = Mathf.Abs(direction.x) > 0f ? direction.x : Facing;
        if (Mathf.Abs(direction.x) > 0f)
        {
            Facing = direction.x > 0 ? 1f : -1f;
        }

        DesiredVelocity = new Vector2(direction.x, 0f) * maxSpeed;
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;

        acceleration = GetAcceleration();

        maxSpeedChange = acceleration * Time.fixedDeltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, DesiredVelocity.x, maxSpeedChange);

        body.velocity = velocity;
    }

    private float GetAcceleration()
    {
        if ((direction.x < 0f && velocity.x > 0f) || (direction.x > 0f && velocity.x < 0f))
        {
            return groundCheck.OnGround ? maxGroundDeceleration : maxAirDeceleration;
        }
        else if (direction.x == 0f)
        {
            return groundCheck.OnGround ? maxGroundDeceleration : maxAirDeceleration;
        }

        return groundCheck.OnGround ? maxGroundAcceleration : maxAirAcceleration;
    }
}
