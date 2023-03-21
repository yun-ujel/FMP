using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract bool GetJumpHeld();

    public abstract bool GetJumpPressed();

    public abstract float GetHorizontalInput();

    public abstract float GetVerticalInput();

    public abstract bool GetAttackPressed();

    public abstract bool GetAttackHeld();
}
