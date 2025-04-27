using UnityEngine;
using UnityEngine.Serialization;

public class MagicStoneGridManager : Singleton<MagicStoneGridManager>
{
    [SerializeField] private RectTransform gridRectTransform;
    public const float CELL_SIZE = 100f;
    public float GridScaleMultiply => transform.localScale.x;

    public Vector2Int ScreenToCell(Vector2 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gridRectTransform,
            screenPos,
            null,
            out Vector2 localPos
        );
        int col = Mathf.FloorToInt(localPos.x / CELL_SIZE);
        int row = Mathf.FloorToInt(-localPos.y / CELL_SIZE);

        return new Vector2Int(col, row);
    }

    public Vector2 CellPosToStonePos(Vector2 cellPos, Vector2Int size)
    {
        var xOffset = size.x % 2 == 0 ? 50 : 0;
        var yOffset = size.y % 2 == 0 ? 50 : 0;

        var returnValue = new Vector2(cellPos.x - 2, cellPos.y - 2);
        returnValue *= CELL_SIZE;
        returnValue += new Vector2(50 + xOffset, 50 + yOffset);

        return new Vector2(returnValue.x, -returnValue.y);
    }

    public Vector2 CellToWorld(Vector2Int cell)
    {
        var cellSize = CELL_SIZE;
        float x = cell.x * cellSize + cellSize / 2f;
        float y = -(cell.y * cellSize + cellSize / 2f);
        return new Vector2(x, y);
    }
    
}