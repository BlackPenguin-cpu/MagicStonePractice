using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MagicStone
{
    public class MagicStoneDictionary : Singleton<MagicStoneDictionary>
    {
        [SerializeField] private Transform stoneParent;
        [SerializeField] private MagicStoneSelectUI stoneSelectUIPrefab;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private List<MagicStoneData> stoneDataList;

        private Dictionary<string, MagicStoneData> _stoneDataDictionary;

        public Dictionary<string, MagicStoneData> StoneDataDictionary
        {
            get
            {
                if (_stoneDataDictionary != null) return _stoneDataDictionary;

                _stoneDataDictionary = new Dictionary<string, MagicStoneData>();
                foreach (var data in stoneDataList)
                {
                    _stoneDataDictionary.Add(data.stoneName, data);
                }

                return _stoneDataDictionary;
            }
            private set => _stoneDataDictionary = value;
        }

        private List<MagicStoneSelectUI> stoneSelectUIList = new List<MagicStoneSelectUI>();

        private void Start()
        {
            DictionaryCreate();
        }

        private void DictionaryCreate()
        {
            foreach (var magicStoneData in stoneDataList)
            {
                var selectUI = Instantiate(stoneSelectUIPrefab, stoneParent);
                selectUI.magicStoneIcon.sprite = magicStoneData.stoneIcon;
                selectUI.magicStoneData = magicStoneData;

                stoneSelectUIList.Add(selectUI);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(gridLayoutGroup.transform as RectTransform);
            gridLayoutGroup.enabled = false;
            SelectUIUpdate();
        }

        public void SelectUIUpdate()
        {
            foreach (var selectUI in stoneSelectUIList)
            {
                selectUI.OnStateUpdate();
            }
        }
    }
}