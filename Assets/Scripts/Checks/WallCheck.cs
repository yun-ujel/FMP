using UnityEngine;

[CreateAssetMenu(fileName = "Wall Check", menuName = "Scriptable Object/Collision Check/Wall Check")]
public class WallCheck : CollisionCheck
{
    public bool Wall { get; private set; }

    [SerializeField, Range(0f, 1f)] private float minWallNormalX = 0.95f;

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
        Wall = false;
    }

    public override void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Wall |= Mathf.Abs(collision.GetContact(i).normal.x) >= minWallNormalX;
        }
    }

    public override void Initialize()
    {
        Wall = false;
    }
}
