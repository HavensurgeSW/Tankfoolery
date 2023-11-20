using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentData : MonoBehaviour
{
    
    [SerializeField] Material[] teamMaterials = new Material[2];
    [SerializeField] Vector2Int position;


    private void Init(Vector2Int pos)
    {
        position.x = pos.x;
        position.y = pos.y;
    }
}
