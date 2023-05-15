using UnityEngine;
using Platforming.Capabilities;
using Platforming.Collision;

namespace Graphics.Animation
{
    public class MindCharacterAnimation : MonoBehaviour
    {
        [System.Serializable]
        private class CharacterReferences
        {
            [SerializeField] private GameObject gameObject;
            [SerializeField] private Rigidbody2D rigidbody2D;
            [SerializeField] private GroundCheck groundCheck;
            [SerializeField] private Move move;

            public Vector2 Velocity => rigidbody2D.velocity;
            public bool OnGround => groundCheck.OnGround;
            public float Facing => move.Facing;

            public void Start()
            {
                if (rigidbody2D == null)
                {
                    rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                }

                if (groundCheck == null && gameObject.GetComponent<CollisionRelay>().TryGetCheck
                    (out CollisionCheck collisionCheck, typeof(GroundCheck)))
                {
                    groundCheck = (GroundCheck)collisionCheck;
                }

                if (move == null)
                {
                    Debug.Log(Capability.TryGetCapability(out Capability capability, typeof(Move), gameObject));
                    move = (Move)capability;
                }
            }
        }

        [SerializeField] private CharacterReferences character;

        [SerializeField] private Animator animator;

        private void Start()
        {
            if (animator == null) { animator = GetComponent<Animator>(); }
            character.Start();
        }

        private void Update()
        {
            animator.SetFloat("VelocityX", Mathf.Abs(character.Velocity.x));
            animator.SetFloat("VelocityY", character.Velocity.y);
            animator.SetBool("OnGround", character.OnGround);

            FlipTowardsMovement();
        }

        private void FlipTowardsMovement()
        {
            transform.localScale = new Vector3
            (
                character.Facing * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

}