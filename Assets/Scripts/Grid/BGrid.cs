using UnityEngine;

public class BGrid<TGridObject>
{
    public event System.EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : System.EventArgs { public int x; public int y; }

    private Vector3 originPosition;

    private int width;
    private int height;
    private float cellSize;

    private TGridObject[,] gridArray;

    private TextMesh[,] debugTextArray;

    public BGrid(int width, int height, float cellSize, Vector3 originPosition, System.Func<TGridObject> createGridObject)
    {
        this.originPosition = originPosition;

        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new TGridObject[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.Length; x++)
        {
            for (int y = 0; y < gridArray.Length; y++)
            {
                gridArray[x, y] = createGridObject();
            }
        }

        bool showDebug = true;
        if (showDebug)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = ExtensionMethods.CreateWorldText
                    (
                        gridArray[x, y]?.ToString(),
                        null,
                        GridToWorldPosition(x, y) + (new Vector3(cellSize, cellSize) * 0.5f),
                        12,
                        Color.white,
                        TextAnchor.MiddleCenter
                    );

                    Debug.DrawLine(GridToWorldPosition(x, y), GridToWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GridToWorldPosition(x, y), GridToWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GridToWorldPosition(0, height), GridToWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GridToWorldPosition(width, 0), GridToWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
    }


    public void TriggerGridObjectChanged(int x, int y)
    {
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }


    private Vector3 GridToWorldPosition(int x, int y)
    {
        return (new Vector3(x, y) * cellSize) + originPosition;
    }
    private void WorldToGridPosition(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }


    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();

            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
        //else { Debug.LogError("Ignored: value of " + value + " set at: (" + x + ", " + y + ") was outside of grid."); }
        
    }
    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        WorldToGridPosition(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }


    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        return default;
    }
    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        WorldToGridPosition(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
}
