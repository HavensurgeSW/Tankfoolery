using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    Vector2Int pos;

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
}
