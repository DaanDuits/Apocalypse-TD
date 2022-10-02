using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public Cell[,] grid;
    public Vector2Int gridSize;
    public float cellRadius;
    public Vector2Int startPos;
    public Cell destinationCell;

    private float cellDiameter;

    public FlowField(float _cellRadius, Vector2Int _gridSize, Vector2Int _startPos)
    {
        cellRadius = _cellRadius;
        cellDiameter = _cellRadius * 2f;
        gridSize = _gridSize;
        startPos = _startPos;
    }
    public void CreateGrid()
    {
        //create a grid on which the enemies can travel
        grid = new Cell[gridSize.x, gridSize.y];

        int x = 0;

        for (int posX = startPos.x; posX < gridSize.x + startPos.x; posX++)
        {
            int y = 0;
            for (int posY = startPos.y; posY < gridSize.y + startPos.y; posY++)
            {
                Vector2 worldPos = new Vector2(cellDiameter * posX + cellRadius, cellDiameter * posY + cellRadius);
                grid[x, y] = new Cell(worldPos, new Vector2Int(x, y));
                y++;
            }
            x++;
        }
    }

    public void CreateCostField()
    {
        //set the cost of each cell on the grid based on the objects in the scene
        Vector2 cellHalfExtents = Vector2.one * cellRadius;
        int terrainMask = LayerMask.GetMask("Impassible", "RoughTerrain");
        foreach (Cell curCell in grid)
        {
            Collider2D[] obstacles = Physics2D.OverlapBoxAll(curCell.worldPos, cellHalfExtents, 0f, terrainMask);
            bool hasIncreasedCost = false;
            foreach (Collider2D col in obstacles)
            {
                if (col.gameObject.layer == 8)
                {
                    curCell.IncreaseCost(255);
                    continue;
                }
                else if (!hasIncreasedCost && col.gameObject.layer == 9)
                {
                    curCell.IncreaseCost(50);
                    hasIncreasedCost = true;
                }
            }
        }    
    }

    public void CreateIntegrationField(Cell _destinationCell)
    {
        //Get the best cost for each cell 
        destinationCell = _destinationCell;
        destinationCell.cost = 0;
        destinationCell.bestCost = 0;

        Queue<Cell> cellsToCheck = new Queue<Cell>();

        cellsToCheck.Enqueue(destinationCell);

        while (cellsToCheck.Count > 0)
        {
            Cell curCell = cellsToCheck.Dequeue();
            List<Cell> curNeighbours = GetNeighbourCells(curCell.gridIndex, GridDirection.cardinalDirections);
            foreach (Cell curNeighbour in curNeighbours)
            {
                if (curNeighbour.cost == byte.MaxValue)
                    continue;
                if (curNeighbour.cost + curCell.bestCost < curNeighbour.bestCost)
                {
                    curNeighbour.bestCost = (ushort)(curNeighbour.cost + curCell.bestCost);
                    cellsToCheck.Enqueue(curNeighbour);
                }
            }
        }
    }

    public void CreateFlowField()
    {
        //Set the best cost and best direction for each cell
        foreach (Cell curCell in grid)
        {
            List<Cell> curNeighbours = GetNeighbourCells(curCell.gridIndex, GridDirection.allDirections);

            int bestCost = curCell.bestCost;
            foreach (Cell curNeighbour in curNeighbours)
            {
                if (curNeighbour.bestCost < bestCost)
                {
                    bestCost = curNeighbour.bestCost;
                    curCell.bestDirection = GridDirection.GetDirectionFromV2I(curNeighbour.gridIndex - curCell.gridIndex);
                }
            }
        }
    }

    private List<Cell> GetNeighbourCells(Vector2Int nodeIndex, List<GridDirection> directions)
    {
        //Get the neighbour of the specified cell
        List<Cell> neighbourCells = new List<Cell>();

        foreach (Vector2Int curDirection in directions)
        {
            Cell newNeighbour = GetCellAtRelativePos(nodeIndex, curDirection);
            if (newNeighbour != null)
                neighbourCells.Add(newNeighbour);
        }
        return neighbourCells;
    }

    private Cell GetCellAtRelativePos(Vector2Int orignPos, Vector2Int relativePos)
    {
        //Get the cell from a relative position on the grid
        Vector2Int finalPos = orignPos + relativePos;
        if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
            return null;
        else
            return grid[finalPos.x, finalPos.y];
    }

    public Cell GetCellFromWorldPos(Vector2 worldPos)
    {
        //Get the cell from the given world position
        float percentX = (worldPos.x - (startPos.x * cellDiameter)) / (gridSize.x * cellDiameter);
        float percentY = (worldPos.y - (startPos.y * cellDiameter)) / (gridSize.y * cellDiameter);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.Clamp(Mathf.FloorToInt(gridSize.x * percentX), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt(gridSize.y * percentY), 0, gridSize.y - 1);
        return grid[x, y];
    }
}
