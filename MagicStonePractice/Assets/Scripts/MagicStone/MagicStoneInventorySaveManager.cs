using System.Collections.Generic;
using UnityEngine;

namespace MagicStone
{
    public class MagicStoneInventorySaveManager : Singleton<MagicStoneInventorySaveManager>
    {
        private const string SAVE_NAME = "CurrentInventory";

        [ContextMenu("ResetSaveData")]
        public void ResetSaveData()
        {
            PlayerPrefs.DeleteAll();
        }

        public void SavePresetIndex(int preset)
        {
            PlayerPrefs.SetInt(SAVE_NAME, preset);
        }
        public int LoadPresetIndex()
        {
            return PlayerPrefs.GetInt(SAVE_NAME);
        }
    
        public void SaveCurInventory(InventoryInfo curInventoryInfo, int presetNum)
        {
            var serializableData =
                new SerializableDictionary<string, BlockPosInfo>(curInventoryInfo.placeStoneInfoDictionary);
        
            var jsonStr = JsonUtility.ToJson(serializableData);
            PlayerPrefs.SetString($"{SAVE_NAME}{presetNum}", jsonStr);
        }

        public InventoryInfo? LoadInventory(int presetNum)
        {
            if (!PlayerPrefs.HasKey($"{SAVE_NAME}{presetNum}")) return null;

            var jsonStr = PlayerPrefs.GetString($"{SAVE_NAME}{presetNum}");

            var returnValue = JsonUtility.FromJson<SerializableDictionary<string, BlockPosInfo>>(jsonStr);
            return new InventoryInfo(returnValue.ToDictionary());
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.Save();
        }
    }

    [System.Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        public List<KeyValuePairSerializable<TKey, TValue>> list = new List<KeyValuePairSerializable<TKey, TValue>>();

        public SerializableDictionary(Dictionary<TKey, TValue> dictionary)
        {
            foreach (var kv in dictionary)
            {
                list.Add(new KeyValuePairSerializable<TKey, TValue> { key = kv.Key, value = kv.Value });
            }
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            var dict = new Dictionary<TKey, TValue>();
            foreach (var kv in list)
            {
                dict[kv.key] = kv.value;
            }

            return dict;
        }

        public void FromDictionary(Dictionary<TKey, TValue> dictionary)
        {
            list.Clear();
            foreach (var kv in dictionary)
            {
                list.Add(new KeyValuePairSerializable<TKey, TValue> { key = kv.Key, value = kv.Value });
            }
        }
    }

    [System.Serializable]
    public struct KeyValuePairSerializable<TKey, TValue>
    {
        public TKey key;
        public TValue value;
    }
}