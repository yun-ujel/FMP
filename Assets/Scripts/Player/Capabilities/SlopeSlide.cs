using UnityEngine;

namespace Platforming.Capabilities
{
    public class SlopeSlide : Capability
    {
        [SerializeField] private SlopeCheck slopeCheck;
        private bool isSliding;
        private Move move;

        [Header("References")]
        [SerializeField] private Capability[] abilitiesDuringSlide;

        private void Awake()
        {
            _ = TryGetComponent(out move);
        }

        private void Update()
        {
            if (slopeCheck.OnSlope && !isSliding)
            {
                //Debug.Log("Begin Slide, Slope Direction: " + slopeCheck.SlopeFacing);
                InitiateSlide();
            }
        }

        private void LateUpdate()
        {
            if (isSliding && !slopeCheck.OnSlope)
            {
                //Debug.Log("End Slide, Slope Direction: " + slopeCheck.SlopeFacing);
                FinishSlide();
            }
        }

        private void InitiateSlide()
        {
            isSliding = true;
            if (move != null)
            {
                move.Facing = slopeCheck.SlopeFacing;
            }

            DisableOtherCapabilitiesExcept(abilitiesDuringSlide);
        }

        private void FinishSlide()
        {
            isSliding = false;
            EnableOtherCapabilities();
        }
    }

}