using System.Collections.Generic;
using UnityEngine;

public abstract class Capability : MonoBehaviour
{
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
            otherCapabilities[i].enabled = false;
        }
    }
    public virtual void EnableOtherCapabilities()
    {
        var otherCapabilities = GetOtherCapabilities();
        for (int i = 0; i < otherCapabilities.Length; i++)
        {
            otherCapabilities[i].enabled = true;
        }
    }
}
