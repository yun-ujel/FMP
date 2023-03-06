using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject playerGameObject;

    [Header("References (optional)")]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private CollisionCheck playerCollisionCheck;
    [SerializeField] private Move playerMovement;
    [SerializeField] private Animator animator;

    [Header("Speed Values")]
    [SerializeField, Range(0f, 10f)] private float minAnimationSpeed = 1f;
    [SerializeField, Range(0f, 10f)] private float maxAnimationSpeed = 3f;
    [SerializeField] private bool rollAtMaxSpeed;
    private float animationSpeed;

    private void Start()
    {
        if (playerRigidbody == null) playerRigidbody = playerGameObject.GetComponent<Rigidbody2D>();
        if (playerCollisionCheck == null) playerCollisionCheck = playerGameObject.GetComponent<CollisionCheck>();
        if (playerMovement == null) playerMovement = playerGameObject.GetComponent<Move>();

        if (animator == null) animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerCollisionCheck.Ground)
        {
            animationSpeed = Remap(Mathf.Abs(playerRigidbody.velocity.x), 0f, Mathf.Abs(playerMovement.DesiredVelocity.x), minAnimationSpeed, maxAnimationSpeed);

            if (animationSpeed == maxAnimationSpeed && rollAtMaxSpeed)
            {
                animator.speed = 1f;
                animator.Play("Roll");
            }
            else if (Mathf.Abs(playerRigidbody.velocity.x) > 0f)
            {
                animator.speed = Mathf.Min(animationSpeed, maxAnimationSpeed);
                animator.Play("Walk");
            }
            else
            {
                animator.speed = 1f;
                animator.Play("Idle");
            }
        }
        else
        {
            animator.speed = 0.5f;
            animator.Play("Roll");
        }

        if (playerRigidbody.velocity.x > 0f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (playerRigidbody.velocity.x < 0f)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private float Remap(float input, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return (input - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin;
    }
}
