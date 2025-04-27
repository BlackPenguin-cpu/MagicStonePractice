using UnityEngine;

public class MagicStoneInventory : Singleton<MagicStoneInventory>
{
    private const int invenSize = 4;
    public MagicStoneData[,] inventoryGridState = new MagicStoneData[invenSize, invenSize];

    
    public bool CanPlace(MagicStoneData stoneData, Vector2Int pos)
    {
        for (int x = 0; x < stoneData.width; x++)
        {
            for (int y = 0; y < stoneData.height; y++)
            {
                if(stoneData.shape[x][y] == false) continue;
                
                int checkX = pos.x + x;
                int checkY = pos.y + y;

                //인벤토리 넓이
                if (checkX < 0 || checkY < 0 || checkX >= invenSize || checkY >= invenSize)
                    return false; // 범위 초과

                if (inventoryGridState[checkX, checkY])
                    return false; // 이미 다른 아이템이 있음
            }
        }
        return true;
        
    }
}