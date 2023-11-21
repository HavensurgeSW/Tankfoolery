using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using AI.Utils;

public class AgentBehaviour : MonoBehaviour
{

    public enum MovementPossibilities
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NONE
    }

    public string foodTag = "Food";

    FoodManager foodHandler = null;
    Action<Vector2Int> onFoodEaten = null;
    private void Awake()
    {
        foodHandler = FindObjectOfType<FoodManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag ==foodTag)
        {
            Debug.Log("Collided with " + foodTag);
            Food food = collision.transform.GetComponent<Food>();
            foodHandler.EatFood(food.GetPosition());
            Destroy(collision.gameObject, 0.25f);          
        }
    }

    public void SetBehaviourNeeds(Action<Vector2Int> onFoodEaten, FoodManager foodHandler)
    {
        this.onFoodEaten = onFoodEaten;

        if (this.foodHandler != null)
            return;

        this.foodHandler = foodHandler;
    }

    public Vector2Int Movement(MoveDirection direction, int currentX, int currentY, int limitX, int limitY, Action OnReachLimitY = null, Action OnReachLimitX = null)
    {
        Vector2Int finalPos = new Vector2Int(currentX, currentY);
        switch (direction)
        {
            case MoveDirection.Left:
                if (currentX-1 > 0)
                {
                    //transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                    finalPos.x -= 1;
                    
                    
                }
                else
                {
                    OnReachLimitX?.Invoke();
                }
                break;
            case MoveDirection.Right:
                if (currentX + 1 < limitX)
                {
                    //transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                    finalPos.x += 1;
                   
                }
                else
                {
                    OnReachLimitX?.Invoke();
                }
                break;
            case MoveDirection.Up:
                if (currentY + 1 < limitY)
                {
                    //transform.position = new Vector3(transform.position.x, (transform.position.y + 1) *HSSUtils.GridSpaceSize, transform.position.z);
                    finalPos.y += 1;
                   
                }
                else
                {
                    OnReachLimitY?.Invoke();
                }
                break;
            case MoveDirection.Down:
                if (currentY - 1 > 0)
                {
                    //transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                    finalPos.y -= 1;

                }
                else
                {
                    OnReachLimitY?.Invoke();
                }
                break;
            case MoveDirection.None:
                transform.position = transform.position;
                break;
        }
        return finalPos;
    }
}



