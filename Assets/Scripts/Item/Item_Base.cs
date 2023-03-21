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
        public void AddItem()
        {
            if (ItemManager.Instance.HaveItemList[this] == 0)
            {
                CreateItem();
            }
            else
            {
                UpgradeItem();
            }
        }

        /// <summary> 아이템을 처음 얻었을 때 실행하는 함수 </summary>
        protected abstract void CreateItem();
        /// <summary> 이미 가지고 있는 아이템을 얻었을 때 실행하는 함수 </summary>
        protected abstract void UpgradeItem();
    }
}
