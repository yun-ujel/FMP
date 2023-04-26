using UnityEngine;

[ExecuteAlways]
public class ScaleToScreen : MonoBehaviour
{
    // This script is intended for direct child objects of a Camera (e.g. Sprite Mask)

    [SerializeField, Range(0f, 1f)] private float screenScaleX = 1f;
    [SerializeField] private bool mirrorXScale;

    [Space]

    [SerializeField, Range(0f, 1f)] private float screenScaleY = 1f;
    [SerializeField] private bool mirrorYScale;

    private Vector2 screenScale = new Vector2(0.5f, 1f);

    [Space]

    [SerializeField] private Vector2 anchoredPosition = new Vector2(-1f, -1f);
    private Vector2 screenScaleOnLastCalculation;
    private Vector2 anchoredPosOnLastCalculation;

    private void Awake()
    {
        Rescale();
    }

    private void Update()
    {
        screenScale = new Vector2(screenScaleX, screenScaleY);
    }

    private void LateUpdate()
    {
        if (screenScaleOnLastCalculation != screenScale || anchoredPosOnLastCalculation != anchoredPosition)
        {
            Rescale();
        }
    }

    private void Rescale()
    {
        CalculateScale(screenScale);
        screenScaleOnLastCalculation = screenScale;
        anchoredPosOnLastCalculation = anchoredPosition;
    }

    private void CalculateScale(Vector2 screenScale)
    {
        transform.LocalReset2D();

        // Scales an Object's size to match the Camera, multiplied by a proportion on the X or Y Axis.
        // Also anchors their position to a point relative to the Camera.

        Vector2 scale = Camera.main.GetWorldSize();
        transform.localScale = scale * screenScale;        

        Vector3 worldPosition = ((scale * GetAnchors()) + (transform.localScale * GetMirrors())) * 0.5f;
        worldPosition.z = transform.localPosition.z;

        transform.localPosition = worldPosition;
    }

    private Vector2 GetMirrors()
    {
        return new Vector2(mirrorXScale ? -1f : 1f, mirrorYScale ? -1f : 1f);
    }

    private Vector2 GetAnchors()
    {
        return anchoredPosition * -GetMirrors();
    }
}
