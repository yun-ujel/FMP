using UnityEngine;

public class PlayerHairPosition : MonoBehaviour
{
    [SerializeField] private Vector3 offsetInPixels;
    public Vector3 Offset => offsetInPixels / 24f;

    [Header("Debug Display")]
    [SerializeField] private float boxSize = 0.416666666667f;
    [SerializeField] private Color color = Color.green;

    private void OnDrawGizmosSelected()
    {
        ExtensionMethods.DrawBox(GetPosition(), boxSize / 2, color);
    }

    public Vector3 GetPosition()
    {
        return transform.position + (offsetInPixels / 24f);
    }
}
