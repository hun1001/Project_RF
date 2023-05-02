using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemSaveManager
{
    private static Dictionary<ItemType, ItemInventoryData> _itemInventoryDataDict = new Dictionary<ItemType, ItemInventoryData>();
    private static Dictionary<ItemType, ItemEquipmentData> _itemEquipmentDataDict = new Dictionary<ItemType, ItemEquipmentData>();

    #region Inventory
    public static ItemInventoryData GetItemInventory(ItemType itemType)
    {
        ItemInventoryData itemInventoryData = null;
        if (_itemInventoryDataDict.TryGetValue(itemType, out itemInventoryData) == false)
        {
            itemInventoryData = new ItemInventoryData();
            _itemInventoryDataDict.Add(itemType, itemInventoryData);
        }

        if (SaveManager.WasSaved(SaveKey.GetItemInventory(itemType)))
        {
            _itemInventoryDataDict[itemType] = SaveManager.Load<ItemInventoryData>(SaveKey.GetItemInventory(itemType));
        }
        else
        {
            SaveItemInventory(itemType);
        }

        return _itemInventoryDataDict[itemType];
    }

    public static void BuyItem(ItemType itemType, string itemName)
    {
        if(!_itemInventoryDataDict.ContainsKey(itemType))
        {
            _itemInventoryDataDict.Add(itemType, new ItemInventoryData());
        }

        if (!_itemInventoryDataDict[itemType]._itemInventoryList.Contains(itemName))
        {
            _itemInventoryDataDict[itemType]._itemInventoryList.Add(itemName);
        }

        SaveItemInventory(itemType);
    }

    public static void SaveItemInventory(ItemType itemType)
    {
        SaveManager.Save(SaveKey.GetItemInventory(itemType), _itemInventoryDataDict[itemType]);
    }
    #endregion

    #region Equipment
    public static ItemEquipmentData GetItemEquipment(ItemType itemType)
    {
        ItemEquipmentData itemEquipmentData = null;
        if (_itemEquipmentDataDict.TryGetValue(itemType, out itemEquipmentData) == false)
        {
            itemEquipmentData = new ItemEquipmentData();
            _itemEquipmentDataDict.Add(itemType, itemEquipmentData);
        }

        if (SaveManager.WasSaved(SaveKey.GetItemEquipment(itemType)))
        {
            _itemEquipmentDataDict[itemType] = SaveManager.Load<ItemEquipmentData>(SaveKey.GetItemEquipment(itemType));
        }
        else
        {
            SaveItemEquipment(itemType);
        }

        return _itemEquipmentDataDict[itemType];
    }

    public static void ItemEquip(ItemType itemType, int idx, string itemName)
    {
        if (!_itemEquipmentDataDict.ContainsKey(itemType))
        {
            _itemEquipmentDataDict.Add(itemType, new ItemEquipmentData());
        }

        while (_itemEquipmentDataDict[itemType]._itemEquipmentList.Count < 3)
        {
            _itemEquipmentDataDict[itemType]._itemEquipmentList.Add("");
        }

        _itemEquipmentDataDict[itemType]._itemEquipmentList[idx] = itemName;

        //if (_itemEquipmentDataDict[itemType]._itemEquipmentList[idx] == "")
        //{
        //    _itemEquipmentDataDict[itemType]._itemEquipmentList[idx] = itemName;
        //}
        //else
        //{
        //    _itemEquipmentDataDict[itemType]._itemEquipmentList[idx] = itemName;
        //}

        //if (!_itemEquipmentDataDict[itemType]._itemEquipmentList.Contains(itemName))
        //{
        //    if (_itemEquipmentDataDict[itemType]._itemEquipmentList[idx] == "")
        //    {
        //        _itemEquipmentDataDict[itemType]._itemEquipmentList[idx] = itemName;
        //    }
        //    else
        //    {
        //        _itemEquipmentDataDict[itemType]._itemEquipmentList[idx] = itemName;
        //    }
        //}

        SaveItemEquipment(itemType);
    }

    public static void SaveItemEquipment(ItemType itemType)
    {
        SaveManager.Save(SaveKey.GetItemEquipment(itemType), _itemEquipmentDataDict[itemType]);
    }
    #endregion
}
