using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CheckNormals : CollisionCheck
{
    [SerializeField, Range(0f, 1f)] private float minGroundNormalY = 0.9f;
    // The minimum normal (Y) value for a surface to be classified as ground, rather than a slope (lower)

    [SerializeField, Range(0f, 1f)] private float minSlopeNormalY = 0.1f;
    // The minimum normal (Y) value for a surface to be classified as a slope, rather than a wall (lower)

    private float normalY;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        SetFalse();
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            normalY = collision.GetContact(i).normal.y;

            Ground |= normalY >= minGroundNormalY;
            Slope |= normalY >= minSlopeNormalY && collision.GetContact(i).normal.y < minGroundNormalY;
            Wall |= normalY < minSlopeNormalY;
        }
    }
}
