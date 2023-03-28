using UnityEngine;

[CreateAssetMenu(fileName = "Midair", menuName = "Scriptable Object/Animation Handler/Midair")]
public class MidairAnim : AnimationHandler
{
    [Header("Active When:")]
    [SerializeField] private float yVelocityIsBelow = float.PositiveInfinity;
    [SerializeField] private float yVelocityIsAbove = float.NegativeInfinity;

    public override bool IsAnimationValid()
    {
        return cAnim.Velocity.y < yVelocityIsBelow && cAnim.Velocity.y > yVelocityIsAbove && !cAnim.AnyCollision;
    }
}
