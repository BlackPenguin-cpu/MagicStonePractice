using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicStoneDictionary : MonoBehaviour
{
    [SerializeField] private Transform stoneParent;
    [SerializeField] private MagicStone stonePrefab;
    [SerializeField] private List<MagicStoneData> stoneDataList;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    void Start()
    {
        StartCoroutine(DictionaryCreate());
    }

    private IEnumerator DictionaryCreate()
    {
        foreach (var magicStoneData in stoneDataList)
        {
            var obj = Instantiate(stonePrefab, stoneParent);
            obj.magicStoneIcon.sprite = magicStoneData.stoneIcon;
            obj.magicStoneData = magicStoneData;
        }

        yield return null;
        gridLayoutGroup.enabled = false;
    }
}