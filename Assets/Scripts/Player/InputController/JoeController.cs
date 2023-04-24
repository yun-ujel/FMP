using UnityEngine;

[System.Serializable]
public class Joe
{
    public bool isAxis;
    public string name;
}

[CreateAssetMenu(fileName = "JoeController", menuName = "Scriptable Object/Input Controller/Joe Controller")]
public class JoeController : InputController
{
    // Requires InputManager-JoeVersion

    [Header("Axes")]
    [SerializeField] private Joe[] xAxisBindings;
    [SerializeField] private Joe[] yAxisBindings;

    [Header("Jump")]
    [SerializeField] private Joe[] jumpBindings;

    [Header("Attack")]
    [SerializeField] private Joe[] attackBindings;


    public override bool GetAttackHeld()
    {
        for (int i = 0; i < attackBindings.Length; i++)
        {
            if (attackBindings[i].isAxis)
            {
                if (Input.GetAxisRaw(attackBindings[i].name) >= 1f)
                {
                    return true;
                }
            }
            else
            {
                if (Input.GetButton(attackBindings[i].name))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override bool GetAttackPressed()
    {
        for (int i = 0; i < attackBindings.Length; i++)
        {
            if (attackBindings[i].isAxis)
            {
                if (Input.GetAxisRaw(attackBindings[i].name) >= 1f)
                {
                    return true;
                }
            }
            else
            {
                if (Input.GetButtonDown(attackBindings[i].name))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override float GetHorizontalInput()
    {
        throw new System.NotImplementedException();
    }

    public override bool GetJumpHeld()
    {
        for (int i = 0; i < jumpBindings.Length; i++)
        {
            if (jumpBindings[i].isAxis)
            {
                if (Input.GetAxisRaw(jumpBindings[i].name) >= 1f)
                {
                    return true;
                }
            }
            else
            {
                if (Input.GetButton(jumpBindings[i].name))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override bool GetJumpPressed()
    {
        for (int i = 0; i < jumpBindings.Length; i++)
        {
            if (jumpBindings[i].isAxis)
            {
                if (Input.GetAxisRaw(jumpBindings[i].name) >= 1f)
                {
                    return true;
                }
            }
            else
            {
                if (Input.GetButtonDown(jumpBindings[i].name))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override float GetVerticalInput()
    {
        throw new System.NotImplementedException();
    }

    public override Vector2 GetInputAxes()
    {
        Vector2 squared = new Vector2(GetHorizontalInput(), GetVerticalInput());
        return squared.normalized;
    }
}
