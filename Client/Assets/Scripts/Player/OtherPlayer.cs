using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    private Tank tank = null;

    private void Awake()
    {
        tank = Pool.PoolManager.Get<Tank>(PlayerDataManager.Instance.GetPlayerTankID()).SetTank(GroupType.Enemy);
    }

    private void Update()
    {
        if (tank == null)
            return;


    }

    public void ReturnToPool()
    {
        Pool.PoolManager.Pool(PlayerDataManager.Instance.GetPlayerTankID(), tank.gameObject);
        Pool.PoolManager.Pool("Assets/Prefabs/OtherPlayer.prefab", this.gameObject);
    }
}
