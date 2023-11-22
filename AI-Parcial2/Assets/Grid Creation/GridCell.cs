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

        GridCell upPosition = map.GetGridCell(new Vector2Int(pos.x, pos.y + 1));
        GridCell downPosition = map.GetGridCell(new Vector2Int(pos.x, pos.y-1));
        GridCell leftPosition = map.GetGridCell(new Vector2Int(pos.x-1, pos.y));
        GridCell rightPosition = map.GetGridCell(new Vector2Int(pos.x+1, pos.y));

        adjacents.Add(upPosition);
        adjacents.Add(downPosition);
        adjacents.Add(leftPosition);
        adjacents.Add(rightPosition);

    }

    public List<GridCell> GetAdjacents(){
        return adjacents;
    }
}
