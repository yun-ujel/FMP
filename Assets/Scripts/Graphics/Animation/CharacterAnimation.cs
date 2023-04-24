using UnityEngine;
using System.Collections.Generic;
public class CharacterAnimation : MonoBehaviour
{
    [Header("Character References")]
    [SerializeField] private GameObject character;
    private List<Capability> capabilities;
    private AdditionalCharacterInfo characterInfo;
    private CollisionRelay relay;
    private Rigidbody2D body;

    private Move move;

    [Header("Animation References")]
    [SerializeField] private AnimationHandler[] animations;

    private Animator animator;
    public Vector2 Velocity => body.velocity;
    public string LastAnimationPlayed { get; private set; }

    private bool isExitingAnimation;
    private float queuedExitTime;

    private Vector3 targetLocalPosition;
    private float timeSinceLastAnimationStarted;

    private void Start()
    {
        relay = character.GetComponent<CollisionRelay>();
        body = character.GetComponent<Rigidbody2D>();

        if (!character.TryGetComponent(out characterInfo))
        {
            capabilities = new List<Capability>();
            capabilities.AddRange(character.GetComponents<Capability>());
        }
        else
        {
            capabilities = characterInfo.Capabilities;
        }

        animator = GetComponent<Animator>();

        for (int i = 0; i < animations.Length; i++)
        {
            animations[i].SetCharacterAnimator(this);
        }

        move = (Move)GetCapability(typeof(Move));
    }


    private void Update()
    {
        FlipTowardsMovement();        

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetLocalPosition, timeSinceLastAnimationStarted);

        if (!isExitingAnimation)
        {
            for (int i = 0; i < animations.Length; i++)
            {
                if (animations[i].IsAnimationValid())
                {                    
                    PlayAnimation(animations[i]);
                    //if (!isExitingAnimation) { Debug.Log("Started playing animation \"" + animations[i].name + "\" at speed " + animations[i].GetAnimationSpeed()); }
                    break;
                }
            }
        }
        else if (queuedExitTime > 0f)
        {
            queuedExitTime -= Time.deltaTime;
        }
        else
        {
            isExitingAnimation = false;
        }

        timeSinceLastAnimationStarted += Time.deltaTime;
    }

    private void FlipTowardsMovement()
    {
        transform.localScale = new Vector3(move.Facing * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    // Methods for use of AnimationHandlers
    public Capability GetCapability(System.Type type)
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
    public CollisionCheck GetCheck(System.Type type)
    {
        for (int i = 0; i < relay.CollisionChecks.Length; i++)
        {
            if (relay.CollisionChecks[i].GetType() == type)
            {
                return relay.CollisionChecks[i];
            }
        }

        return null;
    }

    private void PlayAnimation(AnimationHandler animation)
    {
        if (LastAnimationPlayed != animation.name && queuedExitTime > 0f)
        {
            isExitingAnimation = true;
        }
        else
        {
            SetTargetLocalPosition(animation.GetLocalPositionOverride());
            animator.speed = Mathf.Min(animation.GetAnimationSpeed(), 10f);

            animator.Play(animation.name);
            LastAnimationPlayed = animation.name;
            queuedExitTime = animation.ExitTime;
        }
    }

    public AdditionalCharacterInfo GetAdditionalCharacterInfo()
    {
        return characterInfo;
    }

    private void SetTargetLocalPosition(Vector3 set)
    {
        if (targetLocalPosition != set)
        {
            timeSinceLastAnimationStarted = 0f;
        }

        targetLocalPosition = set;
    }
}
