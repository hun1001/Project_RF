using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemSaveManager
{
    private static Dictionary<ItemType, ItemInventoryData> _itemInventoryDict = new Dictionary<ItemType, ItemInventoryData>();

    public static ItemInventoryData GetItemInventory(ItemType itemType)
    {
        ItemInventoryData itemInventoryData = null;
        if (_itemInventoryDict.TryGetValue(itemType, out itemInventoryData) == false)
        {
            itemInventoryData = new ItemInventoryData();
            _itemInventoryDict.Add(itemType, itemInventoryData);
        }

        if (SaveManager.WasSaved(SaveKey.GetItemInventory(itemType)))
        {
            _itemInventoryDict[itemType] = SaveManager.Load<ItemInventoryData>(SaveKey.GetItemInventory(itemType));
        }
        else
        {
            SaveItemInventory(itemType);
        }

        return _itemInventoryDict[itemType];
    }

    public static void BuyItem(ItemType itemType, string itemName)
    {
        if(!_itemInventoryDict.ContainsKey(itemType))
        {
            _itemInventoryDict.Add(itemType, new ItemInventoryData());
        }

        if (!_itemInventoryDict[itemType]._itemInventoryList.Contains(itemName))
        {
            _itemInventoryDict[itemType]._itemInventoryList.Add(itemName);
        }

        SaveItemInventory(itemType);
    }

    public static void SaveItemInventory(ItemType itemType)
    {
        SaveManager.Save(SaveKey.GetItemInventory(itemType), _itemInventoryDict[itemType]);
    }
}
