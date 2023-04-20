using UnityEngine;

[CreateAssetMenu(fileName = "PlayerController", menuName = "Scriptable Object/Input Controller/Player Controller")]
public class PlayerController : InputController
{
    public override bool GetJumpPressed()
    {
        return Input.GetButtonDown("Jump");
    }

    public override bool GetJumpHeld()
    {
        return Input.GetButton("Jump");
    }
    public override float GetHorizontalInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override bool GetAttackPressed()
    {
        return Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J);
    }

    public override float GetVerticalInput()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public override bool GetAttackHeld()
    {
        return Input.GetButton("Fire1") || Input.GetKey(KeyCode.J);
    }

    public override Vector2 GetInputAxes()
    {
        Vector2 squared = new Vector2(GetHorizontalInput(), GetVerticalInput());
        // Assuming GetHorizontalInput() and GetVerticalInput() only return values between 1f and -1f,
        // The Squared Magnitude of them can only be either 0, 1 or 2.
        // Therefore, if the squared magnitude of them is above 1,
        // we set it to what the normalized version would be. (Roughly Speaking)

        return squared.sqrMagnitude <= 1f ? squared : squared * 0.7f;
    }
}
