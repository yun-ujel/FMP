using UnityEngine;

[CreateAssetMenu(fileName = "Ground Check", menuName = "Scriptable Object/Collision Check/Ground Check")]
public class GroundCheck : CollisionCheck
{
    public bool OnGround { get; private set; }

    [SerializeField, Range(0f, 1f)] private float minGroundNormalY = 0.9f;
    // The minimum normal (Y) value for a surface to be classified as ground
    public override void CollisionEnter(Collision2D collision)
    {
        base.CollisionEnter(collision);
        EvaluateCollision(collision);
    }

    public override void CollisionStay(Collision2D collision)
    {
        base.CollisionStay(collision);
        EvaluateCollision(collision);
    }

    public override void CollisionExit(Collision2D collision)
    {
        base.CollisionExit(collision);
        OnGround = false;
    }

    public override void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            OnGround |= collision.GetContact(i).normal.y >= minGroundNormalY;
        }
    }

    public override void Initialize()
    {
        OnGround = false;
    }
}