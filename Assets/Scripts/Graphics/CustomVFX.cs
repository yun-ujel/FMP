using UnityEngine;

public class CustomVFX : MonoBehaviour
{
    private Camera cam;

    [Header("Shockwave")]
    // References
    [SerializeField] private Material shockwaveMaterial;
    
    [Space]

    // Max Values
    [SerializeField, Range(-10f, 0f)] private float maxMagnitude = -1f;
    [SerializeField, Range(0f, 1f)] private float maxSize = 0.05f;
    [SerializeField] private float duration;
    [SerializeField] private float maxRadius;
    
    [Space]

    // Animation
    [SerializeField] private AnimationCurve magnitudeDecreaseOverTime;
    [SerializeField] private AnimationCurve sizeDecreaseOverTime;
    private float shockTimer;

    // Defaults
    private float defaultMagnitude;
    private float defaultSize;
    private float defaultRadius;
    private Vector2 defaultFocalPoint;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        defaultMagnitude = shockwaveMaterial.GetFloat("_Magnitude");
        defaultSize = shockwaveMaterial.GetFloat("_Size");
        defaultRadius = shockwaveMaterial.GetFloat("_Radius");
        defaultFocalPoint = shockwaveMaterial.GetVector("_FocalPoint");
    }
    private void Update()
    {
        if (shockTimer < duration)
        {
            RunShock();
            shockTimer += Time.deltaTime;
        }
        else
        {
            SetShockDefaults();
        }
    }

    public void TriggerShock(Vector3 worldPosition)
    {
        SetShockValues(maxSize, maxMagnitude);
        shockwaveMaterial.SetVector("_FocalPoint", cam.WorldToViewportPoint(worldPosition));
        shockTimer = 0f;
    }
    private void RunShock()
    {
        shockwaveMaterial.SetFloat("_Size", maxSize - (maxSize * sizeDecreaseOverTime.Evaluate(shockTimer / duration)));
        shockwaveMaterial.SetFloat("_Magnitude", maxMagnitude - (maxMagnitude * magnitudeDecreaseOverTime.Evaluate(shockTimer / duration)));
        shockwaveMaterial.SetFloat("_Radius", maxRadius * (shockTimer / duration));
    }
    private void SetShockValues(float size, float magnitude)
    {
        shockwaveMaterial.SetFloat("_Size", size);
        shockwaveMaterial.SetFloat("_Magnitude", magnitude);
    }

    private void SetShockDefaults()
    {
        shockwaveMaterial.SetFloat("_Magnitude", defaultMagnitude);
        shockwaveMaterial.SetFloat("_Size", defaultSize);
        shockwaveMaterial.SetFloat("_Radius", defaultRadius);
        shockwaveMaterial.SetVector("_FocalPoint", defaultFocalPoint);
    }
}
