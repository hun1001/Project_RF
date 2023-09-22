using System.Collections.Generic;
using Addressable;
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

    public static CountryType GetCountryType(string type) => type switch
    {
        "USSR" => CountryType.USSR,
        "Germany" => CountryType.Germany,
        "USA" => CountryType.USA,
        "Britain" => CountryType.Britain,
        "France" => CountryType.France,
        _ => CountryType.None
    };

    public static List<Shell> GetUseShell(string useShells)
    {
        List<Shell> shells = new List<Shell>();

        string[] shellIds = useShells.Split('/');

        for (int i = 0; i < shellIds.Length; ++i)
        {
            Shell shell = AddressablesManager.Instance.GetResource<GameObject>(shellIds[i]).GetComponent<Shell>();
            shells.Add(shell);
        }

        return shells;
    }
}
