using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Item
{
    public class ItemManager : MonoBehaviour
    {
        /// <summary> 아이템 리스트 SO </summary>
        public ItemListSO ItemListSO;

        /// <summary> 가중치 랜덤 뽑기 - 아이템 </summary>
        private static WeightedRandomPicker<Item_Base> _picker = new WeightedRandomPicker<Item_Base>();

        /// <summary> 아이템과 가중치 값을 넣는다 </summary>
        private void Awake()
        {
            int weight = 0;
            foreach(var item in ItemListSO.ItemList)
            {
                // y = -x + (1 + 희귀도 최대값)
                weight = -item.ItemSO.Rarity + 6;
                _picker.Add(item, weight);
            }
        }
    }
}
