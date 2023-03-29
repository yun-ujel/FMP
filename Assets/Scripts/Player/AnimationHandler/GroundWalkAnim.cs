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
    
    [Space]
    
    [SerializeField] private bool isHoldingObject = false;
    private GrabObject grab;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        move = (Move)cAnim.GetCapability(typeof(Move));
        groundCheck = (GroundCheck)cAnim.GetCheck(typeof(GroundCheck));

        if (isHoldingObject)
        {
            grab = (GrabObject)cAnim.GetCapability(typeof(GrabObject));
        }
    }

    public override bool IsAnimationValid()
    {
        if (!isHoldingObject)
        {
            return groundCheck.OnGround && move.enabled && Mathf.Abs(cAnim.Velocity.x) > 0f
                && Mathf.Abs(cAnim.Velocity.x) > xVelocityIsAbove;
        }
        // Else
        return groundCheck.OnGround && move.enabled && Mathf.Abs(cAnim.Velocity.x) > 0f
            && Mathf.Abs(cAnim.Velocity.x) > xVelocityIsAbove && grab.IsHolding;
    }

    public override float GetAnimationSpeed()
    {
        return Mathf.Abs(cAnim.Velocity.x).Remap(0f, Mathf.Abs(move.DesiredVelocity.x), minAnimationSpeed, maxAnimationSpeed);
    }
}
