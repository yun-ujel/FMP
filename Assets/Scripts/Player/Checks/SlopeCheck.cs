using UnityEngine;

namespace Platforming.Collision
{
    [CreateAssetMenu(fileName = "Slope Check", menuName = "Scriptable Object/Collision Check/Slope Check")]
    public class SlopeCheck : CollisionCheck
    {
        [SerializeField, Range(0f, 1f)] private float minSlopeNormalY = 0.1f;
        [SerializeField, Range(0f, 1f)] private float maxSlopeNormalY = 0.9f;

        [Space]

        [SerializeField] private LayerMask groundLayer;

        public bool OnSlope { get; private set; } = false;
        public float SlopeFacing { get; private set; } = 0f;
        // The Direction the slope is facing. Between -1f, 0f and 1f

        private Vector3 slopeNormal;
        // The normal of the first contact that is sloped.

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
                OnSlope = EvaluateContact(collision, i, out slopeNormal);

                if (OnSlope)
                {
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

        private bool EvaluateContact(Collision2D collision, int index, out Vector3 normal)
        {
            Vector3 direction = collision.GetContact(index).point - (Vector2)collision.otherCollider.transform.position;
            normal = Vector3.up;

            RaycastHit2D hit = Physics2D.Raycast
            (
                collision.otherCollider.transform.position,
                direction,
                6f,
                groundLayer
            );

            return hit.normal.y < maxSlopeNormalY && hit.normal.y >= minSlopeNormalY;
        }
    }
}
