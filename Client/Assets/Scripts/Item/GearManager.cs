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
    }
}
