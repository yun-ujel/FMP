using UnityEngine;

namespace Graphics.Animation
{
    public class WorldCharacterAnimation : MonoBehaviour
    {
        [System.Serializable]
        private class CharacterReferences
        {
            [SerializeField] private GameObject gameObject;
            [SerializeField] private Rigidbody2D rigidbody2D;
            [SerializeField] private IsometricMovement move;

            public Vector2 Direction => move.Direction;
            public float SqrVelocity => rigidbody2D.velocity.sqrMagnitude;

            public void Start()
            {
                if (rigidbody2D == null)
                {
                    rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                }

                if (move == null)
                {
                    Debug.Log(Capability.TryGetCapability(out Capability capability, typeof(IsometricMovement), gameObject));
                    move = (IsometricMovement)capability;
                }
            }
        }

        [SerializeField] private CharacterReferences character;
        [SerializeField] private Animator animator;

        private float facing = 1f;

        private void Start()
        {
            if (animator == null) { animator = GetComponent<Animator>(); }
            character.Start();
        }

        private void Update()
        {
            animator.SetFloat("DirectionX", Mathf.Abs(character.Direction.x));
            animator.SetFloat("DirectionY", character.Direction.y);
            animator.SetFloat("SqrVelocity", character.SqrVelocity);

            FlipTowardsMovement();
        }

        private void FlipTowardsMovement()
        {
            if (Mathf.Abs(character.Direction.x) > 0f)
            {
                facing = character.Direction.x > 0 ? 1f : -1f;
            }

            transform.localScale = new Vector3
            (
                facing * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

}