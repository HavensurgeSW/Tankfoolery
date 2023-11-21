using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]FoodManager foodManager;
    [SerializeField]GameObject agent;

    [Range(100, 500), Tooltip("Grid Size")] public int gridSize = 100;
    [SerializeField] gamegrid gameGrid;
   
    bool playerTurn = false;
    void Start()
    {
        StartGame();
    }

    void StartGame() {
        Vector2Int sizeAPI = new Vector2Int(gridSize, gridSize);

        gameGrid.CreateGrid(sizeAPI);
        foodManager.PopulateGridWithFood(gridSize);
        
        Instantiate(agent);
        agent.GetComponent<Agent>().Init(new Vector2Int(0,0));

    }
    void SwitchTurn() { 
        if (playerTurn) { playerTurn = false; } else { playerTurn = true; }
    }

   
}
