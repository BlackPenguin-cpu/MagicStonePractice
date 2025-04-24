using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class MagicStoneManager : Singleton<MagicStoneManager>
{
    [SerializeField] private GameObject magicStoneInventory;
    [SerializeField] private Image magicStonePrefab;

    private MagicStoneData selectedMagicStoneData;
    private Image selectedMagicStoneObj;

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

        if (Input.GetMouseButtonUp(0))
        {
            OnMouseButtonUp();
        }
    }

    private void OnMouseButtonUp()
    {
        selectedMagicStoneObj.gameObject.SetActive(false);
        var pos = Input.mousePosition;
        
        Debug.Log(MagicStoneGridManager.Instance.ScreenToCell(pos));
    }

    private void OnHold()
    {
        if (selectedMagicStoneObj.IsActive())
        {
            selectedMagicStoneObj.rectTransform.position = Input.mousePosition;
        }
    }

    public void OnClickStone(MagicStoneData stoneData)
    {
        selectedMagicStoneData = stoneData;
        if (selectedMagicStoneObj == null)
        {
            selectedMagicStoneObj = Instantiate(magicStonePrefab, magicStoneInventory.transform);
        }
        
        selectedMagicStoneObj.sprite = stoneData.stoneIcon;
        selectedMagicStoneObj.SetNativeSize();
        selectedMagicStoneObj.gameObject.SetActive(true);
    }
}