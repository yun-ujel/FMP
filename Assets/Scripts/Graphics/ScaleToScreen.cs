using UnityEngine;

public class ScaleToScreen : MonoBehaviour
{
    // This script is intended for direct child objects of a Camera (e.g. Sprite Mask)

    [SerializeField] private Vector2 screenScale = new Vector2(0.5f, 1f);
    [SerializeField] private Vector2 anchoredPosition = new Vector2(-1f, 0f);
    private Vector2 screenScaleOnLastCalculation;
    private Vector2 anchoredPosOnLastCalculation;

    private void Start()
    {
        Rescale();
    }

    private void Update()
    {
        if (screenScaleOnLastCalculation != screenScale || anchoredPosOnLastCalculation != anchoredPosition)
        {
            Rescale();
        }
    }

    private void Rescale()
    {
        transform.ScaleToScreen(screenScale, anchoredPosition);
    }
}
