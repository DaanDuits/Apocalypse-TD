using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField curFlowField;
    [SerializeField]
    GridDebug debug;

    bool first = true;
    Vector2 endPos;
    
    public Vector2Int startPos;

    private void InitializeFlowField()
    {
        curFlowField = new FlowField(cellRadius, gridSize, startPos);
        curFlowField.CreateGrid();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !first)
        {
            InitializeFlowField();
            curFlowField.CreateCostField();

            Cell destinationCell = curFlowField.GetCellFromWorldPos(endPos);
            curFlowField.CreateIntegrationField(destinationCell);

            curFlowField.CreateFlowField();

            debug.SetFlowField(curFlowField);
            debug.DrawFlowField();
        }
    }
    public void CreateFlowField(Vector2 pos)
    {
        InitializeFlowField();
        curFlowField.CreateCostField();

        Cell destinationCell = curFlowField.GetCellFromWorldPos(pos);
        curFlowField.CreateIntegrationField(destinationCell);
        endPos = pos;

        curFlowField.CreateFlowField();

        debug.SetFlowField(curFlowField);
        debug.DrawFlowField();
        first = false;
    }
}
