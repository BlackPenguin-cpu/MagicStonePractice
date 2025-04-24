using UnityEngine;
using UnityEngine.Serialization;

public class MagicStoneGridManager : Singleton<MagicStoneGridManager>
{
    private readonly float cellSize = 100f;
    [SerializeField] private RectTransform gridRectTransform;

    public Vector2Int ScreenToCell(Vector2 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gridRectTransform,
            screenPos,
            null, 
            out Vector2 localPos
        );

        int col = Mathf.FloorToInt(localPos.x / cellSize);
        int row = Mathf.FloorToInt(localPos.y / cellSize);

        return new Vector2Int(col, row);
    }


    public Vector2 CellToWorld(Vector2Int cell)
    {
        float x = cell.x * cellSize + cellSize / 2f;
        float y = -(cell.y * cellSize + cellSize / 2f);
        return new Vector2(x, y);
    }
}