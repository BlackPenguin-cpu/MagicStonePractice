using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class MagicStoneManager : Singleton<MagicStoneManager>
{
    [SerializeField] private GameObject magicStoneGridObj;
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
        var pos = MagicStoneGridManager.Instance.ScreenToCell(Input.mousePosition);
        var value = MagicStoneInventory.Instance.CanPlace(selectedMagicStoneData, pos);

        Debug.Log(pos);
        Debug.Log(value);
    }

    private void OnHold()
    {
        if (!selectedMagicStoneObj.IsActive()) return;

        var pos = BlockParsePos(Input.mousePosition, selectedMagicStoneData.Size);
        selectedMagicStoneObj.rectTransform.position = pos;
    }

    private Vector2 BlockParsePos(Vector2 mousePos, Vector2Int blockSize)
    {
        int offsetX = blockSize.x % 2 == 0 ? 1 : 0;
        int offsetY = blockSize.y % 2 == 0 ? 1 : 0;
        var realSize = CELL_SIZE * GridScale.x;

        return mousePos + new Vector2(offsetX * realSize / 2f, -offsetY * realSize / 2f);
    }

    public void OnClickStone(MagicStoneData stoneData)
    {
        selectedMagicStoneData = stoneData;
        if (selectedMagicStoneObj == null)
        {
            selectedMagicStoneObj = Instantiate(magicStonePrefab, magicStoneGridObj.transform);
        }

        selectedMagicStoneObj.sprite = stoneData.stoneIcon;
        selectedMagicStoneObj.SetNativeSize();
        selectedMagicStoneObj.gameObject.SetActive(true);
    }
}