using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(menuName = "SO/Item/ItemSO")]
    public class ItemSO : ScriptableObject
    {
        public string ID;
        public string Name;
        public string Description;
        public int Rarity;
        public int NecessaryGoods;
    }
}
