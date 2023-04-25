using UnityEngine;

[CreateAssetMenu(fileName = "Compound", menuName = "Scriptable Object/Animation Handler/Compound Animation Handler")]
public class CompoundAnimationHandler : AnimationHandler
{
    [SerializeField] private AnimationHandler[] animationHandlers;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        for (int i = 0; i < animationHandlers.Length; i++)
        {
            animationHandlers[i].SetCharacterAnimator(cAnim);
        }
    }
    public override bool IsAnimationValid()
    {
        for (int i = 0; i < animationHandlers.Length; i++)
        {
            if (!animationHandlers[i].IsAnimationValid())
            {
                return false;
            }
        }
        return base.IsAnimationValid();
    }
}
