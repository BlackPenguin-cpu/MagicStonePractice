using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Newtonsoft.Json;

namespace MagicStone
{
    public class MagicStoneInventorySaveManager : Singleton<MagicStoneInventorySaveManager>
    {
        private const string INVENTORY_DATA = "CurrentInventory";
        private const string PRESET_DATA = "PresetNum";

        private Dictionary<string, BlockPosInfo>[] inventorySaveDataArray = new Dictionary<string, BlockPosInfo>[3];

        [ContextMenu("ResetSaveData")]
        public void ResetSaveData()
        {
            PlayerPrefs.DeleteAll();
        }

        private void Awake()
        {
            LoadData();
        }

        public void SavePresetIndex(int preset)
        {
            PlayerPrefs.SetInt(PRESET_DATA, preset);
        }

        public int LoadPresetIndex()
        {
            return PlayerPrefs.GetInt(PRESET_DATA);
        }

        public void SaveCurInventory(Dictionary<string, BlockPosInfo> curInventoryInfo, int presetNum)
        {
            inventorySaveDataArray[presetNum] = new Dictionary<string, BlockPosInfo>(curInventoryInfo);
        }

        [CanBeNull]
        public Dictionary<string, BlockPosInfo> LoadInventory(int presetNum)
        {
            if (inventorySaveDataArray[presetNum] == null || inventorySaveDataArray[presetNum].Count == 0) return null;
            return inventorySaveDataArray[presetNum];
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void LoadData()
        {
            if (!PlayerPrefs.HasKey($"{INVENTORY_DATA}")) return;

            var jsonStr = PlayerPrefs.GetString($"{INVENTORY_DATA}");
            var presetData = JsonConvert.DeserializeObject<Dictionary<string, BlockPosInfo>[]>(jsonStr);

            inventorySaveDataArray = presetData;
        }

        private void SaveData()
        {
            var jsonStr = JsonConvert.SerializeObject(inventorySaveDataArray);
            PlayerPrefs.SetString($"{INVENTORY_DATA}", jsonStr);
            PlayerPrefs.Save();
        }
    }
}