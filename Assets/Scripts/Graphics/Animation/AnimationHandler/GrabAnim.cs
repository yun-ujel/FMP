using UnityEngine;

[CreateAssetMenu(fileName = "Grab", menuName = "Scriptable Object/Animation Handler/Grab")]
public class GrabAnim : AnimationHandler
{
    private GrabObject grab;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        isAnimationValidOverride = cAnim.TryGetCapability(out Capability cap, typeof(GrabObject));

        if (isAnimationValidOverride)
        {
            grab = (GrabObject)cap;
        }
    }

    public override bool IsAnimationValid()
    {
        return base.IsAnimationValid() && grab.IsHolding;
    }
}
