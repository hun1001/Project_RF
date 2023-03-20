using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public abstract class Item_Base : MonoBehaviour
    {
        /// <summary>  해당 아이템의 정보 SO </summary>
        public ItemSO ItemSO;

        /// <summary> 해당 아이템을 구매했을 때 실행되는 함수 </summary>
        public abstract void AddItem();
    }
}
