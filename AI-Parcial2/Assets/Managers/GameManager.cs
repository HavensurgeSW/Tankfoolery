using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]FoodManager foodManager;

    [Range(100, 500), Tooltip("Grid Size")] public int gridSize = 100;
    [SerializeField]gamegrid gameGrid;

    bool playerTurn = false;
    void Start()
    {
        StartGame();
    }

    void StartGame() {
        Vector2Int sizeAPI = new Vector2Int(gridSize, gridSize);

        gameGrid.CreateGrid(sizeAPI);
        foodManager.PopulateGridWithFood(gridSize);

    }
    void SwitchTurn() { 
        if (playerTurn) { playerTurn = false; } else { playerTurn = true; }
    }

   
}
