using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(menuName = "SO/Item/ItemListSO")]
    public class ItemListSO : ScriptableObject
    {
        /// <summary> 패시브 아이템 리스트 </summary>
        public Item_Base[] PassiveItemList;

        /// <summary> 액티브 아이템 리스트 </summary>
        public Item_Base[] ActiveItemList;
    }
}
