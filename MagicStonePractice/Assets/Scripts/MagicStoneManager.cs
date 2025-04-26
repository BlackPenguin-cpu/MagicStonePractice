using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class MagicStoneManager : Singleton<MagicStoneManager>
{
    [SerializeField] private GameObject magicStoneInventory;
    [SerializeField] private Image magicStonePrefab;

    public const float CELL_SIZE = 100f;

    public Vector3 GridScale => MagicStoneGridManager.Instance.transform.localScale;

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
        var size = new Vector2Int(selectedMagicStoneData.width, selectedMagicStoneData.height);
        var pos = MagicStoneGridManager.Instance.StonePosToGridPos(Input.mousePosition, size);
    }

    private void OnHold()
    {
        if (selectedMagicStoneObj.IsActive())
        {
            var pos = BlockParsePos(Input.mousePosition, selectedMagicStoneData.size);
            selectedMagicStoneObj.rectTransform.position = pos;
        }
    }

    private Vector2 BlockParsePos(Vector2 mousePos, Vector2Int blockSize)
    {
        var uiPos = mousePos;

        int offsetX = blockSize.x % 2 == 0 ? 1 : 0;
        int offsetY = blockSize.y % 2 == 0 ? 1 : 0;
        var realSize = CELL_SIZE * GridScale.x;

        return uiPos + new Vector2(offsetX * realSize / 2f, offsetY * realSize / 2f);
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