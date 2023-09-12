using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

public static class SATSaveManager
{
    private static string _curretSATID = null;
    public static string SATID => _curretSATID;

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        _curretSATID = PlayerPrefs.GetString("SATID", null);
        EventManager.TriggerEvent(EventKeyword.SATReplacement);
    }

    public static void SetSAT(string id)
    {
        _curretSATID = id;
        PlayerPrefs.SetString("SATID", id);
        EventManager.TriggerEvent(EventKeyword.SATReplacement);
    }
}
