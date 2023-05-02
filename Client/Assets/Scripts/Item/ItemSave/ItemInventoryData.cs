using System;
using System.Collections.Generic;

[Serializable]
public class ItemInventoryData
{
    public List<string> _itemInventoryList;

    public ItemInventoryData()
    {
        _itemInventoryList = new List<string>();
    }
}
