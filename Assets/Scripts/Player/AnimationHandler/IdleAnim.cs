using UnityEngine;

[CreateAssetMenu(fileName = "Idle", menuName = "Scriptable Object/Animation Handler/Idle")]
public class IdleAnim : AnimationHandler
{
    public override bool IsAnimationValid()
    {
        return true;
    }
}
