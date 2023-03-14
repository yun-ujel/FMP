using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SlopeSlide : Capability
{
    [SerializeField] private SlopeCheck slopeCheck;
    private bool isSliding;

    private void Update()
    {
        if (slopeCheck.OnSlope && !isSliding)
        {
            Debug.Log("Begin Slide, Slope Direction: " + slopeCheck.SlopeFacing);
            BeginSlide();
        }
    }

    private void LateUpdate()
    {
        if (isSliding && !slopeCheck.OnSlope)
        {
            Debug.Log("End Slide, Slope Direction: " + slopeCheck.SlopeFacing);
            EndSlide();
        }
    }

    private void BeginSlide()
    {
        isSliding = true;
        DisableOtherCapabilities();
    }

    private void EndSlide()
    {
        isSliding = false;
        EnableOtherCapabilities();
    }
}
