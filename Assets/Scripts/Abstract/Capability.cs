using System.Collections.Generic;
using UnityEngine;

public abstract class Capability : MonoBehaviour
{
    protected bool IsActive { get; set; }
    public virtual Capability[] GetOtherCapabilities()
    {
        List<Capability> capabilities = new List<Capability>();
        capabilities.AddRange(gameObject.GetComponents<Capability>());
        capabilities.Remove(this);

        return capabilities.ToArray();
    }

    public virtual void DisableOtherCapabilities()
    {
        var otherCapabilities = GetOtherCapabilities();
        for (int i = 0; i < otherCapabilities.Length; i++)
        {
            otherCapabilities[i].IsActive = false;
            otherCapabilities[i].Disable();
        }
    }
    public virtual void EnableOtherCapabilities()
    {
        var otherCapabilities = GetOtherCapabilities();
        for (int i = 0; i < otherCapabilities.Length; i++)
        {
            otherCapabilities[i].enabled = true;
            otherCapabilities[i].IsActive = true;
        }
    }

    public virtual void Disable()
    {
        enabled = false;
    }
}
