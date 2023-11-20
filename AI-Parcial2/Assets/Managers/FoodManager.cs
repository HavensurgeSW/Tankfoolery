using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using AI.Utils;

public class FoodManager : MonoBehaviour
{

    int initialFood;
    [SerializeField] int currentFood;
    [SerializeField] gamegrid GameGrid;
    [SerializeField] GameObject FoodPrefab;

    [SerializeField]public List<Food> foodList { get; private set; } = new List<Food>();

    

    public void SpawnFood(Vector2Int foodPos){
        GameObject tempFood = Instantiate(FoodPrefab, new Vector3(foodPos.x *HSSUtils.GridSpaceSize, foodPos.y * HSSUtils.GridSpaceSize, -5f), Quaternion.identity);
        tempFood.name = "Food: "+ foodPos.x + " "+ foodPos.y;
        tempFood.transform.parent = transform;
        Food f = new Food(foodPos);
        foodList.Add(f);
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
    public void PopulateGridWithFood(int gridsize){
        List<Vector2Int> posList = new List<Vector2Int>();
        Vector2Int randomPosition = new Vector2Int();

        int TEMPtotalFood = gridsize*2;

        for (int i = 0; i < TEMPtotalFood; i++)
        {
            do
            {
                randomPosition.x = Random.Range(1, gridsize-1);
                randomPosition.y = Random.Range(1, gridsize-1);
            } while (posList.Contains(randomPosition));

            posList.Add(randomPosition);

            SpawnFood(randomPosition);
            GameGrid.GetGridCell(randomPosition).FoodWasPlaced();
        }
    }

    public void EatFood(Vector2Int foodPosition){
        Food toRemove = foodList.Find(food => food.GetPosition() == foodPosition);
        if (foodList.Contains(toRemove)) {
            foodList.Remove(toRemove);
            currentFood--;
        }
    }
}
