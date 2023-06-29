using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInput
{
    [CreateAssetMenu(fileName = "ArcadeController", menuName = "Scriptable Object/Input Controller/Arcade Controller")]
    public class ArcadeController : InputController
    {
        public override bool GetJumpPressed()
        {
            return (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.B)) && Enabled;
        }
        public override bool GetJumpHeld()
        {
            return (Input.GetKey(KeyCode.M) || Input.GetKey(KeyCode.B)) && Enabled;
        }
        public override float GetHorizontalInput()
        {
            return Enabled ? Input.GetAxisRaw("Horizontal") : 0f;
        }

        public override bool GetInteractPressed()
        {
            return (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Z)) && Enabled;
        }

        public override float GetVerticalInput()
        {
            return Enabled ? Input.GetAxisRaw("Vertical") : 0f;
        }

        public override bool GetInteractHeld()
        {
            return (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Z)) && Enabled;
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
            return (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.C)) && Enabled;
        }

        public override bool GetBackHeld()
        {
            return (Input.GetKey(KeyCode.H) || Input.GetKey(KeyCode.C)) && Enabled;
        }
    }
}