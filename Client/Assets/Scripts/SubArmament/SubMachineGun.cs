using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMachineGun : BaseSubArmament
{
    public override SubArmamentKeyActionType ActionType => SubArmamentKeyActionType.OnKeyHold;
    public override SATSO GetSATSO() => _satSO as MGTypeSATSO;

    protected override void OnFire()
    {
        PoolManager.Get<Shell>("Bullet", FirePoint.position, FirePoint.rotation).SetShell(Tank, 10, 20);
        PoolManager.Get("MuzzleFlash4", FirePoint.position, FirePoint.rotation);
    }
}
