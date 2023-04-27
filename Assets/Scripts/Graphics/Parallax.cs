using UnityEngine;

public class Parallax : MonoBehaviour
{
    private readonly Vector2 screenSizeInUnits = new Vector2
    (
        640f / 24f,
        360f / 24f
    );
    
    [SerializeField] private Renderer rend;
    [SerializeField] private Transform[] gridVisualTransforms;

    [Space]

    [SerializeField, Range(0f, 1f)] private float offsetX;

    private void Awake()
    {
        if (rend == null) { rend = GetComponent<Renderer>(); }
    }

    private void Update()
    {
        rend.material.mainTextureOffset = Vector2.right * offsetX;
        for (int i = 0; i < gridVisualTransforms.Length; i++)
        {
            gridVisualTransforms[i].localPosition = Vector3.left * (screenSizeInUnits.x * offsetX);
        }
    }
}
