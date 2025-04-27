using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicStoneObj : MonoBehaviour, IPointerDownHandler
{
    public MagicStoneData magicStoneData;

    private Image _image;

    public Image Image
    {
        get
        {
            if (!_image)
            {
                _image = GetComponent<Image>();
            }

            return _image;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MagicStoneInventory.Instance.UnequippedStone(magicStoneData);
        MagicStoneManager.Instance.OnClickStone(this);
    }
}