using UnityEngine;

public static class ExtensionMethods
{
    public static void ScaleToScreen(this Transform transform, Vector2 screenScale, Vector2 anchors)
    {
        transform.LocalReset2D();

        // Scales an Object's size to match the Camera, multiplied by a proportion on the X or Y Axis.
        // Also anchors their position to a point relative to the Camera.

        Vector2 scale = GetWorldSize(Camera.main);
        transform.localScale = scale * screenScale;

        Vector3 worldPosition = new Vector3
        (
            ((scale.x * anchors.x) + transform.localScale.x) * 0.5f,
            ((scale.y * anchors.y) + transform.localScale.y) * 0.5f,
            transform.localPosition.z
        );

        transform.localPosition = worldPosition;
    }

    public static void LocalReset2D(this Transform transform)
    {
        // Resets Transform values to the parent Transform's values.

        transform.localPosition = new Vector3(0f, 0f, transform.localPosition.z);
        transform.localRotation = Quaternion.Euler(0f, 0f, transform.localEulerAngles.z);
        transform.localScale = new Vector3(1f, 1f, transform.localScale.z);
    }

    public static Vector2 GetWorldSize(this Camera camera)
    {
        // Returns the Camera's Orthographic size converted to world units.

        float screenToWorldHeight = 2 * camera.orthographicSize;
        float screenToWorldWidth = screenToWorldHeight * camera.aspect;

        Debug.Log("Calculated World Size: " + new Vector2(screenToWorldWidth, screenToWorldHeight));

        return new Vector2(screenToWorldWidth, screenToWorldHeight);
    }
}
