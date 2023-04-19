using System.Collections.Generic;
using UnityEngine;

public abstract class Capability : MonoBehaviour
{
    protected bool IsActive { get; set; }

    [SerializeField] protected InputController inputController = null;
    public virtual Capability[] GetCapabilitiesWithException(Capability[] exceptions)
    {
        List<Capability> capabilities = new List<Capability>();
        capabilities.AddRange(gameObject.GetComponents<Capability>());
        for (int i = 0; i < exceptions.Length; i++)
        {
            capabilities.Remove(exceptions[i]);
        }

        return capabilities.ToArray();
    }

    public virtual void DisableOtherCapabilities()
    {
        //Debug.Log(this + " Has Disabled other Capabilities");
        Capability[] otherCapabilities = GetCapabilitiesWithException(new Capability[] { this });
        for (int i = 0; i < otherCapabilities.Length; i++)
        {
            otherCapabilities[i].IsActive = false;
            otherCapabilities[i].DisableCapability();
        }
    }
    public virtual void EnableOtherCapabilities()
    {
        //Debug.Log(this + " Has Enabled other Capabilities");
        Capability[] otherCapabilities = GetCapabilitiesWithException(new Capability[] { this });
        for (int i = 0; i < otherCapabilities.Length; i++)
        {
            otherCapabilities[i].enabled = true;
            otherCapabilities[i].EnableCapability();
        }
    }

    public virtual void DisableOtherCapabilitiesExcept(Capability[] exceptions)
    {
        List<Capability> listExceptions = new List<Capability>() { this };
        listExceptions.AddRange(exceptions);
        exceptions = listExceptions.ToArray();

        Capability[] capabilities = GetCapabilitiesWithException(exceptions);

        for (int i = 0; i < capabilities.Length; i++)
        {
            capabilities[i].IsActive = false;
            capabilities[i].DisableCapability();
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

    public virtual void TriggerMainEffect() { }
}
