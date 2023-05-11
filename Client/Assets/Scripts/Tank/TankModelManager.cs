using System.Collections;
using System.Collections.Generic;
using Util;
using UnityEngine;

public class TankModelManager : MonoBehaviour
{
    private GameObject _tankModel = null;
    public Tank TankModel => _tankModel.GetComponent<Tank>();

    private void Awake()
    {
        if (string.IsNullOrEmpty(PlayerDataManager.Instance.GetPlayerTankID()))
        {
            ChangeTankModel(Addressable.AddressablesManager.Instance.GetResource<GameObject>("T-44").GetComponent<Tank>());
        }
        else
        {
            ChangeTankModel(Addressable.AddressablesManager.Instance.GetResource<GameObject>(PlayerDataManager.Instance.GetPlayerTankID()).GetComponent<Tank>());
        }
    }

    public void ChangeTankModel(Tank tank)
    {
        if (_tankModel != null)
        {
            Destroy(_tankModel);
        }
        _tankModel = Instantiate(tank.transform.GetChild(0).gameObject, transform);
        PlayerDataManager.Instance.SetPlayerTankID(tank.ID);
    }
}
