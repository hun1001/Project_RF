using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SheetDataUtil
{
    public static TankType GetTankType(string type) => type switch
    {
        "Light" => TankType.Light,
        "Medium" => TankType.Medium,
        "Heavy" => TankType.Heavy,
        "Destroyer" => TankType.Destroyer,
        _ => TankType.Medium,
    };
}
