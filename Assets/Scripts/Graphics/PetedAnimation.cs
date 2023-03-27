using System.Collections.Generic;
using UnityEngine;

public class PetedAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject playerGameObject;

    [Header("References (optional)")]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private GroundCheck playerGroundCheck;

    [Space]

    [SerializeField] private Animator animator;

    private List<Capability> capabilities;
    private Move move;

    [Header("Speed Values")]
    [SerializeField, Range(0f, 10f)] private float minAnimationSpeed = 1f;
    [SerializeField, Range(0f, 10f)] private float maxAnimationSpeed = 3f;
    [SerializeField] private bool rollAtMaxMoveSpeed;
    private float animationSpeed;

    private void Start()
    {
        if (playerRigidbody == null) 
            playerRigidbody = playerGameObject.GetComponent<Rigidbody2D>();

        if (animator == null) 
            animator = GetComponent<Animator>();

        if (playerGroundCheck == null)
            playerGroundCheck = playerGameObject.GetComponent<GroundCheck>();

        capabilities = new List<Capability>();
        capabilities.AddRange(playerGameObject.GetComponents<Capability>());

        move = (Move)GetCapability(typeof(Move));
    }

    private void Update()
    {
        FlipTowardsMovement();

        if (playerGroundCheck.OnGround)
        {
            if (move != null)
            {
                DoMoveAnimation();
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
    }

    private void FlipTowardsMovement()
    {
        transform.localScale = new Vector3(move.Facing * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private float Remap(float input, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return (input - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin;
    }

    private Capability GetCapability(System.Type type)
    {
        for (int i = 0; i < capabilities.Count; i++)
        {
            if (capabilities[i].GetType() == type)
            {
                return capabilities[i];
            }
        }

        return null;
    }

    private void DoMoveAnimation()
    {
        animationSpeed = Remap(Mathf.Abs(playerRigidbody.velocity.x), 0f, Mathf.Abs(move.DesiredVelocity.x), minAnimationSpeed, maxAnimationSpeed);

        if (animationSpeed == maxAnimationSpeed && rollAtMaxMoveSpeed)
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
}
