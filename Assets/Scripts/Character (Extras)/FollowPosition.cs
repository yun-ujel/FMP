using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    [SerializeField] private Transform refTransform;
    private Vector3 positionOnLastUpdate;
    [Space]
    [SerializeField] private float distanceFromTarget;
    [Space]
    [SerializeField] private float smoothTime;
    [SerializeField] private float stationarySmoothTime;
    [Space]
    [SerializeField] private bool rotateToFaceTarget;

    private float currentSmoothTime;

    private Vector3 velocity;
    private Vector3 facing;

    private void FixedUpdate()
    {
        if (refTransform.position != positionOnLastUpdate)
        {
            facing = refTransform.position - transform.position;
            currentSmoothTime = smoothTime;
        }
        else
        {
            facing = Vector3.up;
            currentSmoothTime = stationarySmoothTime;
        }
        transform.position = Vector3.SmoothDamp(transform.position, refTransform.position + (facing * -distanceFromTarget), ref velocity, currentSmoothTime, Mathf.Infinity, Time.fixedDeltaTime);
        positionOnLastUpdate = refTransform.position;

        if (rotateToFaceTarget) { transform.right = facing; }
    }
}
