using UnityEngine;

public static class ExtensionMethods
{
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

        //Debug.Log("Calculated World Size: " + new Vector2(screenToWorldWidth, screenToWorldHeight));

        return new Vector2(screenToWorldWidth, screenToWorldHeight);
    }

    public static float Remap(this float input, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return (input - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin;
    }

    public static void DrawBox(this Vector2 position, float radius)
    {
        // Left
        Debug.DrawLine(position + new Vector2(-radius, -radius), position + new Vector2(-radius, radius));

        // Top
        Debug.DrawLine(position + new Vector2(-radius, radius), position + new Vector2(radius, radius));

        // Right
        Debug.DrawLine(position + new Vector2(radius, radius), position + new Vector2(radius, -radius));

        // Bottom
        Debug.DrawLine(position + new Vector2(radius, -radius), position + new Vector2(-radius, -radius));
    }
    public static void DrawBox(this Vector2 position, float radius, Color boxColor)
    {
        Debug.DrawLine(position + new Vector2(-radius, -radius), position + new Vector2(-radius, radius), boxColor);
        Debug.DrawLine(position + new Vector2(-radius, radius), position + new Vector2(radius, radius), boxColor);
        Debug.DrawLine(position + new Vector2(radius, radius), position + new Vector2(radius, -radius), boxColor);
        Debug.DrawLine(position + new Vector2(radius, -radius), position + new Vector2(-radius, -radius), boxColor);
    }
    public static void DrawBox(this Vector3 position, float radius)
    {
        Debug.DrawLine(position + new Vector3(-radius, -radius), position + new Vector3(-radius, radius));
        Debug.DrawLine(position + new Vector3(-radius, radius), position + new Vector3(radius, radius));
        Debug.DrawLine(position + new Vector3(radius, radius), position + new Vector3(radius, -radius));
        Debug.DrawLine(position + new Vector3(radius, -radius), position + new Vector3(-radius, -radius));
    }
    public static void DrawBox(this Vector3 position, float radius, Color boxColor)
    {
        Debug.DrawLine(position + new Vector3(-radius, -radius), position + new Vector3(-radius, radius), boxColor);
        Debug.DrawLine(position + new Vector3(-radius, radius), position + new Vector3(radius, radius), boxColor);
        Debug.DrawLine(position + new Vector3(radius, radius), position + new Vector3(radius, -radius), boxColor);
        Debug.DrawLine(position + new Vector3(radius, -radius), position + new Vector3(-radius, -radius), boxColor);
    }
}
