using UnityEngine;

[CreateAssetMenu(fileName = "Ground", menuName = "Scriptable Object/Animation Handler/Ground")]
public class GroundAnim : AnimationHandler
{
    private Move move;
    private GroundCheck groundCheck;

    [Header("Active When:")]
    [SerializeField, Range(0f, 10f)] private float xVelocityIsAbove = Mathf.NegativeInfinity;
    [SerializeField, Range(0f, 10f)] private float xVelocityIsBelow = Mathf.Infinity;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        move = (Move)cAnim.GetCapability(typeof(Move));
        groundCheck = (GroundCheck)cAnim.GetCheck(typeof(GroundCheck));
    }

    public override bool IsAnimationValid()
    {
        return

            groundCheck.OnGround && move.enabled
            && Mathf.Abs(cAnim.Velocity.x) > xVelocityIsAbove && Mathf.Abs(cAnim.Velocity.x) < xVelocityIsBelow

        && base.IsAnimationValid();
    }

    public override float GetAnimationSpeed()
    {
        return base.GetAnimationSpeed();
    }
}