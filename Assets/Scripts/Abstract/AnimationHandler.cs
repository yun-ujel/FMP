using UnityEngine;
public abstract class AnimationHandler : ScriptableObject
{
    protected CharacterAnimation cAnim;

    [SerializeField] protected Vector3 localPositionOverride = Vector3.zero;

    [SerializeField] protected string[] potentialLastAnimations;

    public virtual void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        cAnim = characterAnimation;
    }

    public virtual bool IsAnimationValid()
    {
        return LastAnimationIsMatching();
    }

    public virtual bool LastAnimationIsMatching()
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

    public virtual float GetAnimationSpeed()
    {
        return 1f;
    }

    public virtual Vector3 GetLocalPositionOverride()
    {
        return localPositionOverride;
    }
}