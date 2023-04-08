using UnityEngine;

public class DrawingSystemBrush : MonoBehaviour
{
    [SerializeField] private DrawingSystem drawingSystem;
    [Header("Position")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 offsetEuler;

    [Header("Emission")]
    [SerializeField, Range(0f, 1f)] private float emissionChance;
    [SerializeField, Range(0, 10)] private int emissionRolls;
    private int successfulEmissionsThisFrame;

    [Header("Particle")]
    [SerializeField] private int minPixelRadius = 1;
    [SerializeField] private int maxPixelRadius = 1;
    [SerializeField] private int colourIndex;

    [Header("Shape")]
    [SerializeField] private float randomCircleRadius = 0f;

    private void OnDrawGizmos()
    {
        ExtensionMethods.DrawBox(GetPosition(), (1f/ 24f) + randomCircleRadius, Color.red);
    }

    private void Update()
    {
        successfulEmissionsThisFrame = 0;
        for (int i = 0; i < emissionRolls; i++)
        {
            if (Random.Range(0f, 1f) <= emissionChance)
            {
                successfulEmissionsThisFrame += 1;
                if (randomCircleRadius > 0f)
                {
                    Emit(GetPosition() + (new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y) * randomCircleRadius));
                }
                else if (successfulEmissionsThisFrame < 2)
                {
                    Emit(GetPosition());
                }
                else
                {
                    break;
                }
            }
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
