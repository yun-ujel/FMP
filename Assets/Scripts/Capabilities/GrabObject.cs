using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Move), typeof(Rigidbody2D))]
public class GrabObject : Capability
{
    private HoldableObject objectBeingHeld;
    private List<GameObject> objectsToHoldList = new List<GameObject>();

    private GameObject[] objectsToHold;
    private bool isHolding;

    private Move move;
    private Rigidbody2D body;

    [Header("Grabbing")]
    [SerializeField] private Vector3 holdOffset = new Vector3(0f, 1f, 0f);
    [SerializeField, Range(0f, 1f)] private float grabAnimationLength = 0.18f;
    private float grabAnimationCounter;

    [Space]

    [SerializeField, Range(0f, 50f)] private float dragWhileGrabbing = 24f;
    private float defaultDrag;
    [SerializeField] private bool disableJumpDuringGrab;
    private Jump jump;

    [Header("Throwing")]
    [SerializeField] private Vector2 minThrowForce = new Vector2(0f, 4f);
    [SerializeField] private Vector2 maxThrowForce = new Vector2(6f, 8f);

    [Space]

    [SerializeField] private Vector2 throwForceMultiplier = new Vector2(1.8f, 1.4f);

    private void Awake()
    {
        move = GetComponent<Move>();
        
        body = GetComponent<Rigidbody2D>();
        defaultDrag = body.drag;

        if (disableJumpDuringGrab)
        {
            _ = TryGetComponent(out jump);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Holdable"))
        {
            if (!objectsToHoldList.Contains(collision.gameObject))
            {
                objectsToHoldList.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Holdable"))
        {
            objectsToHoldList.Remove(collision.gameObject);
        }
    }

    private void Update()
    {
        if (inputController.GetAttackHeld() && !isHolding)
        {
            // GRABBING

            if (GetObjectToHold() != null)
            {
                objectBeingHeld = GetObjectToHold().GetComponentInParent<HoldableObject>();
                objectBeingHeld.Grab(transform.position + holdOffset, grabAnimationLength);

                body.drag = dragWhileGrabbing;
                grabAnimationCounter = grabAnimationLength;

                isHolding = true;

                SetJump(false);
            }
            else
            {
                objectBeingHeld = null;
            }
        }
        else if (inputController.GetAttackHeld() && isHolding)
        {
            // HOLDING

            objectBeingHeld.Hold(transform.position + holdOffset);
        }
        else if (isHolding)
        {
            // THROWING

            objectBeingHeld.Throw
            (
                new Vector2
                (
                    Mathf.Clamp(Mathf.Abs(body.velocity.x) + minThrowForce.x, minThrowForce.x, maxThrowForce.x) * move.Facing
                    * throwForceMultiplier.x,
                    Mathf.Clamp(body.velocity.y + minThrowForce.y, minThrowForce.y, maxThrowForce.y)
                    * throwForceMultiplier.y
                )
            );
            isHolding = false;

            objectsToHoldList.Remove(objectBeingHeld.gameObject);

            objectBeingHeld = null;
        }

        if (grabAnimationCounter <= 0f && grabAnimationCounter > -10f)
        {
            body.drag = defaultDrag;

            SetJump(true);

            grabAnimationCounter = -10f;
        }
        else
        {
            grabAnimationCounter -= Time.deltaTime;
        }
    }


    private GameObject testObjectToHold;
    private float lowestDistanceFromObject; // Used for SetObjectToHold calculation.
                                            // If there's more than one holdable object in range,
                                            // we iterate through each holdable object and find the closest one.
    private GameObject GetObjectToHold()
    {
        if (objectsToHoldList.Count > 1)
        {
            objectsToHold = objectsToHoldList.ToArray();

            for (int i = 0; i < objectsToHold.Length; i++)
            {
                if (lowestDistanceFromObject < Vector2.SqrMagnitude(objectsToHold[i].transform.position - transform.position))
                {
                    lowestDistanceFromObject = Vector2.SqrMagnitude(objectsToHold[i].transform.position - transform.position);
                    testObjectToHold = objectsToHold[i];
                }
            }
        }
        else if (objectsToHoldList.Count == 1)
        {
            testObjectToHold = objectsToHoldList[0];
        }
        else
        {
            testObjectToHold = null;
        }

        return testObjectToHold;
    }

    public override void DisableCapability()
    {
        if (isHolding)
        {
            objectBeingHeld.Drop();
            isHolding = false;
        }
        body.drag = defaultDrag;

        SetJump(true);

        base.DisableCapability();
    }

    private void SetJump(bool jumpSetting)
    {
        if (jump != null && disableJumpDuringGrab)
            jump.CanJump = jumpSetting;
    }
}
