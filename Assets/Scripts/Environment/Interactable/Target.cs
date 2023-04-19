using UnityEngine;

[RequireComponent(typeof(Collider2D))] // Requires a Collider2D set as a Trigger
public class Target : MonoBehaviour
{    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Holdable"))
        {
            if (collision.gameObject.GetComponentInParent<DefaultThrowObject>().IsBeingHeld)
            {
                Destroy(gameObject);
            }
        }
    }
}
