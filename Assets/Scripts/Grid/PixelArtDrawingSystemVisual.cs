using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PixelArtDrawingSystemVisual : MonoBehaviour
{
    private Mesh mesh;
    private BGrid<PixelArtDrawingSystem.GridPixel> grid;

    private Vector3[] verts;
    private Vector2[] uvs;
    private int[] tris;

    [SerializeField] private int paletteSize;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(BGrid<PixelArtDrawingSystem.GridPixel> grid)
    {
        this.grid = grid;
        UpdateGridVisual();

        grid.OnGridValueChanged += BGrid_OnGridValueChanged;
    }

    private void BGrid_OnGridValueChanged(object sender, BGrid<PixelArtDrawingSystem.GridPixel>.OnGridValueChangedEventArgs args)
    {
        UpdateUVsOfQuad(args.x, args.y);
    }

    private void UpdateGridVisual()
    {
        ExtensionMethods.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out verts, out uvs, out tris);

        // Initialize Grid
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = (x * grid.GetHeight()) + y;

                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                PixelArtDrawingSystem.GridPixel gridPixel = grid.GetGridObject(x, y);
                Vector2 uv = gridPixel.GetColourIndexAsUV(paletteSize - 1);

                ExtensionMethods.AddArraysToMeshAsQuad(verts, uvs, tris, index, grid.GridToWorldPosition(x, y) + quadSize * .5f, quadSize, uv, uv);
            }
        }

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = tris;
    }

    private void UpdateUVsOfQuad(int x, int y)
    {
        int index = ((x * grid.GetHeight()) + y) * 4;
        Vector2 colorIndexasUV = grid.GetGridObject(x, y).GetColourIndexAsUV(paletteSize - 1);

        for (int i = 0; i < 4; i++)
        {
            uvs[index + i] = colorIndexasUV;
        }

        mesh.uv = uvs;
    }
}
