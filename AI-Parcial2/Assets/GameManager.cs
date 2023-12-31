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
    

    public void StartGame(){
        Vector2Int sizeAPI = new Vector2Int(gridSize, gridSize);

        gameGrid.CreateGrid(sizeAPI);
        foodManager.PopulateGridWithFood();

        PopulateBoardWithAgents();

    } 

    public void PopulateBoardWithAgents(){ 
        for (int i = 0; i < gridSize; i++)
        {
            Instantiate(agent);
            agent.GetComponent<Agent>().Init(new Vector2Int(i, 0));
        }

        for (int i = 0; i < gridSize; i++)
        {
            Instantiate(agent);
            agent.GetComponent<Agent>().Init(new Vector2Int(i, gridSize),false);
        }
    }
    void SwitchTurn() { 
        if (playerTurn) { playerTurn = false; } else { playerTurn = true; }
    }

   
}
