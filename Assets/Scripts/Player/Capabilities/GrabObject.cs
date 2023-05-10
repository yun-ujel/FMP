using System.Collections.Generic;
using UnityEngine;

public class GrabObject : Capability
{
    private Rigidbody2D body;
    private HoldableObject objectBeingHeld;

    [Header("Grabbing")]
    private List<GameObject> objectsToHoldList = new List<GameObject>();
    private GameObject[] objectsToHold;
    private bool isGrabbing;

    [Header("Holding")]
    private Vector3 lastHoldOffset = new Vector3(0f, 1f, 0f);
    public bool IsHolding { get; private set; }

    [Header("Throwing")]
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwDelay = 0.2f;
    private float throwDelayCounter;
    private bool isThrowing;

    [Space]

    [SerializeField] private float throwRecoil = 10f;

    private void Awake()
    {
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
        if (!inputController.GetInteractHeld()) { isGrabbing = false; }

        if (inputController.GetInteractPressed() && !IsHolding)
        {
            GetObjectToHold()?.transform.parent.TryGetComponent(out objectBeingHeld);
            if (objectBeingHeld != null)
            {
                objectBeingHeld.Grab(transform.position + GetHoldOffset());

                isGrabbing = true;
                IsHolding = true;
            }
        }

        if (IsHolding)
        {
            objectBeingHeld.Hold(transform.position + GetHoldOffset());

            if (!isGrabbing)
            {
                if (inputController.GetInteractPressed())
                {
                    throwDelayCounter = throwDelay;
                    isThrowing = true;
                }
                else
                {
                    throwDelayCounter -= Time.deltaTime;
                }

                if (throwDelayCounter > 0f || inputController.GetInteractHeld())
                {
                    body.constraints = RigidbodyConstraints2D.FreezeAll;
                }
                else if (isThrowing)
                {
                    body.constraints = RigidbodyConstraints2D.FreezeRotation;
                    body.velocity = GetHoldOffset() * -throwRecoil;

                    objectBeingHeld.QueueThrow(GetHoldOffset() * throwForce);
                    IsHolding = false;

                    isThrowing = false;

                    objectsToHoldList.Remove(objectBeingHeld.gameObject);
                    objectBeingHeld = null;
                }
            }
        }
    }

    private Vector3 GetHoldOffset()
    {
        Vector3 holdOffset = inputController.GetInputAxes();

        if (holdOffset.sqrMagnitude > 0f)
        {
            lastHoldOffset = holdOffset;
        }

        return lastHoldOffset;
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

}
