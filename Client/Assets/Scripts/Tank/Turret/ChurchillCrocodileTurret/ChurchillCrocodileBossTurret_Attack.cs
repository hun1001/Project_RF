using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchillCrocodileBossTurret_Attack : Turret_Attack
{
    public override void Fire()
    {
        base.Fire();
    }

    public void Flamethrow()
    {
        Debug.Log("ChurchillCrocodileBossTurret_Attack.Flamethrow() called");
    }
}
