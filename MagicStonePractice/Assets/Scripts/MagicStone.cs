using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicStone : MonoBehaviour, IPointerDownHandler
{
    public MagicStoneData magicStoneData;
    public Image magicStoneIcon;

    private readonly Color selectedColor = new Color(1, 1, 1, 0.5f);

    public void OnClick()
    {
        
    }

    public void OnHold()
    {
    }

    public void OnDrop()
    {
        magicStoneIcon.color = Color.white;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        magicStoneIcon.color = selectedColor;
        MagicStoneManager.instance.OnClickStone(magicStoneData);
    }
}