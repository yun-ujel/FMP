using UnityEngine;

// ---SETUP INFO---
// In order to stop character from getting caught on ledges,
// Add an EdgeCollider as well as your existing collider (preferably a Box Collider)
// Set the EdgeCollider to the bottom of your sprite
// Set the other Collider to be finish very slightly above the EdgeCollider (probably around 0.01 units)
// This way the bottom of the Collider won't get stuck on small ledges,
// Or gaps within the floor's colliders

[RequireComponent(typeof(Collider2D))]
public class CheckGround : MonoBehaviour
{
    public bool isOnGround { get; private set; }
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
        isOnGround = false;
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            isOnGround |= normal.y >= 0.9f;
        }
    }
}
