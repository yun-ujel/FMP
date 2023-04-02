using UnityEngine;

[CreateAssetMenu(fileName = "JumpEntry", menuName = "Scriptable Object/Animation Handler/Jump Entry")]
public class JumpAnim : AnimationHandler
{
    [SerializeField] private float duration = 0.08f;
    private Jump jump;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);
        jump = (Jump)cAnim.GetCapability(typeof(Jump));
    }

    public override bool IsAnimationValid()
    {
        return base.IsAnimationValid() && jump.TimeSinceLastJump < duration;
    }
}
