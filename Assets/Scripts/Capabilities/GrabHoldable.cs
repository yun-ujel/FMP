using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Move), typeof(Rigidbody2D))]
public class GrabHoldable : Capability
{
    private HoldableObject objectBeingHeld;
    private List<GameObject> objectsToHoldList = new List<GameObject>();

    private GameObject[] objectsToHold;
    private bool isHolding;

    private Move move;
    private Rigidbody2D body;
    [Header("Grabbing")]
    [SerializeField] private Vector3 holdOffset;
    [SerializeField] private float grabAnimationLength;

    [Header("Throwing")]
    [SerializeField] private Vector2 minThrowForce;
    [SerializeField] private Vector2 maxThrowForce;

    [SerializeField] private Vector2 throwForceMultiplier = new Vector2(1f, 1f);

    private void Awake()
    {
        move = GetComponent<Move>();
        body = GetComponent<Rigidbody2D>();
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
            if (GetObjectToHold() != null)
            {
                objectBeingHeld = GetObjectToHold().GetComponentInParent<HoldableObject>();
                objectBeingHeld.Grab(transform.position + holdOffset, grabAnimationLength);
            }
        }
        else if (inputController.GetAttackHeld() && isHolding)
        {
            objectBeingHeld.Hold(transform.position + holdOffset);
        }
        else if (isHolding)
        {
            // Throw
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
            objectBeingHeld = null;
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
                    isHolding = true;
                }
            }
        }
        else if (objectsToHoldList.Count == 1)
        {
            testObjectToHold = objectsToHoldList[0];
            isHolding = true;
        }
        else
        {
            testObjectToHold = null;
            isHolding = false;
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

        base.DisableCapability();
    }
}
