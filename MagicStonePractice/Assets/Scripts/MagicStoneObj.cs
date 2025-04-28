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
        MagicStoneInventory.Instance.UnequippedStone(magicStoneData);
        MagicStoneManager.Instance.OnClickStone(this);
    }
}