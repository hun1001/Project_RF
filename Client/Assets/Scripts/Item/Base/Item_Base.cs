using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class Item_Base : CustomObject
    {
        /// <summary>  해당 아이템의 정보 SO </summary>
        [SerializeField]
        private ItemSO _itemSO;
        public ItemSO ItemSO { get { return _itemSO; } }
    }
}
