using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CheckSlope : CollisionCheck
{
    [SerializeField, Range(0f, 1f)] private float maxSlopeNormalY = 0.9f;
    // The maximum normal (Y) value for a surface to be classified as a slope

    [SerializeField, Range(0f, 1f)] private float minSlopeNormalY = 0.05f;
    // The minimum normal (Y) value for a surface to be classified as a slope

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
        Slope = false;
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Slope |= collision.GetContact(i).normal.y > minSlopeNormalY && collision.GetContact(i).normal.y < maxSlopeNormalY;
        }
    }
}
