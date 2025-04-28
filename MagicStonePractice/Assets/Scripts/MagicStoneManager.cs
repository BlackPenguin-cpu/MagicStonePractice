using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class MagicStoneManager : Singleton<MagicStoneManager>
{
    [SerializeField, HideInInspector] private GameObject magicStoneGridObj;
    [SerializeField, HideInInspector] private MagicStoneObj magicStonePrefab;

    [SerializeField] private MagicStoneData selectedMagicStoneData;
    [SerializeField] private MagicStoneObj selectedMagicStoneObj;

    public static readonly Color invisibleColor = new Color(1f, 1f, 1f, 0.4f);

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
        if (!selectedMagicStoneObj || !selectedMagicStoneObj.gameObject.activeSelf) return;

        var pos = MagicStoneGridManager.Instance.ScreenToCell(Input.mousePosition);
        var isPlace = MagicStoneInventory.Instance.TryPlace(selectedMagicStoneData, pos);

        if (isPlace)
        {
            MagicStonePlaceOnInventory(selectedMagicStoneObj, pos);
            selectedMagicStoneObj = null;
        }
        else
        {
            selectedMagicStoneObj.gameObject.SetActive(false);
        }

        MagicStoneDictionary.Instance.SelectUIUpdate();
    }

    public void MagicStonePlaceOnInventory(MagicStoneData data, Vector2 cellPos)
    {
        var obj = Instantiate(magicStonePrefab, magicStoneGridObj.transform);
        obj.Init(data);
        MagicStonePlaceOnInventory(obj,cellPos);
    }

    private void MagicStonePlaceOnInventory(MagicStoneObj obj, Vector2 pos)
    {
        var stonePos = MagicStoneGridManager.Instance.CellPosToStonePos(pos, obj.magicStoneData.Size);

        obj.transform.localPosition = stonePos;
        obj.Image.color = Color.white;
        MagicStoneInventory.Instance.nowPlaceObjList.Add(obj);
    }

    private void OnHold()
    {
        if (!selectedMagicStoneObj || !selectedMagicStoneObj.gameObject.activeSelf) return;


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

        selectedMagicStoneObj.Init(stoneData);
        selectedMagicStoneObj.Image.color = invisibleColor;
        selectedMagicStoneObj.gameObject.SetActive(true);
    }

    public bool IsStoneAlreadySelected(MagicStoneData data)
    {
        if (!selectedMagicStoneObj || !selectedMagicStoneObj.gameObject.activeSelf) return false;

        return selectedMagicStoneData == data;
    }
}