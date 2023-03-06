using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CollisionCheck))]
public class Move : Capability
{
    //[Header("References")]
    [SerializeField] private InputController inputController = null;
    private Rigidbody2D body;
    private CollisionCheck ground;

    [Header("Speed Values")]
    [SerializeField, Range(0f, 100f)] private float maxSpeed;

    [Header("Ground")]
    [SerializeField, Range(0f, 100f)] private float maxGroundAcceleration;
    [SerializeField, Range(0f, 100f)] private float maxGroundDeceleration;

    [Header("Air")]
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration;
    [SerializeField, Range(0f, 100f)] private float maxAirDeceleration;

    private Vector2 direction;
    public Vector2 DesiredVelocity { get; private set; }
    private Vector2 velocity;

    private float maxSpeedChange;
    private float acceleration;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<CollisionCheck>();
    }

    private void Update()
    {
        direction.x = inputController.GetHorizontalInput();
        DesiredVelocity = new Vector2(direction.x, 0f) * maxSpeed;
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;

        acceleration = ground.Ground
            ? GetIsDecelerating() ? maxGroundDeceleration : maxGroundAcceleration // If On Ground:
            : GetIsDecelerating() ? maxAirDeceleration : maxAirAcceleration; // If In Air:

        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, DesiredVelocity.x, maxSpeedChange);

        body.velocity = velocity;
    }

    private bool GetIsDecelerating()
    {
        if ((direction.x < 0f && velocity.x > 0f) || (direction.x > 0f && velocity.x < 0f))
        {
            // If Direction and Velocity are on opposite sides of 0 (e.g. direction = positive, velocity = negative)
            return true;
        }
        else if (direction.x == 0f)
        {
            return true;
        }

        return false;
    }
}
