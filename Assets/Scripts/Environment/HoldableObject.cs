using UnityEngine;

// --- SETUP ---
// Use "Holdable" Tag for all GameObjects with HoldableObject attached

// Create 2 GameObjects, one with this script, a Rigidbody2D and Collider2D
// Set the layer of this object to "Player" (7)

// The other GameObject should be a child of this, with a Collider2D set as a Trigger
// Set the layer of this object to anything other than "Player" (7)

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class HoldableObject : MonoBehaviour
{
    public bool IsBeingThrown { get; private set; }

    [Header("Physics")]
    private Rigidbody2D body;
    private float defaultGravityScale;

    [Header("Grab Animation")]
    private Vector3 targetPosition;
    private Vector3 initialGrabPosition;
    private Vector3 distanceToTarget;

    private float grabAnimationLength;
    private float grabAnimationCounter;

    [Header("Throw")]
    private Vector3 throwForce;
    private bool throwPending;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        defaultGravityScale = body.gravityScale;
    }

    private void Update()
    {
        if (grabAnimationLength > 0f)
        {
            distanceToTarget = targetPosition - initialGrabPosition;
            if (grabAnimationCounter < grabAnimationLength)
            {
                grabAnimationCounter += Time.deltaTime;

                transform.position = initialGrabPosition + (distanceToTarget * (grabAnimationCounter / grabAnimationLength));
            }
            else
            {
                distanceToTarget = Vector3.zero;
                transform.position = targetPosition;
                grabAnimationLength = 0f;
            }
        }
        else if (throwPending)
        {
            body.freezeRotation = false;
            body.gravityScale = defaultGravityScale;
            body.velocity = throwForce;

            IsBeingThrown = true;
            throwPending = false;
        }
    }

    public void Grab(Vector3 holdPosition, float animationLength)
    {
        // Set Physics
        body.gravityScale = 0f;
        body.velocity = Vector2.zero;

        // Set Rotation
        body.transform.rotation = Quaternion.identity;
        body.freezeRotation = true;

        // Set Grab Animation Parameters
        grabAnimationLength = animationLength; // Time
        grabAnimationCounter = 0f;

        initialGrabPosition = transform.position; // Position
        targetPosition = holdPosition;

        Debug.Log("Grabbed Object: " + name);
    }

    public void Throw(Vector2 throwForce)
    {
        throwPending = true;
        this.throwForce = throwForce;
    }

    public void Hold(Vector3 holdPosition)
    {
        targetPosition = holdPosition;
        if (grabAnimationLength <= 0f)
        {
            transform.position = targetPosition;
        }
    }

    public void Drop()
    {
        grabAnimationLength = 0f;

        throwPending = false;

        body.freezeRotation = false;
        body.gravityScale = defaultGravityScale;
        body.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) // Layer 6 is Ground
        {
            IsBeingThrown = false;
        }
    }
}
