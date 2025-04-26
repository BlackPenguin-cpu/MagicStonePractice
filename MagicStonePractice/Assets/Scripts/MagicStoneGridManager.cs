using UnityEngine;
using UnityEngine.Serialization;

public class MagicStoneGridManager : Singleton<MagicStoneGridManager>
{
    [SerializeField] private RectTransform gridRectTransform;
    
    public Vector2Int ScreenToCell(Vector2 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gridRectTransform,
            screenPos,
            null,
            out Vector2 localPos
        );

        int col = Mathf.FloorToInt(localPos.x / MagicStoneManager.CELL_SIZE);
        int row = Mathf.FloorToInt(localPos.y / MagicStoneManager.CELL_SIZE);

        return new Vector2Int(col, -row);
    }

    public Vector2 StonePosToGridPos(Vector2 mousePos, Vector2Int size)
    {
        var cellPos = ScreenToCell(mousePos);

        int offsetX = size.x % 2 == 0 ? 1 : 0;
        int offsetY = size.y % 2 == 0 ? 1 : 0;

        float x = cellPos.x - offsetX;
        float y = cellPos.y - offsetY;
        return new Vector2(x, -y);
    }

    public Vector2 CellToWorld(Vector2Int cell)
    {
        var cellSize = MagicStoneManager.CELL_SIZE;
        float x = cell.x * cellSize + cellSize / 2f;
        float y = -(cell.y * cellSize + cellSize / 2f);
        return new Vector2(x, y);
    }
}