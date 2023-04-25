
using UnityEngine;

[CreateAssetMenu(fileName = "Isometric", menuName = "Scriptable Object/Animation Handler/Isometric Animation Handler")]
public class IsometricAnimationHandler : AnimationHandler
{
    private IsometricMovement move;

    [Header("Requirements")]
    [SerializeField] private float xDirectionIsAbove = Mathf.NegativeInfinity;
    [SerializeField] private float xDirecttionIsBelow = Mathf.Infinity;

    [Space]

    [SerializeField] private float yDirectionIsAbove = Mathf.NegativeInfinity;
    [SerializeField] private float yDirectionIsBelow = Mathf.Infinity;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);
        isAnimationValidOverride = cAnim.TryGetCapability(out Capability cap, typeof(IsometricMovement));
        if (isAnimationValidOverride)
        {
            move = (IsometricMovement)cap;
        }
    }

    public override bool IsAnimationValid()
    {
        return base.IsAnimationValid()
            && move.Direction.y < yDirectionIsBelow
            && move.Direction.y > yDirectionIsAbove
            && Mathf.Abs(move.Direction.x) < xDirecttionIsBelow
            && Mathf.Abs(move.Direction.x) > xDirectionIsAbove;
    }
}
