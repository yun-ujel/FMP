using UnityEngine;

[CreateAssetMenu(fileName = "Midair", menuName = "Scriptable Object/Animation Handler/Midair")]
public class MidairAnim : AnimationHandler
{
    [Header("Active When:")]
    [SerializeField] private float yVelocityIsBelow = float.PositiveInfinity;
    [SerializeField] private float yVelocityIsAbove = float.NegativeInfinity;

    [Space]

    [SerializeField] private float xVelocityIsBelow = float.PositiveInfinity;
    [SerializeField] private float xVelocityIsAbove = float.NegativeInfinity;

    private GroundCheck groundCheck;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        groundCheck = (GroundCheck)cAnim.GetCheck(typeof(GroundCheck));
    }

    public override bool IsAnimationValid()
    {
        return cAnim.Velocity.y < yVelocityIsBelow
            && cAnim.Velocity.y > yVelocityIsAbove
            && !groundCheck.OnGround
            && Mathf.Abs(cAnim.Velocity.x) < xVelocityIsBelow
            && Mathf.Abs(cAnim.Velocity.x) > xVelocityIsAbove
            && base.IsAnimationValid();
    }
}