using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class ChurchillCrocodileBossTurret_Attack : Turret_Attack
{
    private Flame flame = null;

    private void Start()
    {
        flame = PoolManager.Get<Flame>("", transform);
    }

    public override void Fire()
    {
        base.Fire();
    }

    public void Flamethrow()
    {
        Debug.Log("ChurchillCrocodileBossTurret_Attack.Flamethrow() called");
    }
}
