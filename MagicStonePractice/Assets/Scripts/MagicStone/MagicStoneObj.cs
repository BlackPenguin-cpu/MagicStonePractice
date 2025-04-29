using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MagicStone
{
    public class MagicStoneObj : MonoBehaviour, IPointerDownHandler, ICanvasRaycastFilter
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

        /// <summary>
        ///이미지부분이 아닌 설치되있는 셀을 눌러야만 클릭이 인식될 수 있게
        /// </summary>
        public bool IsRaycastLocationValid(Vector2 mousePos, Camera eventCamera)
        {
            var blockSize = magicStoneData.Size;
            var objSize = Image.rectTransform.sizeDelta;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(Image.rectTransform, mousePos,
                null, out var localPoint);

            localPoint = new Vector2(localPoint.x, -localPoint.y);
            localPoint += objSize / 2;
            var cellSize = MagicStoneGridManager.CELL_SIZE;
            for (int i = 0; i < blockSize.x; i++)
            {
                for (int j = 0; j < blockSize.y; j++)
                {
                    if (magicStoneData.shape[j][i]) continue;
                    if (Range(localPoint.x, cellSize * i, cellSize * (i + 1)) &&
                        Range(localPoint.y, cellSize * j, cellSize * (j + 1)))
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
}