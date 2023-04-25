using UnityEngine;

[CreateAssetMenu(fileName = "Midair", menuName = "Scriptable Object/Animation Handler/Midair")]
public class MidairAnim : AnimationHandler
{
    [Header("Requirements")]
    [SerializeField] private float yVelocityIsBelow = float.PositiveInfinity;
    [SerializeField] private float yVelocityIsAbove = float.NegativeInfinity;
    private GroundCheck groundCheck;


    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        isAnimationValidOverride = cAnim.TryGetCheck(out CollisionCheck check, typeof(GroundCheck));

        if (isAnimationValidOverride)
        {
            groundCheck = (GroundCheck)check;
        }
    }

    public override bool IsAnimationValid()
    {
        return base.IsAnimationValid() 
            && cAnim.Velocity.y < yVelocityIsBelow 
            && cAnim.Velocity.y > yVelocityIsAbove 
            && !groundCheck.OnGround;
    }
}
