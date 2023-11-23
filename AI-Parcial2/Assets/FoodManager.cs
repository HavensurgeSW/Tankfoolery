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

    [SerializeField] public List<Food> foodList { get; private set; } = new List<Food>();

    

    public void SpawnFood(Vector2Int foodPos){
        GameObject tempFood = Instantiate(FoodPrefab, new Vector3(foodPos.x *HSSUtils.GridSpaceSize, foodPos.y * HSSUtils.GridSpaceSize, HSSUtils.defaultZ), Quaternion.identity);
        tempFood.name = "Food: "+ foodPos.x + " "+ foodPos.y;
        tempFood.transform.parent = transform;

        tempFood.GetComponent<Food>().Init(foodPos);
        foodList.Add(tempFood.GetComponentInChildren<Food>());
        currentFood++;
    }

    public void ShuffleFood(){
        List<Vector2Int> posList = new List<Vector2Int>();
        Vector2Int randomPosition = new Vector2Int();

        for (int i = 0; i < currentFood; i++)
        {
            do
            {
                randomPosition.x = Random.Range(1, HSSUtils.gridSize-1);
                randomPosition.y = Random.Range(1, HSSUtils.gridSize-1);
            } while (posList.Contains(randomPosition));

            posList.Add(randomPosition);

            SpawnFood(randomPosition);
            GameGrid.GetGridCell(randomPosition).FoodWasPlaced();
        }
    }

    public int GetCurrentFood() {
        return currentFood;
    }

    public int GetInitialFood(){
        return initialFood;
    }
    public void PopulateGridWithFood(){
        List<Vector2Int> posList = new List<Vector2Int>();
        Vector2Int randomPosition = new Vector2Int();

        int TEMPtotalFood = HSSUtils.gridSize*2; //not exactly right. Tiene que ser Agent Amount *2, y AA tiene un MAX(Gridsize*2);

        for (int i = 0; i < TEMPtotalFood; i++)
        {
            do
            {
                randomPosition.x = Random.Range(1, HSSUtils.gridSize - 1);
                randomPosition.y = Random.Range(1, HSSUtils.gridSize - 1);
            } while (posList.Contains(randomPosition));

            posList.Add(randomPosition);

            SpawnFood(randomPosition);
            GameGrid.GetGridCell(randomPosition).FoodWasPlaced();
        }
    }

    public void EatFood(Vector2Int foodPosition){
        Food toRemove = foodList.Find(food => food.GetPosition() == foodPosition);
            Debug.Log("Removed food at: "+ foodPosition.x+" " +foodPosition.y);
        if (foodList.Contains(toRemove)) {
            foodList.Remove(toRemove);
            GameGrid.GetGridCell(foodPosition).FoodWasEaten();
            currentFood--;
        }
    }

    public void ClearFood() {
        foreach (Food f in foodList) {
            GameGrid.GetGridCell(f.GetPosition()).FoodWasEaten();
        }
        foodList.Clear(); ;
        
        currentFood = 0;
    }
}
