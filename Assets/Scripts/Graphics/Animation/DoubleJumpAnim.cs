using UnityEngine;

[CreateAssetMenu(fileName = "DoubleJump", menuName = "Scriptable Object/Animation Handler/Double Jump")]
public class DoubleJumpAnim : AnimationHandler
{
    [Header("Active When:")]
    [SerializeField] private float yVelocityIsBelow = float.PositiveInfinity;
    [SerializeField] private float yVelocityIsAbove = float.NegativeInfinity;

    [Space]

    [SerializeField] private int onJumpsSpent = 1;

    private Jump jump;
    private GroundCheck groundCheck;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        groundCheck = (GroundCheck)cAnim.GetCheck(typeof(GroundCheck));
        jump = (Jump)cAnim.GetCapability(typeof(Jump));
    }

    public override bool IsAnimationValid()
    {
        return cAnim.Velocity.y < yVelocityIsBelow && cAnim.Velocity.y > yVelocityIsAbove && !groundCheck.OnGround && jump.JumpsSpent >= onJumpsSpent && base.IsAnimationValid();
    }
}