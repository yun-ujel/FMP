using UnityEngine;
public static class ExtensionMethods
{
    public static float Remap(this float input, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return (input - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin;
    }
}
