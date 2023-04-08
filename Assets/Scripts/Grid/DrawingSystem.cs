using UnityEngine;

public class DrawingSystem : MonoBehaviour
{
    [SerializeField] private DrawingSystemVisual[] pixelArtDrawingSystemVisuals = new DrawingSystemVisual[4];
    private BGrid<GridPixel>[] grids = new BGrid<GridPixel>[4];

    private Vector2Int gridSize = new Vector2Int(80, 180);
    private float pixelsPerUnitMultiplier = 12f;


    // Mesh Indices use Int16, meaning that you can't have more than 65,535 vertexes in a mesh.
    // To bypass this, we're using 4 different grids with separate meshes, evenly spaced to cover the full screen.
    // Each Pixel on a grid equates to 6 pixels on a 1920x1080 screen.
    private void Awake()
    {
        for (int i = 0; i < pixelArtDrawingSystemVisuals.Length; i++)
        {
            grids[i] = new BGrid<GridPixel>
            (
                gridSize.x, gridSize.y,
                1f / pixelsPerUnitMultiplier,
                new Vector3
                (
                    (i * (20f / 3f)) - (40f / 3f),
                    -7.5f
                ),
                (BGrid<GridPixel> grid, int x, int y) => new GridPixel(grid, x, y)
            );
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ApplyColourToPixel(2, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if (Input.GetMouseButton(1))
        {
            ApplyColourToCircle(1, Camera.main.ScreenToWorldPoint(Input.mousePosition), 8);
        }
        else if (Input.GetMouseButton(2))
        {
            ApplyColourToCircle(2, Camera.main.ScreenToWorldPoint(Input.mousePosition), 8);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ApplyColourToScreen(0);
        }
    }

    private void Start()
    {
        for (int i = 0; i < pixelArtDrawingSystemVisuals.Length; i++)
        {
            pixelArtDrawingSystemVisuals[i].SetGrid(grids[i]);
        }
    }

    public void ApplyColourToPixel(int colourIndex, Vector3 colourPosition)
    {
        float gridSizeX = 1f / pixelsPerUnitMultiplier * gridSize.x;

        GridPixel pixel = GetPixelAtPosition(colourPosition);
        if (pixel != null && pixel.ColourIndex != colourIndex) pixel.SetColourIndex(colourIndex);
    }

    private GridPixel GetPixelAtPosition(Vector3 position)
    {
        float gridSizeX = 1f / pixelsPerUnitMultiplier * gridSize.x;
        if (position.x < -1f * gridSizeX)
        {
            return grids[0].GetGridObject(position);
        }
        else if (position.x < 0f)
        {
            return grids[1].GetGridObject(position);
        }
        else if (position.x < gridSizeX)
        {
            return grids[2].GetGridObject(position);
        }
        else if (position.x < 2 * gridSizeX)
        {
            return grids[3].GetGridObject(position);
        }
        return null;
    }

    public void ApplyColourToCircle(int colourIndex, Vector3 colourPosition, int radius)
    {
        float worldPixelSize = 1f / pixelsPerUnitMultiplier;
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (new Vector2(x, y).magnitude < radius)
                {
                    ApplyColourToPixel(colourIndex, colourPosition + new Vector3
                    (
                        x * worldPixelSize,
                        y * worldPixelSize
                    ));
                }
            }
        }
    }

    public void ApplyColourToScreen(int colourIndex)
    {
        for (int i = 0; i < grids.Length; i++)
        {
            for (int x = 0; x < grids[i].GetWidth(); x++)
            {
                for (int y = 0; y < grids[i].GetHeight(); y++)
                {
                    GridPixel pixel = grids[i].GetGridObject(x, y);
                    if (pixel.ColourIndex != colourIndex)
                    {
                        pixel.SetColourIndex(colourIndex);
                    }
                }
            }
        }
    }

    public class GridPixel
    {
        // The Grid this Pixel/Object is placed on
        private BGrid<GridPixel> grid;

        // The Position of this Pixel/Object on the grid
        private int x;
        private int y;

        public int ColourIndex { get; private set; }

        public void SetColourIndex(int index)
        {
            ColourIndex = index;
            grid.TriggerGridValueChanged(x, y);
        }

        public Vector2 GetColourIndexAsUV(int paletteSize)
        {
            return new Vector2
                (
                    (float)ColourIndex / paletteSize,
                    0
                );
        }

        public GridPixel(BGrid<GridPixel> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return ColourIndex.ToString();
        }
    }
}
