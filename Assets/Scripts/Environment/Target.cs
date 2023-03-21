using UnityEngine;

[RequireComponent(typeof(Collider2D))] // Requires a Collider2D set as a Trigger
public class Target : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Holdable"))
        {
            if (collision.gameObject.GetComponentInParent<HoldableObject>().IsBeingThrown)
            {
                Destroy(gameObject);
            }
        }
    }
}
