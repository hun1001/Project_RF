using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

public static class SATSaveManager
{
    public static string SATID => PlayerPrefs.GetString("SATID", null);

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        EventManager.TriggerEvent(EventKeyword.SATReplacement);
    }

    public static void SetSAT(string id)
    {
        PlayerPrefs.SetString("SATID", id);
        EventManager.TriggerEvent(EventKeyword.SATReplacement);
    }
}
