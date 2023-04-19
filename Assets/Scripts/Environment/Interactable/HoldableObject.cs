using UnityEngine;

// --- SETUP ---
// Use "Holdable" Tag for all GameObjects with HoldableObject attached

// Create 2 GameObjects, one with this script, a Rigidbody2D and Collider2D
// Set the layer of this object to "Player" (7)

// The other GameObject should be a child of this, with a Collider2D set as a Trigger
// Set the layer of this object to anything other than "Player" (7)

// -------------

// This way, the object with the "Player" layer will collide with the ground, but not the player.
// The child will be a valid trigger for the player, meaning that they can detect it and grab it.

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class HoldableObject : MonoBehaviour
{
    public virtual bool IsBeingHeld { get; protected set; }

    [Header("Physics")]
    protected Rigidbody2D body;
    protected float startingGravityScale;

    [Header("Throw")]
    protected Vector3 throwForce;
    protected bool throwPending;

    [Header("Animation")]
    protected Vector3 targetPosition;
    protected Vector3 velocity;
    protected float smoothTime = 0.02f;

    public virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        startingGravityScale = body.gravityScale;
    }

    private void Update()
    {
        if (IsBeingHeld)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }

        if (throwPending) { Throw(); throwPending = false; }
    }

    public virtual void Grab(Vector3 holdPosition)
    {
        SetStateToHeld();

        targetPosition = holdPosition;
    }

    public virtual void QueueThrow(Vector2 throwForce)
    {
        throwPending = true;
        this.throwForce = throwForce;
    }

    public virtual void Throw()
    {
        transform.position = targetPosition;

        SetStateToThrown();

        body.velocity = throwForce;        
    }

    public virtual void Hold(Vector3 holdPosition)
    {
        targetPosition = holdPosition;
    }

    public virtual void Drop()
    {
        SetStateToThrown();
    }

    public virtual void SetStateToHeld()
    {
        IsBeingHeld = true;

        body.gravityScale = 0f;
        body.transform.rotation = Quaternion.identity;

        body.freezeRotation = true;
        if (TryGetComponent(out GravityMultiplier gravity)) { gravity.enabled = false; }
    }

    public virtual void SetStateToThrown()
    {
        IsBeingHeld = false;
        throwPending = false;

        body.freezeRotation = false;
        body.gravityScale = startingGravityScale;

        if (TryGetComponent(out GravityMultiplier gravity)) { gravity.enabled = true; }
    }
}
