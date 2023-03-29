using UnityEngine;

[CreateAssetMenu(fileName = "Grab", menuName = "Scriptable Object/Animation Handler/Grab")]
public class GrabAnim : AnimationHandler
{
    private GrabObject grab;

    [SerializeField] private float animationClipLength = 0.25f;
    private float animationScriptLength = 0.18f;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        grab = (GrabObject)cAnim.GetCapability(typeof(GrabObject));
        animationScriptLength = grab.GrabAnimationLength;
    }

    public override bool IsAnimationValid()
    {
        return grab.InGrabAnimation;
    }

    public override float GetAnimationSpeed()
    {
        return animationClipLength / animationScriptLength;
    }
}
