using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    public string ID;
    private Tank tank = null;
    public Tank Tank => tank;

    private void Awake()
    {
        tank = Pool.PoolManager.Get<Tank>(PlayerDataManager.Instance.GetPlayerTankID()).SetTank(GroupType.Enemy);
    }

    public void TransformUpdate(string args)
    {
        string[] arg = args.Split(',');
        tank.transform.position = new Vector3(float.Parse(arg[0]), float.Parse(arg[1]), float.Parse(arg[2]));
        tank.transform.rotation = new Quaternion(float.Parse(arg[3]), float.Parse(arg[4]), (float.Parse(arg[5])), float.Parse(arg[6]));
        tank.Turret.transform.rotation = new Quaternion(float.Parse(arg[7]), float.Parse(arg[8]), (float.Parse(arg[9])), float.Parse(arg[10]));
    }

    public void Fire()
    {
        tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire();
    }

    public void Damage(int damage)
    {

    }

    public void ReturnToPool()
    {
        Pool.PoolManager.Pool(PlayerDataManager.Instance.GetPlayerTankID(), tank.gameObject);
        Pool.PoolManager.Pool("Assets/Prefabs/OtherPlayer.prefab", this.gameObject);
    }
}
