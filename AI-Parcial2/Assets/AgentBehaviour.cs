using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private FoodManager foodHandler = null;
    private Action<Vector2Int> onFoodEaten = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(foodTag))
        {
            Debug.Log("Collided with " + foodTag);
            Food food = null;

            if (collision.gameObject.TryGetComponent(out food))
            {
                onFoodEaten?.Invoke(food.GetPosition());
                foodHandler.EatFood(food.GetPosition());

                collision.enabled = false;               
                Destroy(collision.gameObject, 0.25f);
            }
        }
    }

    public void SetBehaviourNeeds(Action<Vector2Int> onFoodEaten, FoodManager foodHandler)
    {
        this.onFoodEaten = onFoodEaten;

        if (this.foodHandler != null)
            return;

        this.foodHandler = foodHandler;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            Movement(MoveDirection.Up, 100, 100);
        }
    }

    public void Movement(MoveDirection direction, int limitX, int limitY, Action OnReachLimitY = null, Action OnReachLimitX = null)
    {
        switch (direction)
        {
            case MoveDirection.Left:
                if (transform.position.x - 1 > 0)
                {
                    transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                }
                else
                {
                    OnReachLimitX?.Invoke();
                }
                break;
            case MoveDirection.Right:
                if (transform.position.x + 1 < limitX)
                {
                    transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                }
                else
                {
                    OnReachLimitX?.Invoke();
                }
                break;
            case MoveDirection.Up:
                if (transform.position.y + 1 < limitY)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                }
                else
                {
                    OnReachLimitY?.Invoke();
                }
                break;
            case MoveDirection.Down:
                if (transform.position.y - 1 > 0)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
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
    }
}



