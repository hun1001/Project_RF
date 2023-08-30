using Pool;
using UnityEngine;

public class SubBattery : SubArmament
{
    public override SubArmamentKeyActionType ActionType => SubArmamentKeyActionType.OnKeyDownUp;

    public override void Aim()
    {
        Debug.Log("Aim");
    }

    public override void Fire()
    {
        PoolManager.Get<Shell>("Bullet", FirePoint.position, FirePoint.rotation).SetShell(GetComponent<Tank>(), 200, 40);
        PoolManager.Get("MuzzleFlash4", FirePoint.position, FirePoint.rotation);
    }
}
