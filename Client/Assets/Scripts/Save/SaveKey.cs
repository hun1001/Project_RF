using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveKey
{
    public static string GetTechTreeProgress(CountryType countryType) => countryType.ToString() + "_TechTreeProgress";
    public static string GetItemInventory(ItemType itemType) => itemType.ToString() + "_ItemInventory";
    public const string GoodsInformation = "GoodsInformation";
}
