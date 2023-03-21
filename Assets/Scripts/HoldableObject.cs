using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class HoldableObject : MonoBehaviour
{
    public bool isBeingThrown { get; private set; }

    private Rigidbody2D body;
    private float defaultGravityScale;
    private Vector3 targetPosition;

    private float grabAnimationLength;
    private float grabAnimationCounter;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        defaultGravityScale = body.gravityScale;
    }

    private void Update()
    {
        if (grabAnimationLength > 0f)
        {
            if (grabAnimationCounter < grabAnimationLength)
            {
                grabAnimationCounter += Time.deltaTime;
                
            }
            else
            {
                grabAnimationLength = 0f;
            }
        }
    }

    public void Grab(Vector3 holdPosition, float animationLength)
    {
        if (!isBeingThrown)
        {
            body.gravityScale = 0f;
            body.transform.rotation = Quaternion.identity;
            body.velocity = Vector2.zero;
            body.freezeRotation = true;

            grabAnimationLength = animationLength;

            Debug.Log("Grabbed Object: " + name);
        }
    }

    public void Throw(Vector2 throwForce)
    {
        body.freezeRotation = false;
        body.gravityScale = defaultGravityScale;
        body.velocity = throwForce;

        isBeingThrown = true;
    }

    public void Hold(Vector3 holdPosition)
    {
        targetPosition = holdPosition;
    }

    public void Drop()
    {
        body.freezeRotation = false;
        body.gravityScale = defaultGravityScale;
        body.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) // Layer 6 is Ground
        {
            isBeingThrown = false;
        }
    }
}
