using UnityEngine;

namespace MagicStone
{
    public class MagicStoneGridManager : Singleton<MagicStoneGridManager>
    {
        [SerializeField] private RectTransform gridRectTransform;
        public float GridScaleMultiply => transform.localScale.x;

        public Vector2Int ScreenToCell(Vector3 screenPos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                gridRectTransform,
                screenPos,
                null,
                out var localPos
            );
            int col = Mathf.FloorToInt(localPos.x / MagicStoneExtension.CELL_SIZE);
            int row = Mathf.FloorToInt(-localPos.y / MagicStoneExtension.CELL_SIZE);

            return new Vector2Int(col, row);
        }


    
    }
}