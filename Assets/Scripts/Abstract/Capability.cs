using System.Collections.Generic;
using UnityEngine;

public abstract class Capability : MonoBehaviour
{
    protected bool IsActive { get; set; }

    [SerializeField] protected InputController inputController = null;
    public virtual Capability[] GetOtherCapabilities()
    {
        List<Capability> capabilities = new List<Capability>();
        capabilities.AddRange(gameObject.GetComponents<Capability>());
        capabilities.Remove(this);

        return capabilities.ToArray();
    }

    public virtual void DisableOtherCapabilities()
    {
        //Debug.Log(this + " Has Disabled other Capabilities");
        var otherCapabilities = GetOtherCapabilities();
        for (int i = 0; i < otherCapabilities.Length; i++)
        {
            otherCapabilities[i].IsActive = false;
            otherCapabilities[i].DisableCapability();
        }
    }
    public virtual void EnableOtherCapabilities()
    {
        //Debug.Log(this + " Has Enabled other Capabilities");
        var otherCapabilities = GetOtherCapabilities();
        for (int i = 0; i < otherCapabilities.Length; i++)
        {
            otherCapabilities[i].enabled = true;
            otherCapabilities[i].EnableCapability();
        }
    }

    public virtual void DisableCapability()
    {
        enabled = false;
    }

    public virtual void EnableCapability()
    {
        IsActive = true;
    }
}
