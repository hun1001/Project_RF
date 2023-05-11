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
            Tank _playerTank = _player.GetComponent<Tank>();

            int itemIdx = 0;
            // Passive
            foreach (var itemID in _passiveItemEquipmentData._itemEquipmentList)
            {
                if (itemID != "")
                {
                    itemIdx = _passiveItemEquipmentData._itemEquipmentList.IndexOf(itemID) + 1;
                    if (itemIdx > _playerTank.TankSO.PassiveItemInventorySize) continue;
                    var item = PoolManager.Get<Passive_Item>(itemID, _player.transform);
                    item.ItemEquip();
                }
            }

            int activeIdx = 1;
            // Active
            foreach (var itemID in _activeItemEquipmentData._itemEquipmentList)
            {
                if (itemID != "")
                {
                    itemIdx = _activeItemEquipmentData._itemEquipmentList.IndexOf(itemID) + 1;
                    if(itemIdx > _playerTank.TankSO.ActiveItemInventorySize)
                    {
                        _controllerCanvas.ButtonGroup.SetButton(activeIdx, null, false);
                        continue;
                    }
                    var item = PoolManager.Get<Active_Item>(itemID, _player.transform);
                    item.ItemEquip(activeIdx);
                }
                else
                {
                    _controllerCanvas.ButtonGroup.SetButton(activeIdx, null, false);
                }
                activeIdx++;
            }
        }
    }
}
