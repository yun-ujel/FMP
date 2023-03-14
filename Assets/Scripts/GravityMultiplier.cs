using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityMultiplier : MonoBehaviour
{
    private Rigidbody2D body;

    [SerializeField, Range(0f, 20f)] private float downwardGravityMultiplier = 5f;
    [SerializeField, Range(0f, 20f)] private float upwardGravityMultiplier = 3f;
    private float defaultGravityScale;

    // Because of how Gravity values are set on the Rigidbody, 
    // the GravityMultiplier must be disabled if any other script wants to alter gravity values

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

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
