using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public virtual Capability[] Capabilities { get; protected set; }
    public virtual float Facing { get; protected set; } = 1f;

    public virtual void Awake()
    {
        Capabilities = gameObject.GetComponents<Capability>();
    }
}
