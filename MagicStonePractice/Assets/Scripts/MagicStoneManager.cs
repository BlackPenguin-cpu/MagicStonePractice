using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MagicStoneManager : MonoBehaviour
{
    public static MagicStoneManager instance;

    [SerializeField] private GameObject magicStoneInventory;
    [SerializeField] private Image magicStonePrefab;
    
    private MagicStoneData selectedMagicStoneData;
    private Image selectedMagicStoneObj;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        InputUpdateFunc();
    }

    private void InputUpdateFunc()
    {
        if (Input.GetMouseButton(0))
        {
            OnHold();
        }
    }

    private void OnHold()
    {
        if (selectedMagicStoneObj)
        {
            selectedMagicStoneObj.rectTransform.position = Input.mousePosition;
        }
    }

    public void OnClickStone(MagicStoneData stoneData)
    {
        selectedMagicStoneData = stoneData;
        selectedMagicStoneObj = Instantiate(magicStonePrefab,magicStoneInventory.transform);
        selectedMagicStoneObj.sprite = stoneData.stoneIcon;
    }
}