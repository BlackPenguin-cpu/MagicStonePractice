using UnityEngine;

namespace MagicStone
{
    public static class MagicStoneExtension
    {
        public const float CELL_SIZE = 100f;

        public static Vector2 CellPosToStonePos(Vector2 cellPos, Vector2Int size)
        {
            var halfCellSize = CELL_SIZE / 2;

            var xOffset = size.x % 2 == 0 ? halfCellSize : 0;
            var yOffset = size.y % 2 == 0 ? halfCellSize : 0;

            var returnValue = new Vector2(cellPos.x - 2, cellPos.y - 2);
            returnValue *= CELL_SIZE;
            returnValue += new Vector2(halfCellSize + xOffset, halfCellSize + yOffset);

            return new Vector2(returnValue.x, -returnValue.y);
        }

        public static Vector2 CellToWorld(Vector2Int cell)
        {
            float x = cell.x * CELL_SIZE + CELL_SIZE / 2f;
            float y = -(cell.y * CELL_SIZE + CELL_SIZE / 2f);
            return new Vector2(x, y);
        }

        /// <summary>
        /// 만약 블럭이 짝수일경우 가운데 부분이 집어진다면 그리드가 애매해지기 때문에
        /// 따라서 마우스 위치를 왼쪽위 블럭으로 옮긴다
        /// </summary>
        public static Vector2 BlockParsePos(Vector2 mousePos, Vector2Int blockSize)
        {
            int offsetX = blockSize.x % 2 == 0 ? 1 : 0;
            int offsetY = blockSize.y % 2 == 0 ? 1 : 0;
            var realSize = CELL_SIZE * MagicStoneGridManager.Instance.GridScaleMultiply;

            return mousePos + new Vector2(offsetX * realSize / 2f, -offsetY * realSize / 2f);
        }

        public static bool Range(float target, float min, float max)
        {
            return target > min && target < max;
        }
    }
}