using UnityEngine;

public class DrawingSystemBrush : MonoBehaviour
{
    [SerializeField] private DrawingSystem drawingSystem;
    [Header("Position")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 offsetEuler;

    [Header("Emission")]
    [SerializeField, Range(0f, 1f)] private float emissionChance;
    [SerializeField, Range(0, 200f)] private float emissionsPerSecond;
    private float timeSinceLastEmission;

    private int emissionsThisFrame = 0;

    [Header("Particle")]
    [SerializeField] private int minPixelRadius = 1;
    [SerializeField] private int maxPixelRadius = 1;
    [SerializeField] private int colourIndex;

    [Header("Shape")]
    [SerializeField] private float randomCircleRadius = 0f;

    private void OnDrawGizmosSelected()
    {
        ExtensionMethods.DrawBox(GetPosition(), Mathf.Max(1f / 24f, randomCircleRadius), Color.red);
    }

    private void Start()
    {
        if (drawingSystem == null) { drawingSystem = DrawingSystem.Instance; }
    }

    private void Update()
    {
        timeSinceLastEmission += Time.deltaTime;
        emissionsThisFrame = Mathf.FloorToInt(timeSinceLastEmission * emissionsPerSecond);

        if (emissionsThisFrame > 0)
        {
            for (int i = 0; i < emissionsThisFrame; i++)
            {
                if (Random.Range(0f, 1f) < emissionChance)
                {
                    if (randomCircleRadius > (1f/ 24f))
                    {
                        Emit(GetPosition() + (new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y) * randomCircleRadius));
                    }
                    else
                    {
                        Emit(GetPosition());
                        break;
                    }
                }
            }
            emissionsThisFrame = 0;
            timeSinceLastEmission = 0;
        }
    }

    private void Emit(Vector3 position)
    {
        int pixelRadius = Random.Range(minPixelRadius, maxPixelRadius);
        if (pixelRadius <= 1)
        {
            drawingSystem.ApplyColourToPixel(colourIndex, position);
        }
        else
        {
            drawingSystem.ApplyColourToCircle(colourIndex, position, pixelRadius);
        }
    }

    private Vector3 GetPosition()
    {
        return transform.position + (Quaternion.Euler(offsetEuler) * offset);
    }
}
