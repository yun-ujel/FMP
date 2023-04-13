using UnityEngine;

[CreateAssetMenu(fileName = "Slide", menuName = "Scriptable Object/Animation Handler/Slide")]
public class SteelSlideAnim : AnimationHandler
{
    private SteelSlope steelSlope;
    private SlopeCheck slopeCheck;

    [Header("Requirements")]
    [SerializeField, Range(0, 180)] private float maxSlopeAngle;
    [SerializeField] private LastSlope lastSlopeRequirement;

    public enum LastSlope
    {
        steeper,
        shallower,
        none
    }

    [Space]

    [SerializeField] private float yVelocityIsBelow = float.PositiveInfinity;
    [SerializeField] private float yVelocityIsAbove = float.NegativeInfinity;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        steelSlope = (SteelSlope)cAnim.GetCapability(typeof(SteelSlope));
        slopeCheck = (SlopeCheck)cAnim.GetCheck(typeof(SlopeCheck));
    }

    public override bool IsAnimationValid()
    {
        return steelSlope.IsSliding
            && slopeCheck.AnyCollision
            && GetSlopeAngle() <= maxSlopeAngle
            && cAnim.Velocity.y < yVelocityIsBelow && cAnim.Velocity.y > yVelocityIsAbove
            && base.IsAnimationValid();
    }

    private bool IsSlopeChangeValid()
    {
        // Uses SteelSlope.LastMoveAngle and SteelSlope.CurrentMoveAngle to check whether the last slope the player was on was steeper
        // If it is, it will return true depending on what preference the AnimationHandler has set
        // For example, if lastSlopeWasSteeper was unchecked, it will return the inverse, essentially checking whether the last slope was shallower instead.

        // If the player was last in midair, the last slope would be considered steeper. This is set in SteelSlope rather than this script.

        bool slopeWasSteeper = steelSlope.LastMoveAngle < steelSlope.CurrentMoveAngle;

        return (lastSlopeRequirement == LastSlope.steeper && slopeWasSteeper)
            || (lastSlopeRequirement == LastSlope.shallower && !slopeWasSteeper)
            || (lastSlopeRequirement == LastSlope.none);
    }

    private float GetSlopeAngle()
    {
        return steelSlope.CurrentMoveAngle;
    }
}