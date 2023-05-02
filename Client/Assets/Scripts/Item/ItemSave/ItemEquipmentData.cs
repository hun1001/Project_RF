using System;
using System.Collections.Generic;

[Serializable]
public class ItemEquipmentData
{
    public List<string> _itemEquipmentList;

    public ItemEquipmentData(ItemType type)
    {
        switch (type)
        {
            case ItemType.Passive:
                _itemEquipmentList = new List<string>(new string[3]);
                break;
            case ItemType.Active:
                _itemEquipmentList = new List<string>(new string[2]);
                break;
        }
    }
}
