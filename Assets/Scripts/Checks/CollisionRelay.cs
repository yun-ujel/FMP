using UnityEngine;
public class CollisionRelay : MonoBehaviour
{
    private Collision2D collision;
    [SerializeField] private CollisionCheck[] collisionChecks;

    public bool AnyCollision
    {
        get
        {
            for (int i = 0; i < collisionChecks.Length; i++)
            {
                if (!collisionChecks[i].AnyCollision)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public CollisionCheck[] CollisionChecks => collisionChecks;

    private void Start()
    {
        for (int i = 0; i < collisionChecks.Length; i++)
        {
            collisionChecks[i].Initialize();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < collisionChecks.Length; i++)
        {
            collisionChecks[i].CollisionEnter(collision);
        }

        //Debug.Log("Enter Collision " + collision.collider.name);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        DrawRays(collision);

        for (int i = 0; i < collisionChecks.Length; i++)
        {
            collisionChecks[i].CollisionStay(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        for (int i = 0; i < collisionChecks.Length; i++)
        {
            collisionChecks[i].CollisionExit(collision);
        }

        //Debug.Log("Exit Collision " + collision.collider.name);
    }


    private void DrawRays(Collision2D collision)
    {
        Vector3 dir;
        for (int i = 0; i < collision.contactCount; i++)
        {
            dir = Vector3.ProjectOnPlane(Vector3.down, collision.GetContact(i).normal).normalized;

            Debug.DrawLine(transform.position, collision.GetContact(i).point, Color.magenta);
            Debug.DrawRay(collision.GetContact(i).point, collision.GetContact(i).normal, Color.yellow);
            Debug.DrawRay(collision.GetContact(i).point, dir, Color.green);
            Debug.DrawRay(collision.GetContact(i).point, -dir, Color.cyan);
        }
    }
}
