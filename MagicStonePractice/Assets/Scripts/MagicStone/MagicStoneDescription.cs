using System.Text;
using TMPro;
using UnityEngine;

namespace MagicStone
{
    public class MagicStoneDescription : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI descText;
        
        private const string NONE_TEXT = "아무것도 없어용 ㅠㅠㅠ";

        public void ShowDescription()
        {
            if (MagicStoneInventory.Instance.placeStoneInfoDictionary.Count <= 0)
            {
                descText.text = NONE_TEXT;
                return;
            }

            var sb = new StringBuilder();
            foreach (var magicStoneName in MagicStoneInventory.Instance.placeStoneInfoDictionary.Keys)
            {
                if (!MagicStoneDictionary.Instance.StoneDataDictionary.TryGetValue(magicStoneName, out var stoneData)) 
                    continue;
                
                sb.AppendLine(stoneData.description);
            }

            descText.text = sb.ToString();
        }
    }
}