using UnityEngine;

public class GrabAnim : AnimationHandler
{
    private GrabObject grab;

    private readonly float animationClipLength = 0.25f;
    private float animationScriptLength = 0.18f;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        grab = (GrabObject)cAnim.GetCapability(typeof(GrabObject));
    }

    public override bool IsAnimationValid()
    {
        return grab.InGrabAnimation;
    }
}
