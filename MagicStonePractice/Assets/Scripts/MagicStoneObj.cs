using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicStoneObj : MonoBehaviour, IPointerDownHandler
{
    public MagicStoneData magicStoneData;

    private Image _image;
    public Image Image => _image ??= GetComponent<Image>();

    public void Init(MagicStoneData data)
    {
        magicStoneData = data;
        Image.sprite = data.stoneIcon;
        Image.SetNativeSize();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!IsClickStoneCell(eventData.position)) return;

        MagicStoneInventory.Instance.UnequippedStone(magicStoneData);
        MagicStoneManager.Instance.OnClickStone(this);
    }

    //이미지부분이 아닌 설치되있는 셀을 눌러야만 클릭이 인식될 수 있게
    private bool IsClickStoneCell(Vector2 mousePos)
    {
        var blockSize = magicStoneData.Size;
        var objSize = Image.rectTransform.sizeDelta;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(Image.rectTransform, mousePos,
            null, out var localPoint);

        localPoint += objSize / 2;
        var xOffset = objSize.x / blockSize.x;
        var yOffset = objSize.y / blockSize.y;
        for (int i = 0; i < blockSize.x; i++)
        {
            for (int j = 0; j < blockSize.y; j++)
            {
                if (magicStoneData.shape[j][i]) continue;
                if (Range(localPoint.x, xOffset * i, xOffset * (i + 1)) &&
                    Range(localPoint.y, yOffset * j, yOffset * (j + 1)))
                {
                    return false;
                }
            }
        }

        return true;
    }
    
    private bool Range(float target, float min, float max)
    {
        return target > min && target < max;
    }
}