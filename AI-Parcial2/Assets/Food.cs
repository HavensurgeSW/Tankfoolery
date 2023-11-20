using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food {
    
    private Vector2Int pos;
    
    public Food(Vector2Int position){
        pos = position;
    }

    public void Init(Vector2Int position) { 
        pos.x = position.x;
        pos.y = position.y;
        
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
