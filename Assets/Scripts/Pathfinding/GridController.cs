using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    EnemyController ec;
    Tilemap tilemap;
    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Vector2Int offset;

    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Awake()
    {
        ec = GetComponent<EnemyController>();
        tilemap = ec.level.gameObject.transform.GetChild(1).GetComponent<Tilemap>();
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = tilemap.HasTile(tilemap.WorldToCell(worldPoint)) || ec.possibleSpawns.Contains(worldPoint);

                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
        foreach (Node n in grid)
        {
            grid[n.gridX, n.gridY].neighbours = GetNeighbours(grid[n.gridX, n.gridY]);
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY && grid[checkX, checkY].walkable)
                {
                    if (ec.level.firstTile.Any(p => p == grid[checkX, checkY].worldPos) && ec.level.firstTileDir.Any(p => p == new Vector2Int(x,y)))
                    {
                        neighbours.Add(grid[checkX, checkY]);
                        continue;
                    }
                    if (ec.level.entrance.Any(p => p == grid[checkX, checkY].worldPos) && ec.level.entranceDir.Any(p => p == new Vector2Int(x, y)))
                    {
                        neighbours.Add(grid[checkX, checkY]);
                        continue;
                    }
                    if ((tilemap.HasTile(tilemap.WorldToCell(grid[checkX, checkY].worldPos)) && tilemap.HasTile(tilemap.WorldToCell(node.worldPos))) || (ec.possibleSpawns.Contains(node.worldPos) && ec.possibleSpawns.Contains(grid[checkX, checkY].worldPos)))
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector2 worldPosition, bool target)
    {
        System.Random prng = new System.Random();
        if (target)
        {
            int posX = prng.Next(0, 2);
            int posY = prng.Next(0, 2);
            switch (posX, posY)
            {
                case (0, 1):
                    worldPosition.x -= 0.5f;
                    worldPosition.y += 0.5f;
                    break;
                case (1, 1):
                    worldPosition.x += 0.5f;
                    worldPosition.y += 0.5f;
                    break;
                case (0, 0):
                    worldPosition.x -= 0.5f;
                    worldPosition.y -= 0.5f;
                    break;
                case (1, 0):
                    worldPosition.x += 0.5f;
                    worldPosition.y -= 0.5f;
                    break;
            }
        }
        float percentX = ((worldPosition.x) + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = ((worldPosition.y) + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
     }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));

        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = n.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
