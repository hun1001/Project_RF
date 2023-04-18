using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TechTreeDataManager
{
    private static Dictionary<CountryType, TechTreeProgress> _techTreeProgressDict = new Dictionary<CountryType, TechTreeProgress>();

    public static TechTreeProgress GetTechTreeProgress(CountryType countryType)
    {
        TechTreeProgress techTreeProgress = SaveManager.Load<TechTreeProgress>(SaveKey.GetTechTreeProgress(countryType));

        _techTreeProgressDict[countryType] = techTreeProgress;
        return _techTreeProgressDict[countryType];
    }

    public static void AddTank(CountryType countryType, string tankName)
    {
        if (!_techTreeProgressDict.ContainsKey(countryType))
        {
            _techTreeProgressDict.Add(countryType, new TechTreeProgress());
        }

        if (!_techTreeProgressDict[countryType]._tankProgressList.Contains(tankName))
        {
            _techTreeProgressDict[countryType]._tankProgressList.Add(tankName);
        }
        SaveTechTreeProgress(countryType);
    }

    public static void SaveTechTreeProgress(CountryType countryType)
    {
        SaveManager.Save(SaveKey.GetTechTreeProgress(countryType), _techTreeProgressDict[countryType]);
    }
}
