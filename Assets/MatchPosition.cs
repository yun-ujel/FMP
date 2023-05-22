using UnityEngine;

public class MatchPosition : MonoBehaviour
{
    [SerializeField] private Transform follow;

    [Space]

    [SerializeField, Range(0f, 1f)] private float smoothTime;
    private Vector3 velocity;

    void Update()
    {
        transform.position =
            Vector3.SmoothDamp(transform.position, follow.position, ref velocity, smoothTime);
    }
}
