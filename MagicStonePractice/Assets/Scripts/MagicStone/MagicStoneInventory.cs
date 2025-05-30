using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MagicStone
{
    public class MagicStoneInventory : Singleton<MagicStoneInventory>
    {
        public int presetNum = 0;
        [SerializeField] private TMP_Dropdown presetDropdown;
        private const int INVENTORY_SIZE = 4;

        public List<MagicStoneObj> nowPlaceObjList = new List<MagicStoneObj>();
        private MagicStoneData[,] inventoryGridState = new MagicStoneData[INVENTORY_SIZE, INVENTORY_SIZE];

        public Dictionary<string, BlockPosInfo> placeStoneInfoDictionary =
            new Dictionary<string, BlockPosInfo>();

        private bool IsPlaceable(MagicStoneData stoneData, Vector2Int pos)
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
                    if (checkX < 0 || checkY < 0 || checkX >= INVENTORY_SIZE || checkY >= INVENTORY_SIZE)
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
            if (!IsPlaceable(stoneData, pos)) return false;

            var topLeftPos = GetTopLeftForStone(pos, stoneData.Size);

            var posList = new List<Vector2Int>();
            for (int x = 0; x < stoneData.width; x++)
            {
                for (int y = 0; y < stoneData.height; y++)
                {
                    if (stoneData.shape[y][x] == false) continue;

                    int checkX = topLeftPos.x + x;
                    int checkY = topLeftPos.y + y;

                    inventoryGridState[checkX, checkY] = stoneData;
                    posList.Add(new Vector2Int(checkX, checkY));
                }
            }

            placeStoneInfoDictionary.Add(stoneData.stoneName, new BlockPosInfo(pos, posList));
            SaveCurInventory();

            return true;
        }

        private Vector2Int GetTopLeftForStone(Vector2Int cellPos, Vector2Int stoneSize)
        {
            int halfX = stoneSize.x / 2;
            int halfY = stoneSize.y / 2;
            Vector2Int baseTopLeft = cellPos - new Vector2Int(halfX, halfY);

            int offsetX = (stoneSize.x % 2 == 0) ? 1 : 0;
            int offsetY = (stoneSize.y % 2 == 0) ? 1 : 0;

            return baseTopLeft + new Vector2Int(offsetX, offsetY);
        }

        public void SaveCurInventory()
        {
            MagicStoneInventorySaveManager.Instance.SaveCurInventory(placeStoneInfoDictionary, presetNum);
        }

        public bool IsAlreadyPlace(MagicStoneData stoneData)
        {
            return placeStoneInfoDictionary.ContainsKey(stoneData.stoneName);
        }

        public void UnequippedStone(MagicStoneObj stoneObj)
        {
            var stoneData = stoneObj.magicStoneData;
            if (!placeStoneInfoDictionary.TryGetValue(stoneData.stoneName, out var posList)) return;

            if (posList.gridPosList.Count <= 0) return;

            foreach (var pos in posList.gridPosList)
            {
                inventoryGridState[pos.x, pos.y] = null;
            }

            MagicStoneManager.Instance.magicStoneObjPool.ReturnToPool(stoneObj);
            nowPlaceObjList.Remove(stoneObj);
            placeStoneInfoDictionary.Remove(stoneData.stoneName);
        }

        private void Start()
        {
            presetNum = MagicStoneInventorySaveManager.Instance.LoadPresetIndex();
            presetDropdown.value = presetNum;
            MagicStoneManager.Instance.DescShow();
        }

        private void OnApplicationQuit()
        {
            MagicStoneInventorySaveManager.Instance.SavePresetIndex(presetNum);
        }

        private void LoadInventory(int preset)
        {
            var inventoryData = MagicStoneInventorySaveManager.Instance.LoadInventory(preset);

            if (inventoryData != null)
                NowStateApply(inventoryData);

            MagicStoneDictionary.Instance.SelectUIUpdate();
            MagicStoneManager.Instance.DescShow();
        }

        private void NowStateApply(Dictionary<string, BlockPosInfo> stoneInfoDictionary)
        {
            foreach (var (key, blockPosInfo) in stoneInfoDictionary)
            {
                var data = MagicStoneDictionary.Instance.StoneDataDictionary[key];
                TryPlace(data, blockPosInfo.realPos);
                MagicStoneManager.Instance.MagicStonePlaceOnInventory(data, blockPosInfo.realPos);
            }
        }

        public void PresetChange(int index)
        {
            ResetInventory();
            presetNum = index;
            LoadInventory(presetNum);
        }

        private void ResetInventory()
        {
            foreach (var obj in nowPlaceObjList)
            {
                MagicStoneManager.Instance.magicStoneObjPool.ReturnToPool(obj);
            }

            inventoryGridState = new MagicStoneData[INVENTORY_SIZE, INVENTORY_SIZE];
            nowPlaceObjList.Clear();
            placeStoneInfoDictionary.Clear();
        }
    }

    [System.Serializable]
    public struct BlockPosInfo
    {
        public Vector2Int realPos;
        public List<Vector2Int> gridPosList;

        public BlockPosInfo(Vector2Int realPos, List<Vector2Int> gridPosList)
        {
            this.realPos = realPos;
            this.gridPosList = gridPosList;
        }
    }
}