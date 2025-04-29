using UnityEngine;

namespace MagicStone
{
    public class MagicStoneManager : Singleton<MagicStoneManager>
    {
        [SerializeField, HideInInspector] private Transform magicStoneParent;
        [SerializeField, HideInInspector] private MagicStoneObj magicStonePrefab;
        [SerializeField, HideInInspector] private MagicStoneDescription descText;

        [SerializeField] private MagicStoneObj selectedMagicStoneObj;
        private MagicStoneData SelectedMagicStoneData => selectedMagicStoneObj.magicStoneData;

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
                OnDrop();
            }
        }

        private void OnDrop()
        {
            if (IsSelectMagicStoneAvailable()) return;

            var pos = MagicStoneGridManager.Instance.ScreenToCell(Input.mousePosition);
            var isPlace = MagicStoneInventory.Instance.TryPlace(SelectedMagicStoneData, pos);

            if (isPlace)
            {
                MagicStonePlaceOnInventory(selectedMagicStoneObj, pos);
            }
            else
            {
                magicStoneObjPool.ReturnToPool(selectedMagicStoneObj);
            }

            selectedMagicStoneObj = null;

            MagicStoneInventory.Instance.SaveCurInventory();
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
            obj.MagicStoneObjReset(data);
            MagicStonePlaceOnInventory(obj, cellPos);
        }

        private void MagicStonePlaceOnInventory(MagicStoneObj obj, Vector2 pos)
        {
            var stonePos = MagicStoneExtension.CellPosToStonePos(pos, obj.magicStoneData.Size);

            obj.transform.localPosition = stonePos;
            obj.Image.color = Color.white;
            MagicStoneInventory.Instance.nowPlaceObjList.Add(obj);
        }

        private void OnHold()
        {
            if (IsSelectMagicStoneAvailable()) return;

            var pos = MagicStoneExtension.BlockParsePos(Input.mousePosition, SelectedMagicStoneData.Size);
            selectedMagicStoneObj.transform.position = pos;
        }


        public void OnClickStone(MagicStoneObj stoneObj)
        {
            OnClickStone(stoneObj.magicStoneData);
        }

        public void OnClickStone(MagicStoneData stoneData)
        {
            selectedMagicStoneObj = CreateBlock();
            selectedMagicStoneObj.MagicStoneObjReset(stoneData);

            SelectMagicStoneReset();
        }

        private MagicStoneObj CreateBlock()
        {
            magicStoneObjPool ??= new ObjectPool<MagicStoneObj>(magicStonePrefab, 3, magicStoneParent);
            return magicStoneObjPool.Get();
        }

        public void DescShow()
        {
            descText.ShowDescription();
        }

        private void SelectMagicStoneReset()
        {
            selectedMagicStoneObj.Image.color = invisibleColor;
            selectedMagicStoneObj.transform.SetAsLastSibling();
            selectedMagicStoneObj.gameObject.SetActive(true);
        }

        public bool IsStoneAlreadySelected(MagicStoneData data)
        {
            if (!selectedMagicStoneObj || !selectedMagicStoneObj.gameObject.activeSelf) return false;

            return SelectedMagicStoneData == data;
        }
    }
}