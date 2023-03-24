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

        //screenScale = new Vector2(1 - screenScale.x, 1 - screenScale.y);

        //Vector3 worldPosition = scale * anchors * screenScale;
        //worldPosition.z = transform.localPosition.z;

        //transform.localPosition = worldPosition;
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

        return new Vector2(screenToWorldWidth, screenToWorldHeight);
    }
}
