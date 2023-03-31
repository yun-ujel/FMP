using UnityEngine;

[CreateAssetMenu(fileName = "Walk", menuName = "Scriptable Object/Animation Handler/Ground Walk")]
public class WalkAnim : AnimationHandler
{
    private Move move;
    private GroundCheck groundCheck;

    [Header("Speed Values")]
    [SerializeField, Range(0f, 10f)] private float minAnimationSpeed = 0.8f;
    [SerializeField, Range(0f, 10f)] private float maxAnimationSpeed = 1.6f;

    [Header("Active When:")]
    [SerializeField, Range(0f, 10f)] private float xVelocityIsAbove = 0f;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        move = (Move)cAnim.GetCapability(typeof(Move));
        groundCheck = (GroundCheck)cAnim.GetCheck(typeof(GroundCheck));
    }

    public override bool IsAnimationValid()
    {
        return (groundCheck.OnGround && move.enabled && Mathf.Abs(cAnim.Velocity.x) > 0f
            && Mathf.Abs(cAnim.Velocity.x) > xVelocityIsAbove) && base.IsAnimationValid();
    }

    public override float GetAnimationSpeed()
    {
        return Mathf.Abs(cAnim.Velocity.x).Remap(0f, Mathf.Abs(move.DesiredVelocity.x), minAnimationSpeed, maxAnimationSpeed);
    }
}