using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public abstract class Item_Base : MonoBehaviour
    {
        /// <summary>  해당 아이템의 정보 SO </summary>
        [SerializeField]
        private ItemSO _itemSO;
        public ItemSO ItemSO { get { return _itemSO; } }

        private ItemType _itemType;

        private void Awake()
        {
            _itemType = _itemSO.ItemType;
        }

        /// <summary> 해당 아이템을 구매했을 때 실행되는 함수 </summary>
        public void AddItem()
        {
            if (true)//ItemManager.Instance.HaveItemList[this] == 0)
            {
                CreateItem();
            }
        }

        /// <summary> 아이템을 처음 얻었을 때 실행하는 함수 </summary>
        protected abstract void CreateItem();
    }
}
