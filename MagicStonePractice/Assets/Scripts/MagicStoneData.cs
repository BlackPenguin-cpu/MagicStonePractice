using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MagicStoneInventory", menuName = "MagicStoneData")]
public class MagicStoneData : ScriptableObject
{
    public int height;
    public int width; public Sprite stoneIcon;
    public string stoneName;

    public List<Cell> shape = new List<Cell>();
    
    [System.Serializable]
    public class Cell
    {
        public List<bool> cols = new List<bool>();

        public bool this[int index]
        {
            set
            {
                cols[index] = value;
            }
            get
            {
                return cols[index];    
            }           
        }
    }
}