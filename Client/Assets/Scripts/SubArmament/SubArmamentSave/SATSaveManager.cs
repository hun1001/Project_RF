using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

public static class SATSaveManager
{
    private static string _curretSATID = null;
    public static string SATID => _curretSATID;

    public static void SetSAT(string id)
    {
        _curretSATID = id;
        EventManager.TriggerEvent(EventKeyword.SATReplacement);
    }
}
