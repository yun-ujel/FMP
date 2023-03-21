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
}
