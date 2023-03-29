using UnityEngine;

[CreateAssetMenu(fileName = "Midair", menuName = "Scriptable Object/Animation Handler/Midair")]
public class MidairAnim : AnimationHandler
{
    [Header("Requirements")]
    [SerializeField] private float yVelocityIsBelow = float.PositiveInfinity;
    [SerializeField] private float yVelocityIsAbove = float.NegativeInfinity;
    private GroundCheck groundCheck;

    [Space]

    [SerializeField] private bool isHoldingObject = false;
    private GrabObject grab;




    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

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
            return cAnim.Velocity.y < yVelocityIsBelow && cAnim.Velocity.y > yVelocityIsAbove && !groundCheck.OnGround;
        }
        // Else
        return cAnim.Velocity.y < yVelocityIsBelow && cAnim.Velocity.y > yVelocityIsAbove && !groundCheck.OnGround && grab.IsHolding;
    }
}
