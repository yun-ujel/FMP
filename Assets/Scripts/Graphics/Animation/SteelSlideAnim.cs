using UnityEngine;

[CreateAssetMenu(fileName = "Slide", menuName = "Scriptable Object/Animation Handler/Slope")]
public class SteelSlideAnim : AnimationHandler
{
    private SteelSlope steelSlope;
    private SlopeCheck slopeCheck;

    [Header("Requirements")]
    [SerializeField, Range(0, 1)] private float maxSlopeNormalY;
    
    [Space]

    [SerializeField, Range(0f, 10f)] private float xVelocityIsAbove = Mathf.NegativeInfinity;

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
            && slopeCheck.OnSlope && slopeCheck.SlopeNormal.y <= maxSlopeNormalY
            && Mathf.Abs(cAnim.Velocity.x) > xVelocityIsAbove
            && cAnim.Velocity.y < yVelocityIsBelow && cAnim.Velocity.y > yVelocityIsAbove
            && base.IsAnimationValid();
    }
}