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
        string id = PlayerPrefs.GetString("PlayerTankID");

        if (string.IsNullOrEmpty(id))
        {
            id = "BT-5";
            PlayerPrefs.SetString("PlayerTankID", id);
        }

        return id;
    }
}
