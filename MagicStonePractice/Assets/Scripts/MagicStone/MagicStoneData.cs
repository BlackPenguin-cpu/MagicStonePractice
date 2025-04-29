using System.Collections.Generic;
using UnityEngine;

namespace MagicStone
{
    [CreateAssetMenu(fileName = "MagicStoneInventory", menuName = "MagicStoneData")]
    public class MagicStoneData : ScriptableObject
    {
        public Vector2Int Size => new(width,height);

        public int height;
        public int width;
        public Sprite stoneIcon;
        public string stoneName;
        public string skillName;
        public string description;

        /// <summary>
        ///  [y][x]
        /// </summary>
        public List<Cell> shape = new List<Cell>();
    
        [System.Serializable]
        public class Cell
        {
            public List<bool> cols = new List<bool>();

            public bool this[int index]
            {
                set => cols[index] = value;
                get => cols[index];
            }
        }
    }
}