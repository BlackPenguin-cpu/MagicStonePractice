using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
            if (_stoneDataDictionary == null)
            {
                _stoneDataDictionary = new Dictionary<string, MagicStoneData>();
                foreach (var data in stoneDataList)
                {
                    _stoneDataDictionary.Add(data.stoneName, data);
                }
            }

            return _stoneDataDictionary;
        }
        private set => _stoneDataDictionary = value;
    }

    private List<MagicStoneSelectUI> stoneSelectUIList = new List<MagicStoneSelectUI>();

    void Start()
    {
        StartCoroutine(DictionaryCreate());
    }

    private IEnumerator DictionaryCreate()
    {
        foreach (var magicStoneData in stoneDataList)
        {
            var selectUI = Instantiate(stoneSelectUIPrefab, stoneParent);
            selectUI.magicStoneIcon.sprite = magicStoneData.stoneIcon;
            selectUI.magicStoneData = magicStoneData;

            stoneSelectUIList.Add(selectUI);
        }

        yield return null;
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