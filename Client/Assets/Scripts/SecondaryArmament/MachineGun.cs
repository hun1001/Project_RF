using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : SubArmament
{
    public override SubArmamentKeyActionType ActionType => SubArmamentKeyActionType.OnKeyHold;

    public override void Fire()
    {
        PoolManager.Get<Shell>("Bullet", FirePoint.position, FirePoint.rotation).SetShell(Tank, 10, 20);
        PoolManager.Get("MuzzleFlash4", FirePoint.position, FirePoint.rotation);
    }
}
