using UnityEngine;

namespace MagicStone
{
    public class MagicStoneManager : Singleton<MagicStoneManager>
    {
        [SerializeField, HideInInspector] private Transform magicStoneParent;
        [SerializeField, HideInInspector] private MagicStoneObj magicStonePrefab;
        [SerializeField, HideInInspector] private MagicStoneDescription descText;

        [SerializeField] private MagicStoneData selectedMagicStoneData;
        [SerializeField] private MagicStoneObj selectedMagicStoneObj;

        public ObjectPool<MagicStoneObj> magicStoneObjPool;

        public static readonly Color invisibleColor = new Color(1f, 1f, 1f, 0.4f);

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
            if (IsSelectMagicStoneAvailable()) return;

            var pos = MagicStoneGridManager.Instance.ScreenToCell(Input.mousePosition);
            var isPlace = MagicStoneInventory.Instance.TryPlace(selectedMagicStoneData, pos);

            if (isPlace)
            {
                MagicStonePlaceOnInventory(selectedMagicStoneObj, pos);
                selectedMagicStoneObj = null;
            }
            else
            {
                magicStoneObjPool.ReturnToPool(selectedMagicStoneObj);
            }

            MagicStoneInventory.Instance.SaveInventory();
            MagicStoneDictionary.Instance.SelectUIUpdate();
            DescShow();
        }

        private bool IsSelectMagicStoneAvailable()
        {
            return !selectedMagicStoneObj || !selectedMagicStoneObj.gameObject.activeSelf;
        }

        public void MagicStonePlaceOnInventory(MagicStoneData data, Vector2 cellPos)
        {
            var obj = CreateBlock();
            obj.Init(data);
            MagicStonePlaceOnInventory(obj, cellPos);
        }

        private void MagicStonePlaceOnInventory(MagicStoneObj obj, Vector2 pos)
        {
            var stonePos = MagicStoneGridManager.Instance.CellPosToStonePos(pos, obj.magicStoneData.Size);

            obj.transform.localPosition = stonePos;
            obj.Image.color = Color.white;
            MagicStoneInventory.Instance.nowPlaceObjList.Add(obj);
        }

        private void OnHold()
        {
            if (IsSelectMagicStoneAvailable()) return;

            var pos = BlockParsePos(Input.mousePosition, selectedMagicStoneData.Size);
            selectedMagicStoneObj.transform.position = pos;
        }

        /// <summary>
        /// 만약 블럭이 짝수일경우 가운데 부분이 집어진다면 그리드가 애매해지기 때문에
        /// 따라서 마우스 위치를 왼쪽위 블럭으로 옮긴다
        /// </summary>
        private Vector2 BlockParsePos(Vector2 mousePos, Vector2Int blockSize)
        {
            int offsetX = blockSize.x % 2 == 0 ? 1 : 0;
            int offsetY = blockSize.y % 2 == 0 ? 1 : 0;
            var realSize = MagicStoneGridManager.CELL_SIZE * MagicStoneGridManager.Instance.GridScaleMultiply;

            return mousePos + new Vector2(offsetX * realSize / 2f, -offsetY * realSize / 2f);
        }

        public void OnClickStone(MagicStoneObj stoneObj)
        {
            selectedMagicStoneData = stoneObj.magicStoneData;
            selectedMagicStoneObj = stoneObj;
            SelectMagicStoneInit();
        }


        public void OnClickStone(MagicStoneData stoneData)
        {
            selectedMagicStoneData = stoneData;
            selectedMagicStoneObj ??= CreateBlock();

            selectedMagicStoneObj.Init(stoneData);

            SelectMagicStoneInit();
        }
        private MagicStoneObj CreateBlock()
        {
            magicStoneObjPool ??= new ObjectPool<MagicStoneObj>(magicStonePrefab, 4, magicStoneParent);
            return magicStoneObjPool.Get();
        }

        public void DescShow()
        {
            descText.ShowDescription();
        }

        private void SelectMagicStoneInit()
        {
            selectedMagicStoneObj.Image.color = invisibleColor;
            selectedMagicStoneObj.transform.SetAsLastSibling();
            selectedMagicStoneObj.gameObject.SetActive(true);
        }

        public bool IsStoneAlreadySelected(MagicStoneData data)
        {
            if (!selectedMagicStoneObj || !selectedMagicStoneObj.gameObject.activeSelf) return false;

            return selectedMagicStoneData == data;
        }
    }
}