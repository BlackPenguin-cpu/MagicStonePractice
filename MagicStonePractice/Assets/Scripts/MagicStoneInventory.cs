using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagicStoneInventory : Singleton<MagicStoneInventory>
{
    private const int invenSize = 4;
    private MagicStoneData[,] inventoryGridState = new MagicStoneData[invenSize, invenSize];
    private List<PlaceStoneInfo> placeStoneInfoList = new List<PlaceStoneInfo>();

    private struct PlaceStoneInfo : IEquatable<PlaceStoneInfo>
    {
        public MagicStoneData magicStoneData;
        public List<Vector2Int> posList;

        public PlaceStoneInfo(MagicStoneData magicStoneData, List<Vector2Int> posList)
        {
            this.magicStoneData = magicStoneData;
            this.posList = posList;
        }

        public bool Equals(PlaceStoneInfo other)
        {
            return Equals(magicStoneData, other.magicStoneData) && Equals(posList, other.posList);
        }

        public override bool Equals(object obj)
        {
            return obj is PlaceStoneInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(magicStoneData, posList);
        }
    }

    public bool CanPlace(MagicStoneData stoneData, Vector2Int pos)
    {
        var topLeftPos = GetTopLeftForStone(pos, stoneData.Size);

        for (int x = 0; x < stoneData.width; x++)
        {
            for (int y = 0; y < stoneData.height; y++)
            {
                if (stoneData.shape[y][x] == false) continue;

                int checkX = topLeftPos.x + x;
                int checkY = topLeftPos.y + y;

                //인벤토리 넓이
                if (checkX < 0 || checkY < 0 || checkX >= invenSize || checkY >= invenSize)
                    return false; // 범위 초과

                if (checkX is 0 or 3 && checkY is 0 or 3)
                    return false; // 테두리 칸 예외처리

                if (inventoryGridState[checkX, checkY])
                    return false; // 이미 다른 아이템이 있음
            }
        }

        return true;
    }

    public bool TryPlace(MagicStoneData stoneData, Vector2Int pos)
    {
        if (!CanPlace(stoneData, pos)) return false;

        var topLeftPos = GetTopLeftForStone(pos, stoneData.Size);

        List<Vector2Int> posList = new List<Vector2Int>();
        for (int x = 0; x < stoneData.width; x++)
        {
            for (int y = 0; y < stoneData.height; y++)
            {
                int checkX = topLeftPos.x + x;
                int checkY = topLeftPos.y + y;

                inventoryGridState[checkX, checkY] = stoneData;
                posList.Add(new Vector2Int(checkX, checkY));
            }
        }

        placeStoneInfoList.Add(new PlaceStoneInfo(stoneData, posList));

        return true;
    }

    Vector2Int GetTopLeftForStone(Vector2Int cellPos, Vector2Int stoneSize)
    {
        int halfX = stoneSize.x / 2;
        int halfY = stoneSize.y / 2;
        Vector2Int baseTopLeft = cellPos - new Vector2Int(halfX, halfY);

        int offsetX = (stoneSize.x % 2 == 0) ? 1 : 0;
        int offsetY = (stoneSize.y % 2 == 0) ? 1 : 0;

        return baseTopLeft + new Vector2Int(offsetX, offsetY);
    }

    public void UnequippedStone(MagicStoneData stoneData)
    {
        var info = placeStoneInfoList.Find((x) => x.magicStoneData == stoneData);

        foreach (var pos in info.posList)
        {
            inventoryGridState[pos.x, pos.y] = null;
        }
        
        placeStoneInfoList.Remove(info);
    }
}