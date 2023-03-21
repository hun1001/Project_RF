using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Attack : Turret_Component
{
    public void Fire()
    {
        // TODO : 데미지 설정 공식에 따라 변경
        Pool.PoolManager.Get<Shell>("APHE", Turret.FirePoint.position, Turret.FirePoint.rotation).SetShell(50);
    }
}
