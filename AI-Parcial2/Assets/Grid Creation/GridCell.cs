using AI.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    Vector2Int pos;
    List<GridCell> adjacents;
    gamegrid map;

    public GameObject ObjectOnThisCell = null;
    public bool hasFood;
    public bool isOccupied;


    public void Init(int x, int y, gamegrid m) {
        SetPosition(x,y);
        map = m;
    }
    public void SetPosition(int x, int y) { 
        pos.x = x; 
        pos.y = y;
    }

    public Vector2Int GetPosition() { 
        return new Vector2Int(pos.x, pos.y);
    }

    

    public void FoodWasPlaced() {
        this.transform.GetComponent<MeshRenderer>().material.color = Color.green;
        hasFood = true;
    }

    public void FoodWasEaten() {
        this.transform.GetComponent<MeshRenderer>().material.color = Color.white;
        hasFood= false;
    }

    public void SetAdjacents() { 
        //Solo llamar DESPUES de generar el mapa
        adjacents = new List<GridCell>();

        GridCell leftPosition = null;
        GridCell upPosition = null;
        GridCell downPosition = null;
        GridCell rightPosition = null;

       
        if (!(pos.x == HSSUtils.GetGridSize() - 1))
        {
            rightPosition = map.GetGridCell(new Vector2Int(pos.x + 1, pos.y));
            adjacents.Add(rightPosition);
        }
        else {
            rightPosition = null;
            //adjacents.Add(null);
        }

        if (!(pos.x == 0))
        {
            leftPosition = map.GetGridCell(new Vector2Int(pos.x -1, pos.y));
            adjacents.Add(leftPosition);
        }
        else {
            leftPosition = null;
            //adjacents.Add(leftPosition);
        }

        if (!(pos.y == 0))
        {
            downPosition = map.GetGridCell(new Vector2Int(pos.x, pos.y-1));
            adjacents.Add(downPosition);
        }
        else
        {
            downPosition = null;
            //adjacents.Add(downPosition);
        }

        if (!(pos.y == HSSUtils.GetGridSize() - 1))
        {
            upPosition = map.GetGridCell(new Vector2Int(pos.x, pos.y+1));
            adjacents.Add(upPosition);
        }
        else
        {
            upPosition = null;
            //adjacents.Add(upPosition);
        }
    }

    public List<GridCell> GetAdjacents(){
        return adjacents;
    }
}
