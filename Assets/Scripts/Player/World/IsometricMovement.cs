using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class IsometricMovement : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    private Rigidbody2D body;


    [Header("Speed Values")]
    public float maxSpeed;
    [SerializeField] private float verticalSpeedMultiplier = 0.6f;
    private Vector2 direction;

    private Vector2 velocity;
    [Space]
    [SerializeField] private float acceleration;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        velocity = body.velocity;

        direction = inputController.GetInputAxes();
        direction.y *= verticalSpeedMultiplier;

        velocity = Vector2.MoveTowards(velocity, direction * maxSpeed, acceleration * Time.deltaTime);

        body.velocity = velocity;
    }
}
