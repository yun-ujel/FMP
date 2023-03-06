using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CheckWallHit : MonoBehaviour
{
    public bool WallHit { get; private set; }
    [SerializeField, Range(0f, 1f)] private float minWallNormalX = 0.9f;
    // The minimum normal (X) value for a surface to be classified as a wall
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        WallHit = false;
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            WallHit |= Mathf.Abs(normal.x) >= minWallNormalX;
        }
    }
}
