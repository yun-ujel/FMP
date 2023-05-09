using UnityEngine;

public abstract class InputController : ScriptableObject
{
    [field: SerializeField] public bool Enabled { get; set; } = true;

    public abstract bool GetJumpHeld();

    public abstract bool GetJumpPressed();

    public abstract float GetHorizontalInput();

    public abstract float GetVerticalInput();

    public abstract bool GetAttackPressed();

    public abstract bool GetAttackHeld();

    public abstract Vector2 GetInputAxes();
}


//Buttons
//    Square  = joystick button 0
//    X       = joystick button 1
//    Circle  = joystick button 2
//    Triangle= joystick button 3
//    L1      = joystick button 4
//    R1      = joystick button 5
//    L2      = joystick button 6
//    R2      = joystick button 7
//    Share	= joystick button 8
//    Options = joystick button 9
//    L3      = joystick button 10
//    R3      = joystick button 11
//    PS      = joystick button 12
//    PadPress= joystick button 13

//Axes:
//    LeftStickX      = X-Axis
//    LeftStickY      = Y-Axis (Inverted?)
//    RightStickX     = 3rd Axis
//    RightStickY     = 6th Axis (Inverted?)
//    L2              = 4th Axis (-1.0f to 1.0f range, unpressed is -1.0f)
//    R2              = 5th Axis (-1.0f to 1.0f range, unpressed is -1.0f)
//    DPadX           = 7th Axis
//    DPadY           = 8th Axis (Inverted?)
