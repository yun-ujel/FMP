using UnityEngine;

[CreateAssetMenu(fileName = "GroundWalk", menuName = "Scriptable Object/Animation Handler/Ground Walk")]
public class GroundWalkAnim : AnimationHandler
{
    private Move move;
    private GroundCheck groundCheck;

    [SerializeField, Range(0f, 10f)] private float minAnimationSpeed = 0.8f;
    [SerializeField, Range(0f, 10f)] private float maxAnimationSpeed = 1.6f;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        move = (Move)cAnim.GetCapability(typeof(Move));
        groundCheck = (GroundCheck)cAnim.GetCheck(typeof(GroundCheck));
    }

    public override bool IsAnimationValid()
    {
        return groundCheck.OnGround && move.enabled && Mathf.Abs(cAnim.Velocity.x) > 0f;
    }

    public override float GetAnimationSpeed()
    {
        return Mathf.Abs(cAnim.Velocity.x).Remap(0f, Mathf.Abs(move.DesiredVelocity.x), minAnimationSpeed, maxAnimationSpeed);
    }
}
