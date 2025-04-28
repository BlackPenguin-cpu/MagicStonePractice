using System.Text;
using TMPro;
using UnityEngine;

public class MagicStoneDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descText;
    private const string NoneText = "아무것도 없어용 ㅠㅠㅠ";

    public void ShowDescription()
    {
        StringBuilder sb = new StringBuilder();
        var keyCollection = MagicStoneInventory.Instance.placeStoneInfoDictionary.Keys;
        if (keyCollection.Count <= 0)
        {
            descText.text = NoneText;
            return;
        }

        foreach (var obj in keyCollection)
        {
            MagicStoneDictionary.Instance.StoneDataDictionary.TryGetValue(obj, out var stoneData);
            if (!stoneData) continue;
            sb.AppendLine(stoneData.description);
        }

        descText.text = sb.ToString();
    }
}