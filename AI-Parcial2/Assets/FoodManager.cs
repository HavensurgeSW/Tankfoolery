using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class FoodManager : MonoBehaviour
{

    [SerializeField] int initialFood;
    [SerializeField] int currentFood;
    [SerializeField] gamegrid GameGrid;
    [SerializeField] GameObject FoodPrefab;

    

    public void SpawnFood(Vector2Int foodPos){

        
        Instantiate(FoodPrefab, new Vector3(foodPos.x *GameGrid.GridSpaceSize, foodPos.y * GameGrid.GridSpaceSize, -5f), Quaternion.identity);
        currentFood++;
    }

    public void ShuffleFood(){ 
        
    }

    public int GetCurrentFood() {
        return currentFood;
    }

    public int GetInitialFood(){
        return initialFood;
    }

    public void PopulateGridWithFood(int foodToGenerate){
        List<Vector2Int> posList = new List<Vector2Int>();
        Vector2Int randomPosition = new Vector2Int();

        for (int i = 0; i < foodToGenerate; i++)
        {
            do
            {
                randomPosition.x = Random.Range(1, 99);
                randomPosition.y = Random.Range(1, 99);
            } while (posList.Contains(randomPosition));

            posList.Add(randomPosition);

            SpawnFood(randomPosition);
            GameGrid.GetGridCell(randomPosition).FoodWasPlaced();
        }
    }
}
