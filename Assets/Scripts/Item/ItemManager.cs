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

        /// <summary> 가지고 있는 아이템 리스트 </summary>
        public Dictionary<Item_Base, int> HaveItemList = new Dictionary<Item_Base, int>();
        /// <summary> 상점에 등장하고 있는 아이템 리스트 </summary>
        private List<Item_Base> _showingItemList = new List<Item_Base>();

        /// <summary> 가중치 랜덤 뽑기 - 아이템 </summary>
        private WeightedRandomPicker<Item_Base> _picker = new WeightedRandomPicker<Item_Base>();

        /// <summary> 아이템과 가중치 값을 넣는다 </summary>
        private void Awake()
        {
            _picker.Clear();
            HaveItemList.Clear();
            _showingItemList.Clear();

            int weight = 0;
            foreach(var item in ItemListSO.ItemList)
            {
                // y = -x + (1 + 희귀도 최대값)
                weight = -item.ItemSO.Rarity + 6;
                _picker.Add(item, weight);
            }
        }

        /// <summary> 아이템 뽑기 시작하는 함수 </summary>
        public void ItemPickUp()
        {
            _showingItemList.Clear();
            Item_Base item;
            for(int i = 0; i < 3; i++)
            {
                while (true)
                {
                    item = _picker.GetRandomPick();
                    if (_showingItemList.Contains(item) == false) break;
                }

                _showingItemList.Add(item);
                Dummy(item);
                
                if(_picker.GetItemDictReadonly().Count == _showingItemList.Count)
                {
                    break;
                }
            }
        }

        // 임시로 만든 함수
        private void Dummy(Item_Base dummy)
        {
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
