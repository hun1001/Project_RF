using Pool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using UnityEngine.UI;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine.EventSystems;

namespace Item
{
    public class ItemManager : MonoSingleton<ItemManager>
    {
        [SerializeField]
        private GameObject _itemObject;

        /// <summary> 아이템 리스트 SO </summary>
        public ItemListSO ItemListSO;

        [HideInInspector]
        public Transform PlayerTank = null;

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

        private void Start()
        {
            PlayerTank = GameObject.FindGameObjectWithTag("Player").transform;
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
                var itemObj = _itemObject.transform.GetChild(i).gameObject;
                itemObj.SetActive(true);
                Dummy(item, itemObj);
                
                if(_picker.GetItemDictReadonly().Count == _showingItemList.Count)
                {
                    break;
                }
            }
        }

        /// <summary> 임시로 만든 함수 - UI 만들어지면 수정할거임 </summary>
        private void Dummy(Item_Base item, GameObject obj)
        {
            var nameText = obj.transform.GetChild(0).GetComponent<Text>();
            var descriptionText = obj.transform.GetChild(1).GetComponent<Text>();
            var goldText = obj.transform.GetChild(2).GetComponent<Text>();

            nameText.text = item.ItemSO.Name;
            descriptionText.text = item.ItemSO.Description;
            goldText.text = item.ItemSO.NecessaryGoods.ToString();
            obj.GetComponent<Image>().sprite = item.ItemSO.Image;

            EventTrigger e = obj.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;

            entry.callback.AddListener((data) =>
            {
                if (GoodsManager.DecreaseGoods(GoodsType.GameGoods, item.ItemSO.NecessaryGoods) == false)
                {
                    // 재화 부족!
                    return;
                }

                if (HaveItemList.ContainsKey(item))
                {
                    var key = HaveItemList.FirstOrDefault(kvp => kvp.Key.Equals(item));
                    item = key.Key;
                    HaveItemList[item]++;
                }
                else
                {
                    item = PoolManager.Get<Item_Base>(item.name, PlayerTank);
                    HaveItemList.Add(item, 0);
                }

                if (HaveItemList[item] == item.ItemSO.UpgradeMax)
                {
                    _picker.Remove(item);
                }
                item.AddItem();
                obj.SetActive(false);
            });

            e.triggers.Clear();
            e.triggers.Add(entry);
        }

    }
}
