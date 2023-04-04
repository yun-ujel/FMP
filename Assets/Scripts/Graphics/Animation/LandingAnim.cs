using UnityEngine;

[CreateAssetMenu(fileName = "JumpLand", menuName = "Scriptable Object/Animation Handler/Landing")]
public class LandingAnim : AnimationHandler
{
    private AdditionalCharacterInfo characterInfo;
    private GroundCheck groundCheck;
    [SerializeField] private float duration = 0.12f;
    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);
        characterInfo = cAnim.GetAdditionalCharacterInfo();
        groundCheck = (GroundCheck)cAnim.GetCheck(typeof(GroundCheck));
    }

    public override bool IsAnimationValid()
    {
        return base.IsAnimationValid() && characterInfo.TimeSinceLastLanding <= duration && groundCheck.OnGround;
    }
}
