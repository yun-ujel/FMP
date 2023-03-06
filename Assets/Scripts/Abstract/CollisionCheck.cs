using UnityEngine;

public abstract class CollisionCheck : MonoBehaviour
{
    public virtual bool Ground { get; protected set; }

    public virtual bool Slope { get; protected set; }

    public virtual bool Wall { get; protected set; }

    public virtual void Update()
    {

    }

    public virtual void SetFalse()
    {
        Ground = false;
        Slope = false;
        Wall = false;
    }
}
