using UnityEngine;

public class WorldCharacterAnimation : MonoBehaviour
{
    [System.Serializable]
    private class WorldAnimationHandler
    {
        public string name;

        [Space]

        [SerializeField] private float xVelocityIsAbove = Mathf.NegativeInfinity;
        [SerializeField] private float xVelocityIsBelow = Mathf.Infinity;

        [Space]

        [SerializeField] private float yVelocityIsBelow = Mathf.Infinity;
        [SerializeField] private float yVelocityIsAbove = Mathf.NegativeInfinity;

        public bool IsAnimationValid(Vector2 velocity)
        {
            return velocity.y < yVelocityIsBelow && velocity.y > yVelocityIsAbove
                && Mathf.Abs(velocity.x) > xVelocityIsAbove && Mathf.Abs(velocity.x) < xVelocityIsBelow;
        }
    }

    [Header("References")]
    [SerializeField] private GameObject character;
    [SerializeField] private Animator animator;
    private Rigidbody2D body;
    private IsometricMovement movement;

    [Space]

    [SerializeField] private WorldAnimationHandler[] animations;

    private float lastSqrMagnitude;
    private float direction = 1f;

    private string lastAnimationPlayed = "Fwd";

    private void Start()
    {
        movement = character.GetComponent<IsometricMovement>();
        body = character.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        FlipTowardsMovement();

        if (body.velocity.sqrMagnitude > 0f)
        {
            for (int i = 0; i < animations.Length; i++)
            {
                if (animations[i].IsAnimationValid(body.velocity / movement.maxSpeed))
                {
                    animator.Play("Walk_" + animations[i].name);
                    lastAnimationPlayed = animations[i].name;
                    break;
                }
            }
        }
        else
        {
            animator.Play("Idle_" + lastAnimationPlayed);
        }
    }

    private void FlipTowardsMovement()
    {
        if (body.velocity.sqrMagnitude >= lastSqrMagnitude && body.velocity.sqrMagnitude > 0f)
        {
            direction = body.velocity.x > 0f ? 1f : -1f;
        }

        transform.localScale = new Vector3(direction * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        lastSqrMagnitude = body.velocity.sqrMagnitude;
    }
}
