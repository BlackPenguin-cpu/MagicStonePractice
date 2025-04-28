using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicStoneSelectUI : MonoBehaviour, IPointerDownHandler
{
    public MagicStoneData magicStoneData;
    public Image magicStoneIcon;

    [SerializeField] private Image background;
    private bool isDisable = false;

    public void OnStateUpdate()
    {
        isDisable = MagicStoneInventory.Instance.IsAlreadyPlace(magicStoneData) ||
                         MagicStoneManager.Instance.IsStoneAlreadySelected(magicStoneData);

        if (isDisable)
        {
            magicStoneIcon.color = MagicStoneManager.invisibleColor;
            background.color = MagicStoneManager.invisibleColor;
        }
        else
        {
            magicStoneIcon.color = Color.white;
            background.color = Color.white;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isDisable) return;

        MagicStoneManager.Instance.OnClickStone(magicStoneData);
    }
}