using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food 
{
    [SerializeField] GameObject FoodPrefab;
    private Vector2Int pos;
    bool eaten;

    
    void Init() {
        pos.x = 0;
        pos.y = 0;
        eaten = false;
    }
    void Init(Vector2Int position) { 
        pos.x = position.x;
        pos.y = position.y;
        eaten = false;
    }

    public void SetPosition(Vector2Int position){ 
        pos.x = position.x;
        pos.y = position.y;
    }

    public void SetPosition(int x, int y){
        pos.x = x;
        pos.y = y;
    }

    public Vector2Int GetPosition(){
        return pos;
    }

}
