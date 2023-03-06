using UnityEngine;

// ---SETUP INFO---
// In order to stop character from getting caught on ledges,
// Add an EdgeCollider as well as your existing collider (preferably a Box Collider)
// Set the EdgeCollider to the bottom of your sprite
// Set the other Collider to be finish very slightly above the EdgeCollider (probably around 0.01 units)
// This way the bottom of the Collider won't get stuck on small ledges,
// Or gaps within the floor's colliders

[RequireComponent(typeof(Collider2D))]
public class CheckGround : CollisionCheck
{
    [SerializeField, Range(0f, 1f)] private float minGroundNormalY = 0.9f;
    // The minimum normal (Y) value for a surface to be classified as ground

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
        Ground = false;
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Ground |= collision.GetContact(i).normal.y >= minGroundNormalY;
        }
    }
}
