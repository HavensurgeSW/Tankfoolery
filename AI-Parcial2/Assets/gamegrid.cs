using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamegrid : MonoBehaviour
{

    private int height = 100;
    private int width = 100;
    public float GridSpaceSize { get; private set; } = 5f;

    [SerializeField] private GameObject gridCellPrefab;
    private GameObject[,] gameGrid;

   
    public void CreateGrid() {

        gameGrid = new GameObject[width,height];
        if (gridCellPrefab == null) Debug.LogError("ERROR: grid prefab is not assigned");

        for (int y = 0; y < height; y++) { 
            for (int x = 0; x < width; x++) {
                gameGrid[x, y] = Instantiate(gridCellPrefab, new Vector3(x* GridSpaceSize, y*GridSpaceSize), Quaternion.identity);
                gameGrid[x, y].GetComponent<GridCell>().SetPosition(x,y);
                gameGrid[x, y].transform.parent = transform;
                gameGrid[x, y].gameObject.name= "Grid: " + x.ToString() + " " + y.ToString();
            }
        }


    }

    public GridCell GetGridCell(Vector2Int gridPos){
        return gameGrid[gridPos.x, gridPos.y].GetComponent<GridCell>();
    }


    public Vector2Int GetGridPosFromWorld(Vector3 worldPosition) { 
        int x = Mathf.FloorToInt(worldPosition.x/GridSpaceSize);
        int y = Mathf.FloorToInt(worldPosition.y / GridSpaceSize);

        x = Mathf.Clamp(x, 0, width);
        y = Mathf.Clamp(x, 0, height);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPosFromGrid(Vector2Int gridPos) { 
        float x = gridPos.x * GridSpaceSize;
        float y = gridPos.y * GridSpaceSize;

        return new Vector3(x, 0, y);
    }
    
}
