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

    void Start()
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


    void Update()
    {
        FlipTowardsMovement();

        for (int i = 0; i < animations.Length; i++)
        {
            if (animations[i].IsAnimationValid())
            {
                //Debug.Log("Started playing animation \"" + animations[i].name + "\" at speed " + animations[i].GetAnimationSpeed());
                animator.speed = Mathf.Min(animations[i].GetAnimationSpeed(), 10f);
                animator.Play(animations[i].name);

                LastAnimationPlayed = animations[i].name;
                //Debug.Log("Playing animation at index: " + i);

                break;
            }
        }
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

    public AdditionalCharacterInfo GetAdditionalCharacterInfo()
    {
        return characterInfo;
    }
}
