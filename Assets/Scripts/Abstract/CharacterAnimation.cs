using UnityEngine;

public abstract class CharacterAnimation : MonoBehaviour
{
    [Header("Character References")]
    [SerializeField] protected GameObject character;
    protected Rigidbody2D body;
    protected Capability[] capabilities;
    protected CharacterInfo characterInfo;

    [Header("Animation References")]
    [SerializeField] protected AnimationHandler[] animations;
    protected Animator animator;

    [Space]
    [SerializeField] protected float smoothTime;

    public virtual Vector2 Velocity => body.velocity;
    public virtual string LastAnimationPlayed { get; protected set; }

    protected bool isExitingAnimation;
    protected float queuedExitTime;

    protected Vector3 targetLocalPosition;
    protected Vector3 smoothingVelocity;

    public virtual bool TryGetCapability(out Capability capability, System.Type type)
    {
        for (int i = 0; i < capabilities.Length; i++)
        {
            if (capabilities[i].GetType() == type)
            {
                capability = capabilities[i];
                return true;
            }
        }

        capability = null;
        return false;
    }

    public abstract bool TryGetCheck(out CollisionCheck check, System.Type type);

    public virtual void Start()
    {
        body = character.GetComponent<Rigidbody2D>();
        characterInfo = character.GetComponent<CharacterInfo>();

        capabilities = characterInfo.Capabilities;

        animator = GetComponent<Animator>();

        for (int i = 0; i < animations.Length; i++)
        {
            animations[i].SetCharacterAnimator(this);
        }
    }

    public virtual void Update()
    {
        FlipTowardsMovement();
        CalculateLocalPosition();
        CheckAnimations();
    }

    public virtual void FlipTowardsMovement()
    {
        transform.localScale = new Vector3
        (
            characterInfo.Facing * Mathf.Abs(transform.localScale.x),
            transform.localScale.y, 
            transform.localScale.z
        );
    }

    public virtual void CalculateLocalPosition()
    {
        transform.localPosition = Vector3.SmoothDamp
        (
            transform.localPosition, 
            targetLocalPosition, 
            ref smoothingVelocity, 
            smoothTime, 
            Mathf.Infinity, 
            Time.deltaTime
        );
    }

    public virtual void CheckAnimations()
    {
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
    }

    public virtual void PlayAnimation(AnimationHandler animation)
    {
        if (LastAnimationPlayed != animation.name && queuedExitTime > 0f)
        {
            isExitingAnimation = true;
        }
        else
        {
            targetLocalPosition = animation.GetLocalPositionOverride();
            animator.speed = Mathf.Min(animation.GetAnimationSpeed(), 10f);

            animator.Play(animation.name);
            LastAnimationPlayed = animation.name;
            queuedExitTime = animation.ExitTime;
        }
    }
}
