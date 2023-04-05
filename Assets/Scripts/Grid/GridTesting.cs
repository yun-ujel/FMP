using UnityEngine;

public class GridTesting : MonoBehaviour
{
    private BGrid<bool> grid;
    private void Start()
    {
        grid = new BGrid<bool>(4, 2, 2f, Vector3.zero, () => new bool());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.SetGridObject(position, true);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.SetGridObject(position, false);
        }
    }
}
