using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CustomVFX : MonoBehaviour
{
    private Camera cam;

    [Header("Shockwave")]
    // References
    [SerializeField] private Material shockwaveMaterial;
    [SerializeField] private Blit blit;

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

    private void Awake()
    {
        cam = GetComponent<Camera>();
        blit.SetActive(true);

        shockwaveMaterial = new Material(shockwaveMaterial);
        blit.settings.blitMaterial = shockwaveMaterial;
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
            SetShockValues(0f, 0f);
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

    private void OnDisable()
    {
        blit.SetActive(false);
    }
}
