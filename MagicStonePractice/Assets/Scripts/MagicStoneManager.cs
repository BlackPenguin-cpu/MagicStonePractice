using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class MagicStoneManager : Singleton<MagicStoneManager>
{
    [SerializeField] private GameObject magicStoneGridObj;
    [SerializeField] private MagicStoneObj magicStonePrefab;


    private MagicStoneData selectedMagicStoneData;
    private MagicStoneObj selectedMagicStoneObj;

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
        if (!selectedMagicStoneObj) return;

        var pos = MagicStoneGridManager.Instance.ScreenToCell(Input.mousePosition);
        var isPlace = MagicStoneInventory.Instance.TryPlace(selectedMagicStoneData, pos);

        if (isPlace)
        {
            var stonePos = MagicStoneGridManager.Instance.CellPosToStonePos(pos, selectedMagicStoneData.Size);
            selectedMagicStoneObj.transform.localPosition = stonePos;
            selectedMagicStoneObj = null;
        }
        else
        {
            selectedMagicStoneObj.gameObject.SetActive(false);
        }
    }

    private void OnHold()
    {
        if (!selectedMagicStoneObj) return;

        var pos = BlockParsePos(Input.mousePosition, selectedMagicStoneData.Size);
        selectedMagicStoneObj.transform.position = pos;
    }

    private Vector2 BlockParsePos(Vector2 mousePos, Vector2Int blockSize)
    {
        int offsetX = blockSize.x % 2 == 0 ? 1 : 0;
        int offsetY = blockSize.y % 2 == 0 ? 1 : 0;
        var realSize = MagicStoneGridManager.CELL_SIZE * MagicStoneGridManager.Instance.GridScaleMultiply;

        return mousePos + new Vector2(offsetX * realSize / 2f, -offsetY * realSize / 2f);
    }

    public void OnClickStone(MagicStoneObj stoneObj)
    {
        selectedMagicStoneData = stoneObj.magicStoneData;
        selectedMagicStoneObj = stoneObj;
    }

    public void OnClickStone(MagicStoneData stoneData)
    {
        selectedMagicStoneData = stoneData;
        if (!selectedMagicStoneObj)
        {
            selectedMagicStoneObj = Instantiate(magicStonePrefab, magicStoneGridObj.transform);
        }

        selectedMagicStoneObj.magicStoneData = stoneData;
        selectedMagicStoneObj.Image.sprite = stoneData.stoneIcon;
        selectedMagicStoneObj.Image.SetNativeSize();
        selectedMagicStoneObj.gameObject.SetActive(true);
    }
}