using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract bool GetJumpHeld();

    public abstract bool GetJumpPressed();

    public abstract float GetHorizontalInput();

    public abstract bool GetAttackPressed();
}
