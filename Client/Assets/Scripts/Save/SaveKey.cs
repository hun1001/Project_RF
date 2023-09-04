using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveKey
{
    public static string GetTechTreeProgress(CountryType countryType) => countryType.ToString() + "_TechTreeProgress";
    public static string GetShellEquipment(string tankID) => tankID + "_ShellEquipment";
    public const string GoodsInformation = "GoodsInformation";
}
