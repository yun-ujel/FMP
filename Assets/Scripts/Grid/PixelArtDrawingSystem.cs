using UnityEngine;

public class PixelArtDrawingSystem : MonoBehaviour
{
    [SerializeField] private PixelArtDrawingSystemVisual pixelArtDrawingSystemVisual;
    private BGrid<GridPixel> grid;

    private void Awake()
    {
        grid = new BGrid<GridPixel>(8, 18, (10f/12f), new Vector3((-13f), -7.5f), (BGrid<GridPixel> grid, int x, int y) => new GridPixel(grid, x, y));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetGridObject(mousePosition).SetColourIndex(2);
        }
    }

    private void Start()
    {
        pixelArtDrawingSystemVisual.SetGrid(grid);
    }

    public class GridPixel
    {
        // The Grid this Pixel/Object is placed on
        private BGrid<GridPixel> grid;

        // The Position of this Pixel/Object on the grid
        private int x;
        private int y;

        // The Colour of this pixel/object, represented as an index on a palette
        private int colourIndex = 1;

        public void SetColourIndex(int index)
        {
            colourIndex = index;
            grid.TriggerGridValueChanged(x, y);
        }

        public Vector2 GetColorIndexAsUV(int paletteSize)
        {
            return new Vector2
                (
                    (float)colourIndex / paletteSize,
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
            return colourIndex.ToString();
        }
    }
}
