using UnityEngine;

[CreateAssetMenu(fileName = "GroundWalk", menuName = "Scriptable Object/Animation Handler/Ground Walk")]
public class GroundWalkAnim : AnimationHandler
{
    private Move move;
    private GroundCheck groundCheck;
    [Header("Speed Values")]
    [SerializeField, Range(0f, 10f)] private float minAnimationSpeed = 0.8f;
    [SerializeField, Range(0f, 10f)] private float maxAnimationSpeed = 1.6f;

    [Header("Requirements")]
    [SerializeField, Range(0f, 10f)] private float xVelocityIsAbove = 0f;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        if (cAnim.TryGetCheck(out CollisionCheck check, typeof(GroundCheck)) && cAnim.TryGetCapability(out Capability cap, typeof(Move)))
        {
            isAnimationValidOverride = true;

            move = (Move)cap;
            groundCheck = (GroundCheck)check;
        }
        else
        {
            isAnimationValidOverride = false;
        }
    }

    public override bool IsAnimationValid()
    {
        return base.IsAnimationValid()
            && groundCheck.OnGround
            && move.enabled
            && Mathf.Abs(cAnim.Velocity.x) > 0f
            && Mathf.Abs(cAnim.Velocity.x) > xVelocityIsAbove;
    }

    public override float GetAnimationSpeed()
    {
        if (isAnimationValidOverride)
        {
            return Mathf.Abs(cAnim.Velocity.x).Remap(0f, Mathf.Abs(move.DesiredVelocity.x), minAnimationSpeed, maxAnimationSpeed) 
                * base.GetAnimationSpeed();
        }
        else
        {
            return base.GetAnimationSpeed();
        }
    }
}
