using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    gamegrid gameGrid;
    [SerializeField] private LayerMask gridLayerMask;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GridCell cellMouseIsOver = IsMouseOverAGridSpace();
        if (cellMouseIsOver != null) {
            if (Input.GetMouseButtonDown(0)) {
                cellMouseIsOver.GetComponentInChildren<SpriteRenderer>().material.color = Color.green;
            }
        }
    }

    private GridCell IsMouseOverAGridSpace() { 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, gridLayerMask))
        {
            return hitInfo.transform.GetComponent<GridCell>();
        }
        else { 
            return null;
        }

    }
}
