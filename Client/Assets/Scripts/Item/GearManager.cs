using Pool;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Item
{
    public class GearManager : MonoSingleton<GearManager>
    {
        private ItemEquipmentData _passiveItemEquipmentData;
        private ItemEquipmentData _activeItemEquipmentData;

        private void Awake()
        {
            _passiveItemEquipmentData = ItemSaveManager.GetItemEquipment(ItemType.Passive);
            _activeItemEquipmentData = ItemSaveManager.GetItemEquipment(ItemType.Active);
        }

        private void Start()
        {
            foreach(var itemID in _passiveItemEquipmentData._itemEquipmentList)
            {
                if(itemID != "")
                {
                    var item = PoolManager.Get<Passive_Item>(itemID);
                    item.ItemEquip();
                }
            }

            foreach(var itemID in _activeItemEquipmentData._itemEquipmentList)
            {
                if(itemID != "")
                {

                }
            }
        }
    }
}
