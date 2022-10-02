using UnityEditor;
using UnityEngine;


public enum FlowFieldDisplayType { None, AllIcons, DestinationIcon, CostField, IntegrationField };

public class GridDebug : MonoBehaviour
{
    public GridController gridController;
    public bool displayGrid;

    public FlowFieldDisplayType curDisplayType;

    private Vector2Int gridSize;
    private float cellRadius;
    private FlowField curFlowField;

    public void SetFlowField(FlowField newFlowField)
    {
        curFlowField = newFlowField;
        cellRadius = newFlowField.cellRadius;
        gridSize = newFlowField.gridSize;
    }

    public void DrawFlowField()
    {
        ClearCellDisplay();

        switch (curDisplayType)
        {
            case FlowFieldDisplayType.AllIcons:
                DisplayAllCells();
                break;

            case FlowFieldDisplayType.DestinationIcon:
                DisplayDestinationCell();
                break;

            default:
                break;
        }
    }

    private void DisplayAllCells()
    {
        if (curFlowField == null) 
            return;
        foreach (Cell curCell in curFlowField.grid)
        {
            DisplayCell(curCell);
        }
    }

    private void DisplayDestinationCell()
    {
        if (curFlowField == null)
            return; 
        DisplayCell(curFlowField.destinationCell);
    }

    private void DisplayCell(Cell cell)
    {
        GameObject iconGO = new GameObject();
        iconGO.transform.parent = transform;
        iconGO.transform.position = cell.worldPos;
        Vector2 iconPos = iconGO.transform.position;

        if (cell.cost == 0)
        {
            Debug.DrawLine(new Vector2(iconPos.x, iconPos.y - 0.5f), new Vector2(iconPos.x, iconPos.y + 0.5f), Color.yellow, 600f);
        }
        else if (cell.cost == byte.MaxValue)
        {
            Debug.DrawLine(new Vector2(iconPos.x - 0.5f, iconPos.y - 0.5f), new Vector2(iconPos.x + 0.5f, iconPos.y + 0.5f), Color.red, 600f);
            Debug.DrawLine(new Vector2(iconPos.x + 0.5f, iconPos.y - 0.5f), new Vector2(iconPos.x - 0.5f, iconPos.y + 0.5f), Color.red, 600f);
        }
        else if (cell.bestDirection == GridDirection.North)
        {
            Debug.DrawLine(new Vector2(iconPos.x, iconPos.y), new Vector2(iconPos.x, iconPos.y + 0.25f), Color.red, 600f);
        }
        else if (cell.bestDirection == GridDirection.South)
        {
            Debug.DrawLine(new Vector2(iconPos.x, iconPos.y), new Vector2(iconPos.x, iconPos.y - 0.25f), Color.red, 600f);
        }
        else if (cell.bestDirection == GridDirection.East)
        {
            Debug.DrawLine(new Vector2(iconPos.x, iconPos.y), new Vector2(iconPos.x + 0.25f, iconPos.y), Color.red, 600f);
        }
        else if (cell.bestDirection == GridDirection.West)
        {
            Debug.DrawLine(new Vector2(iconPos.x, iconPos.y), new Vector2(iconPos.x - 0.25f, iconPos.y), Color.red, 600f);
        }
        else if (cell.bestDirection == GridDirection.NorthEast)
        {
            Debug.DrawLine(new Vector2(iconPos.x, iconPos.y), new Vector2(iconPos.x + 0.25f, iconPos.y + 0.25f), Color.red, 600f);
        }
        else if (cell.bestDirection == GridDirection.NorthWest)
        {
            Debug.DrawLine(new Vector2(iconPos.x, iconPos.y), new Vector2(iconPos.x - 0.25f, iconPos.y + 0.25f), Color.red, 600f);
        }
        else if (cell.bestDirection == GridDirection.SouthEast)
        {
            Debug.DrawLine(new Vector2(iconPos.x, iconPos.y), new Vector2(iconPos.x + 0.25f, iconPos.y - 0.25f), Color.red, 600f);
        }
        else if (cell.bestDirection == GridDirection.SouthWest)
        {
            Debug.DrawLine(new Vector2(iconPos.x, iconPos.y), new Vector2(iconPos.x - 0.25f, iconPos.y - 0.25f), Color.red, 600f);
        }
        else
        {
            Debug.DrawLine(new Vector2(iconPos.x - 0.5f, iconPos.y - 0.5f), new Vector2(iconPos.x + 0.5f, iconPos.y + 0.5f), Color.red, 600f);
            Debug.DrawLine(new Vector2(iconPos.x + 0.5f, iconPos.y - 0.5f), new Vector2(iconPos.x - 0.5f, iconPos.y + 0.5f), Color.red, 600f);
        }
    }

    public void ClearCellDisplay()
    {
        foreach (Transform t in transform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (displayGrid)
        {
            if (curFlowField == null)
            {
                DrawGrid(gridController.gridSize, Color.yellow, gridController.cellRadius);
            }
            else
            {
                DrawGrid(gridSize, Color.green, cellRadius);
            }
        }


    }

    private void DrawGrid(Vector2Int drawGridSize, Color drawColor, float drawCellRadius)
    {
        Gizmos.color = drawColor;
        for (int x = gridController.startPos.x; x < drawGridSize.x  + gridController.startPos.x; x++)
        {
            for (int y = gridController.startPos.y; y < drawGridSize.y + gridController.startPos.y; y++)
            {
                Vector3 center = new Vector3(drawCellRadius * 2 * x + drawCellRadius, drawCellRadius * 2 * y + drawCellRadius);
                Vector3 size = Vector3.one * drawCellRadius * 2;
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}