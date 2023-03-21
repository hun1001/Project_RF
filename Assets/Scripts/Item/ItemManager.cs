using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Item
{
    public class ItemManager : MonoSingleton<ItemManager>
    {
        /// <summary> 아이템 리스트 SO </summary>
        public ItemListSO ItemListSO;

        public Dictionary<Item_Base, int> HaveItemList = new Dictionary<Item_Base, int>();

        /// <summary> 가중치 랜덤 뽑기 - 아이템 </summary>
        private WeightedRandomPicker<Item_Base> _picker = new WeightedRandomPicker<Item_Base>();

        /// <summary> 아이템과 가중치 값을 넣는다 </summary>
        private void Awake()
        {
            _picker.Clear();
            HaveItemList.Clear();

            int weight = 0;
            foreach(var item in ItemListSO.ItemList)
            {
                // y = -x + (1 + 희귀도 최대값)
                weight = -item.ItemSO.Rarity + 6;
                _picker.Add(item, weight);
            }
        }



        // 임시로 만든 함수
        private void Dummy()
        {
            Item_Base dummy = ItemListSO.ItemList[0];

            if (HaveItemList[dummy] == dummy.ItemSO.UpgradeMax - 1)
            {
                // 이미 최대 강화!
                return;
            }
            if (GoodsManager.DecreaseGoods(GoodsType.GameGoods, dummy.ItemSO.NecessaryGoods) == false)
            {
                // 재화 부족!
                return;
            }

            if (HaveItemList.ContainsKey(dummy))
            {
                HaveItemList[dummy]++;
            }
            else
            {
                HaveItemList.Add(dummy, 0);
            }

            if (HaveItemList[dummy] == dummy.ItemSO.UpgradeMax - 1)
            {
                _picker.Remove(dummy);
            }
            dummy.AddItem();
        }

    }
}
