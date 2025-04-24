using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicStone : MonoBehaviour, IPointerDownHandler
{
    public MagicStoneData magicStoneData;
    public Image magicStoneIcon;

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
        MagicStoneManager.Instance.OnClickStone(magicStoneData);
    }
}