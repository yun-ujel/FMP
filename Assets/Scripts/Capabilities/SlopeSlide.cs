using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CollisionCheck))]
public class SlopeSlide : Capability
{
    private CollisionCheck collidingWith;

    private void Awake()
    {
        collidingWith = GetComponent<CollisionCheck>();
    }

    private void Update()
    {
        if (collidingWith.Slope)
        {
            DisableOtherCapabilities();
        }
        else
        {
            EnableOtherCapabilities();
        }
    }


}
