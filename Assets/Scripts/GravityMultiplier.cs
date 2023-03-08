using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CollisionCheck))]
public class GravityMultiplier : MonoBehaviour
{
    private Rigidbody2D body;
    private CollisionCheck collision;

    [SerializeField, Range(0f, 20f)] private float downwardGravityMultiplier = 5f;
    [SerializeField, Range(0f, 20f)] private float upwardGravityMultiplier = 3f;
    private float defaultGravityScale;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        collision = GetComponent<CollisionCheck>();

        defaultGravityScale = body.gravityScale;
    }

    private void FixedUpdate()
    {
        CalculateGravity();
    }

    private void CalculateGravity()
    {
        if (body.velocity.y > 0f)
        {
            body.gravityScale = defaultGravityScale * upwardGravityMultiplier;
        }
        else if (body.velocity.y < 0f)
        {
            body.gravityScale = defaultGravityScale * downwardGravityMultiplier;
        }
        else if (body.velocity.y == 0f)
        {
            body.gravityScale = defaultGravityScale;
        }
    }
}
