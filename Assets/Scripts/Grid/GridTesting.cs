using UnityEngine;

public class GridTesting : MonoBehaviour
{
    private BGrid grid;
    private void Start()
    {
        grid = new BGrid(4, 2, 2f, Vector3.left);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.SetValue(position, grid.GetValue(position) + 1);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.SetValue(position, grid.GetValue(position) - 1);
        }
    }
}
