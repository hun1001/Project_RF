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
        [SerializeField]
        private ControllerCanvas _controllerCanvas;
        public ControllerCanvas ControllerCanvas => _controllerCanvas;

        private ItemEquipmentData _passiveItemEquipmentData;
        private ItemEquipmentData _activeItemEquipmentData;

        private GameObject _player;
        public GameObject Player => _player;

        private void Awake()
        {
            _passiveItemEquipmentData = ItemSaveManager.GetItemEquipment(ItemType.Passive);
            _activeItemEquipmentData = ItemSaveManager.GetItemEquipment(ItemType.Active);
        }

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");

            // Passive
            foreach(var itemID in _passiveItemEquipmentData._itemEquipmentList)
            {
                if(itemID != "")
                {
                    var item = PoolManager.Get<Passive_Item>(itemID, _player.transform);
                    item.ItemEquip();
                }
            }

            // Active
            foreach (var itemID in _activeItemEquipmentData._itemEquipmentList)
            {
                int idx = _activeItemEquipmentData._itemEquipmentList.IndexOf(itemID) + 1;
                if (itemID != "")
                {
                    var item = PoolManager.Get<Active_Item>(itemID, _player.transform);
                    item.ItemEquip(idx);
                }
                else
                {
                    _controllerCanvas.ButtonGroup.SetButton(idx, null, false);
                }
            }
        }
    }
}
