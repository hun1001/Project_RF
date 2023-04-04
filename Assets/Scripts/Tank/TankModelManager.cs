using System.Collections;
using System.Collections.Generic;
using Util;
using UnityEngine;

public class TankModelManager : MonoSingleton<TankModelManager>
{
    private GameObject _tankModel = null;

    private void Awake()
    {
        _tankModel = transform.GetChild(0).gameObject;
    }

    public void ChangeTankModel(Tank tank)
    {
        if (_tankModel != null)
        {
            Destroy(_tankModel);
        }
        _tankModel = Instantiate(tank.transform.GetChild(0).gameObject, transform);
    }
}
