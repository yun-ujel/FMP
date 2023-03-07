using UnityEngine;

public class CustomVFX : MonoBehaviour
{
    private Camera cam;

    [Header("Shockwave")]
    [SerializeField] private Material shockwaveMaterial;
    [Space]
    [SerializeField, Range(-10f, 0f)] private float maxMagnitude = -1f;
    [SerializeField, Range(0f, 1f)] private float maxSize = 0.05f;
    [SerializeField, Range(0f, 10f)] private float speed = 1f;
    [SerializeField, Range(0f, 1f)] private float maxRadius = 1f;
    [Space]
    [SerializeField] private AnimationCurve magnitudeDecreaseOverTime;
    [SerializeField] private AnimationCurve sizeDecreaseOverTime;
    private float shockTimer;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        if (shockTimer < (1f / shockwaveMaterial.GetFloat("_Speed")))
        {
            RunShock();
            shockTimer += Time.deltaTime;
        }
        else
        {
            shockwaveMaterial.SetFloat("_Magnitude", 0f);
        }
    }

    public void TriggerShock(Vector3 worldPosition)
    {
        SetShockValues(maxSize, maxMagnitude, speed);
        shockwaveMaterial.SetVector("_FocalPoint", cam.WorldToViewportPoint(worldPosition));
        shockTimer = 0f;
    }
    private void RunShock()
    {
        shockwaveMaterial.SetFloat("_Size", maxSize - (maxSize * sizeDecreaseOverTime.Evaluate(shockTimer * speed)));
        shockwaveMaterial.SetFloat("_Magnitude", maxMagnitude - (maxMagnitude * magnitudeDecreaseOverTime.Evaluate(shockTimer * speed)));
        shockwaveMaterial.SetFloat("_Radius", shockTimer * maxRadius);
    }
    private void SetShockValues(float size, float magnitude, float speed)
    {
        shockwaveMaterial.SetFloat("_Size", size);
        shockwaveMaterial.SetFloat("_Magnitude", magnitude);
        shockwaveMaterial.SetFloat("_Speed", speed);
    }
}
