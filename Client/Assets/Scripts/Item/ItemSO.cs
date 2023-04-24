using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(menuName = "SO/Item/ItemSO")]
    public class ItemSO : ScriptableObject
    {
        /// <summary> 해당 아이템의 ID </summary>
        public string ID;

        /// <summary> 해당 아이템의 이름 </summary>
        public string Name;

        /// <summary> 해당 아이템의 설명 </summary>
        public string Description;

        /// <summary> 해당 아이템의 희귀도 </summary>
        public int Rarity;

        /// <summary> 해당 아이템을 사기 위해 필요한 재화량 </summary>
        public int NecessaryGoods;

        /// <summary> 강화 최대치 </summary>
        public int UpgradeMax;

        /// <summary> 아이템 타입 </summary>
        public ItemType ItemType;

        /// <summary> 해당 아이템의 이미지 </summary>
        public Sprite Image;
    }
}
