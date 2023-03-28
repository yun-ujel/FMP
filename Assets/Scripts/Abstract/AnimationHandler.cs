using UnityEngine;


public abstract class AnimationHandler : ScriptableObject
{
    protected CharacterAnimation cAnim;

    public virtual void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        cAnim = characterAnimation;
    }

    public abstract bool IsAnimationValid();

    public virtual float GetAnimationSpeed()
    {
        return 1f;
    }
}
