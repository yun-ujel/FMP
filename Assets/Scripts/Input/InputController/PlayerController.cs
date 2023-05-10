using UnityEngine;

namespace PlayerInput
{
    [CreateAssetMenu(fileName = "PlayerController", menuName = "Scriptable Object/Input Controller/Player Controller")]
    public class PlayerController : InputController
    {
        public override bool GetJumpPressed()
        {
            return Input.GetButtonDown("Jump") && Enabled;
        }
        public override bool GetJumpHeld()
        {
            return Input.GetButton("Jump") && Enabled;
        }
        public override float GetHorizontalInput()
        {
            return Enabled ? Input.GetAxisRaw("Horizontal") : 0f;
        }

        public override bool GetInteractPressed()
        {
            return (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J)) && Enabled;
        }

        public override float GetVerticalInput()
        {
            return Enabled ? Input.GetAxisRaw("Vertical") : 0f;
        }

        public override bool GetInteractHeld()
        {
            return (Input.GetButton("Fire1") || Input.GetKey(KeyCode.J)) && Enabled;
        }

        public override Vector2 GetInputAxes()
        {
            if (Enabled)
            {
                Vector2 squared = new Vector2(GetHorizontalInput(), GetVerticalInput());
                return squared.normalized;
            }
            return Vector2.zero;
        }

        public override bool GetBackPressed()
        {
            return Input.GetButtonDown("Fire2") && Enabled;
        }

        public override bool GetBackHeld()
        {
            return Input.GetButton("Fire2") && Enabled;
        }
    }

}