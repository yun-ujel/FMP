using UnityEngine;

[CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
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
        return Input.GetButtonDown("Fire1");
    }
}
