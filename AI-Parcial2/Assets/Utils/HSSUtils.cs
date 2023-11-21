using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.Utils { 
    public static class HSSUtils{
        public static float GridSpaceSize = 5f;
        public static float defaultZ = -5f;

        public static Vector3 GetWorldFromPosition(Vector2Int pos){

            return new Vector3(pos.x * GridSpaceSize, pos.y * GridSpaceSize, defaultZ);
        }
    }
}
