using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using AI.Utils;
using UnityEngine.EventSystems;

public class Agent : MonoBehaviour
{

    [SerializeField] Material[] teamMaterials = new Material[2];
    [SerializeField] Vector2Int position;

    [SerializeField] AgentBehaviour behaviour;
    
    public void Init(Vector2Int pos)
    {
        UpdatePosition(pos);
    }

    public void UpdatePosition(Vector2Int pos) {
        position.x = pos.x;
        position.y = pos.y;

        transform.position = new Vector3(pos.x * HSSUtils.GridSpaceSize, pos.y * HSSUtils.GridSpaceSize, -5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            position = behaviour.Movement(MoveDirection.Up,position.x, position.y, 100, 100); //100 IS HARDCODED MAX GRID VALUE.
            transform.position = HSSUtils.GetWorldFromPosition(position);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            position = behaviour.Movement(MoveDirection.Left, position.x, position.y, 100, 100); //100 IS HARDCODED MAX GRID VALUE.
            transform.position = HSSUtils.GetWorldFromPosition(position);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            position = behaviour.Movement(MoveDirection.Down, position.x, position.y, 100, 100); //100 IS HARDCODED MAX GRID VALUE.
            transform.position = HSSUtils.GetWorldFromPosition(position);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            position = behaviour.Movement(MoveDirection.Right, position.x, position.y, 100, 100); //100 IS HARDCODED MAX GRID VALUE.
            transform.position = HSSUtils.GetWorldFromPosition(position);
        }
    }
}
