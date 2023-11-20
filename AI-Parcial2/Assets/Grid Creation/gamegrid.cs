using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamegrid : MonoBehaviour
{
    public float GridSpaceSize { get; private set; } = 5f;


    private int height = 100;
    private int width = 100;

    [SerializeField] private GameObject gridCellPrefab;
    private GameObject[,] gameGrid;

   
    public void CreateGrid(Vector2Int size) {

        gameGrid = new GameObject[size.x, size.y];
        if (gridCellPrefab == null) Debug.LogError("ERROR: grid prefab is not assigned");

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                gameGrid[x, y] = Instantiate(gridCellPrefab, new Vector3(x * GridSpaceSize, y * GridSpaceSize), Quaternion.identity);
                gameGrid[x, y].GetComponent<GridCell>().SetPosition(x, y);
                gameGrid[x, y].transform.parent = transform;
                gameGrid[x, y].gameObject.name = "Grid: " + x.ToString() + " " + y.ToString();
            }
        }


    }

    public GridCell GetGridCell(Vector2Int gridPos){
        return gameGrid[gridPos.x, gridPos.y].GetComponent<GridCell>();
    }
    
}
