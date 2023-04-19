using UnityEngine;

[CreateAssetMenu(fileName = "Grab", menuName = "Scriptable Object/Animation Handler/Grab")]
public class GrabAnim : AnimationHandler
{
    private GrabObject grab;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        grab = (GrabObject)cAnim.GetCapability(typeof(GrabObject));
    }

    public override bool IsAnimationValid()
    {
        return grab.IsHolding;
    }
}
