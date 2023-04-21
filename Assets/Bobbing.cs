using UnityEngine;

public class Bobbing : MonoBehaviour
{
    [SerializeField] private float magnitude = 0.1f;
    [SerializeField] private float speed = 3f;
    private float counter;


    private float offset;
    private void Update()
    {
        AddToYPosition(-offset);

        counter += Time.deltaTime;
        offset = Mathf.Sin(counter * speed) * magnitude;

        AddToYPosition(offset);
    }

    private void AddToYPosition(float add)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + add, transform.position.z);
    }
}
