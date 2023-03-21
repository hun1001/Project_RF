using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Attack : Turret_Component
{
    public void Fire()
    {
        Pool.PoolManager.Instance.Get("APHE", Turret.FirePoint.position, Turret.FirePoint.rotation);
    }
}
