using UnityEngine;
public abstract class AnimationHandler : ScriptableObject
{
    protected CharacterAnimation cAnim;

    [Space]

    [SerializeField] protected float exitTime;
    public virtual float ExitTime => exitTime;

    [SerializeField] protected Vector3 localPositionOverride = Vector3.zero;

    [Space]
    [Space]

    [SerializeField] protected string[] potentialLastAnimations;

    [Space]

    [SerializeField] protected string[] excludedLastAnimations;

    protected bool isAnimationValidOverride = true;


    public virtual void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        isAnimationValidOverride = true;
        cAnim = characterAnimation;
    }

    public virtual bool IsAnimationValid()
    {
        return LastAnimationIsIncluded() && LastAnimationIsNotExcluded() && isAnimationValidOverride;
    }

    public virtual bool LastAnimationIsIncluded()
    {
        if (potentialLastAnimations == null || potentialLastAnimations.Length < 1)
        {
            return true;
        }
        for (int i = 0; i < potentialLastAnimations.Length; i++)
        {
            if (potentialLastAnimations[i] == cAnim.LastAnimationPlayed)
            {
                return true;
            }
        }
        return false;
    }

    public virtual bool LastAnimationIsNotExcluded()
    {
        if (excludedLastAnimations == null || excludedLastAnimations.Length < 1)
        {
            return true;
        }

        for (int i = 0; i < excludedLastAnimations.Length; i++)
        {
            if (excludedLastAnimations[i] == cAnim.LastAnimationPlayed)
            {
                return false;
            }
        }

        return true;
    }

    public virtual float GetAnimationSpeed()
    {
        return 1f;
    }

    public virtual Vector3 GetLocalPositionOverride()
    {
        return localPositionOverride;
    }
}