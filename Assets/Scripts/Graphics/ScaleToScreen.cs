using UnityEngine;

[ExecuteInEditMode]
public class ScaleToScreen : MonoBehaviour
{
    // This script is intended for direct child objects of a Camera (e.g. Sprite Mask)

    [SerializeField, Range(0f, 1f)] private float screenScaleX = 1f;
    [SerializeField, Range(0f, 1f)] private float screenScaleY = 1f;
    private Vector2 screenScale = new Vector2(0.5f, 1f);

    [SerializeField] private Vector2 anchoredPosition = new Vector2(-1f, -1f);
    private Vector2 screenScaleOnLastCalculation;
    private Vector2 anchoredPosOnLastCalculation;

    private void Start()
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
        transform.ScaleToScreen(screenScale, anchoredPosition);
        screenScaleOnLastCalculation = screenScale;
        anchoredPosOnLastCalculation = anchoredPosition;
    }
}
