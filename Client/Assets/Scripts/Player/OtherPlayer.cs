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

        var vec = new Vector3(float.Parse(arg[0]), float.Parse(arg[1]), 0);

        if (Vector3.Distance(vec, tank.transform.position) > 50)
        {
            return;
        }

        tank.transform.position = vec;
        tank.transform.rotation = new Quaternion(0, 0, float.Parse(arg[2]), float.Parse(arg[3]));
        tank.Turret.TurretTransform.rotation = new Quaternion(0, 0, float.Parse(arg[4]), float.Parse(arg[5]));
    }

    public void Fire()
    {
        tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire();
    }

    public void UpdateHP(float hp)
    {
        tank.GetComponent<Tank_Damage>(ComponentType.Damage).SetHP(hp);
    }

    public void ReturnToPool()
    {
        Pool.PoolManager.Pool(PlayerDataManager.Instance.GetPlayerTankID(), tank.gameObject);
        Pool.PoolManager.Pool("Assets/Prefabs/OtherPlayer.prefab", this.gameObject);
    }
}
