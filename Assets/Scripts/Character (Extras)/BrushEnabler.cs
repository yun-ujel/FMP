using UnityEngine;

public class BrushEnabler : MonoBehaviour
{
    [SerializeField] private DrawingSystemBrush brush;
    [SerializeField] private Rigidbody2D referenceBody;

    private void Update()
    {
        brush.enabled = referenceBody.velocity.sqrMagnitude > 0f;
    }
}
