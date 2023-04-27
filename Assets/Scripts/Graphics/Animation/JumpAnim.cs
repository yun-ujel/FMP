using UnityEngine;

[CreateAssetMenu(fileName = "JumpEntry", menuName = "Scriptable Object/Animation Handler/Jump Entry")]
public class JumpAnim : AnimationHandler
{
    [SerializeField] private float duration = 0.08f;
    private Jump jump;
    [SerializeField] private bool lockToStartPosition;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);
        jump = (Jump)cAnim.GetCapability(typeof(Jump));
    }

    public override bool IsAnimationValid()
    {
        return base.IsAnimationValid() && jump.TimeSinceLastJump < duration;
    }

    public override Vector3 GetLocalPositionOverride()
    {
        return lockToStartPosition ?
            (Vector3)jump.JumpStartPosition - (cAnim.transform.parent.position)
            : base.GetLocalPositionOverride();
    }
}
