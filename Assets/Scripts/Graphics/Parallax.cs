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
    private DrawingSystem drawingSystem;

    [Space]

    private Vector2 offset;
    private Vector2 moveDir;

    private float timeSinceScrollStarted;
    private bool isScrolling;

    private void Awake()
    {
        if (rend == null) { rend = GetComponent<Renderer>(); }
        drawingSystem = DrawingSystem.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ScrollInDirection(Vector2.up);
        }

        if (isScrolling)
        {
            if (timeSinceScrollStarted <= 1f)
            {
                offset = Vector2.Lerp(offset, moveDir, timeSinceScrollStarted);

                rend.material.mainTextureOffset = offset;
                for (int i = 0; i < gridVisualTransforms.Length; i++)
                {
                    gridVisualTransforms[i].localPosition = screenSizeInUnits * -offset;
                }
            }
            else
            {
                drawingSystem.ApplyColourToScreen(0);
                for (int i = 0; i < gridVisualTransforms.Length; i++)
                {
                    gridVisualTransforms[i].localPosition = Vector3.zero;
                }
                moveDir = Vector2.zero;
                offset = Vector2.zero;
                isScrolling = false;
            }

            timeSinceScrollStarted += Time.deltaTime * 0.5f;
        }
    }

    public void ScrollInDirection(Vector2 direction)
    {
        moveDir = direction;
        timeSinceScrollStarted = 0f;
        isScrolling = true;
    }

    public void ScrollInDirection(float x = 0f, float y = 1f)
    {
        ScrollInDirection(new Vector2(x, y));
    }
}
