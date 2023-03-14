using UnityEngine;

[CreateAssetMenu(fileName = "Slope Check", menuName = "Scriptable Object/Collision Check/Slope Check")]
public class SlopeCheck : CollisionCheck
{
    [SerializeField, Range(0f, 1f)] private float minSlopeNormalY = 0.1f;
    [SerializeField, Range(0f, 1f)] private float maxSlopeNormalY = 0.9f;

    public bool OnSlope { get; private set; } = false;
    public float SlopeFacing { get; private set; } = 0f;
    // The Direction the slope is facing. Between -1f, 0f and 1f

    private Vector2 slopeNormal;
    // The normal of the first contact that is sloped.
    // May be worth changing the calculation to a raycast,
    // so that circle/capsule colliders won't see sharp corners as slopes due to their own curved edges


    private ContactPoint2D evaluateContact;

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

        OnSlope = false;
        SlopeFacing = 0f;
    }

    public override void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            evaluateContact = collision.GetContact(i);

            OnSlope = evaluateContact.normal.y < maxSlopeNormalY && evaluateContact.normal.y >= minSlopeNormalY;
            if (OnSlope)
            {
                slopeNormal = evaluateContact.normal;
                SlopeFacing = slopeNormal.x > 0f ? 1f : -1f;
                break;
            }
        }
    }

    public Vector2 GetSlopeDirection()
    {
        return !OnSlope ? // If On Slope
            new Vector2(1f, 0f)
            : // Else
            (Vector2)(Vector3.ProjectOnPlane(Vector3.down, slopeNormal).normalized * SlopeFacing);
    }

    public override void Initialize()
    {
        OnSlope = false;
        SlopeFacing = 0f;
    }
}
