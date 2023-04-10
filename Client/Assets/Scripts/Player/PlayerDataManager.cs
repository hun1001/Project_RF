using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    public void SetPlayerTankID(string id)
    {
        PlayerPrefs.SetString("PlayerTankID", id);
    }

    public string GetPlayerTankID()
    {
        return PlayerPrefs.GetString("PlayerTankID");
    }
}
