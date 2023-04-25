using UnityEngine;

[CreateAssetMenu(fileName = "Idle", menuName = "Scriptable Object/Animation Handler/Idle")]
public class IdleAnim : AnimationHandler
{
    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);
    }

    public override bool IsAnimationValid()
    {
        return base.IsAnimationValid();
    }
}
