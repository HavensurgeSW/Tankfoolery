using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]FoodManager foodManager;
    [SerializeField]gamegrid gameGrid;
    bool playerTurn = false;
    void Start()
    {
        StartGame();
    }

    void StartGame() {

        gameGrid.CreateGrid();
        foodManager.PopulateGridWithFood(foodManager.GetInitialFood());
        
        
    }

   

    
    void Update()
    {
        
    }

    void SwitchTurn() { 
        if (playerTurn) { playerTurn = false; } else { playerTurn = true; }
    }

   
}
