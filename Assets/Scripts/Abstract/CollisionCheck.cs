using UnityEngine;

public abstract class CollisionCheck : ScriptableObject
{
    public virtual bool AnyCollision { get; protected set; }

    public virtual void CollisionEnter(Collision2D collision)
    {
        AnyCollision = true;
    }
    public virtual void CollisionStay(Collision2D collision)
    {
        AnyCollision = true;
    }
    public virtual void CollisionExit(Collision2D collision)
    {
        AnyCollision = false;
    }

    public abstract void EvaluateCollision(Collision2D collision);
    public abstract void Initialize();
}
