using System;
using System.Collections.Generic;

[Serializable]
public class ItemEquipmentData
{
    public List<string> _itemEquipmentList;

    public ItemEquipmentData()
    {
        _itemEquipmentList = new List<string>(new string[3]);
    }
}
