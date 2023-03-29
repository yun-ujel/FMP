using UnityEngine;

[CreateAssetMenu(fileName = "Idle", menuName = "Scriptable Object/Animation Handler/Idle")]
public class IdleAnim : AnimationHandler
{
    [Header("Requirements")]
    [SerializeField] private bool isHoldingObject = false;
    private GrabObject grab;

    public override void SetCharacterAnimator(CharacterAnimation characterAnimation)
    {
        base.SetCharacterAnimator(characterAnimation);

        if (isHoldingObject)
        {
            grab = (GrabObject)cAnim.GetCapability(typeof(GrabObject));
        }
    }

    public override bool IsAnimationValid()
    {
        if (!isHoldingObject)
        {
            return true;
        }
        // Else
        return grab.IsHolding;
    }
}
