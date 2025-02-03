using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public const int GRID_SIZE = 16;
    private Dictionary<Vector2Int, int> grid = new Dictionary<Vector2Int, int>();
    public bool showGrid = true;
    public Color gridColor = Color.white;
    private List<LineRenderer> gridLines = new List<LineRenderer>();

    void Start()
    {
        if (showGrid)
        {
            CreateGrid();
        }
    }

    void CreateGrid()
    {
        float halfSize = GRID_SIZE / 2f;
        
        // Draw horizontal lines
        for (int y = 0; y <= GRID_SIZE; y++)
        {
            CreateLine(
                new Vector3(-halfSize, y - halfSize, 0),
                new Vector3(halfSize, y - halfSize, 0)
            );
        }

        // Draw vertical lines
        for (int x = 0; x <= GRID_SIZE; x++)
        {
            CreateLine(
                new Vector3(x - halfSize, -halfSize, 0),
                new Vector3(x - halfSize, halfSize, 0)
            );
        }
    }

    void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.parent = transform;
        LineRenderer line = lineObj.AddComponent<LineRenderer>();
        
        // Configure the line
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = gridColor;
        line.endColor = gridColor;
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        line.positionCount = 2;
        line.useWorldSpace = true;
        
        // Set line positions
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        
        gridLines.Add(line);
    }

    void OnDrawGizmos()
    {
        if (showGrid)
        {
            Gizmos.color = gridColor;
            float halfSize = GRID_SIZE / 2f;
            
            // Draw horizontal lines
            for (int y = 0; y <= GRID_SIZE; y++)
            {
                Gizmos.DrawLine(
                    new Vector3(-halfSize, y - halfSize, 0),
                    new Vector3(halfSize, y - halfSize, 0)
                );
            }
            
            // Draw vertical lines
            for (int x = 0; x <= GRID_SIZE; x++)
            {
                Gizmos.DrawLine(
                    new Vector3(x - halfSize, -halfSize, 0),
                    new Vector3(x - halfSize, halfSize, 0)
                );
            }
        }
    }

    public bool IsCellOccupied(Vector2Int position)
    {
        return grid.ContainsKey(position);
    }

    public void OccupyCell(Vector2Int position, int playerId)
    {
        if (!grid.ContainsKey(position))
            grid[position] = playerId;
    }

    // Helper method to convert world position to grid position
    public static Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        float halfSize = GRID_SIZE / 2f;
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x + halfSize),
            Mathf.RoundToInt(worldPosition.y + halfSize)
        );
    }

    // Helper method to convert grid position to world position
    public static Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        float halfSize = GRID_SIZE / 2f;
        return new Vector3(
            gridPosition.x - halfSize,
            gridPosition.y - halfSize,
            0
        );
    }
}
