using UnityEngine;

public class Press : MonoBehaviour
{
    [SerializeField] private Vector3 targetOffset;
    private Vector3 relativeVelocity;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        relativeVelocity = collision.relativeVelocity;
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, targetOffset);
        Vector3 half = targetOffset * 0.5f;
        Debug.DrawRay(transform.position + half, Vector2.Perpendicular(targetOffset));
    }
}
