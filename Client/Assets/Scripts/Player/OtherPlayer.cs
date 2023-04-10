using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    private Tank tank = null;
    public Tank Tank => tank;

    private void Awake()
    {
        tank = Pool.PoolManager.Get<Tank>(PlayerDataManager.Instance.GetPlayerTankID()).SetTank(GroupType.Enemy);
    }

    public void ReturnToPool()
    {
        Pool.PoolManager.Pool(PlayerDataManager.Instance.GetPlayerTankID(), tank.gameObject);
        Pool.PoolManager.Pool("Assets/Prefabs/OtherPlayer.prefab", this.gameObject);
    }
}
