using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMachineGun : BaseSubArmament
{
    public override SubArmamentKeyActionType ActionType => SubArmamentKeyActionType.OnKeyHold;
    public override SATSO GetSATSO() => _satSO as MGTypeSATSO;

    private bool _canFire = true;

    public override void Fire()
    {
        if(!_canFire)
        {
            return;
        }
        StartCoroutine(FireRateCheckCoroutine());

        base.Fire();
    }

    private IEnumerator FireRateCheckCoroutine()
    {
        _canFire = false;
        yield return new WaitForSeconds(60/(GetSATSO() as MGTypeSATSO).FireRate);
        _canFire = true;
    }

    protected override void OnFire()
    {
        PoolManager.Get<Shell>("Bullet", FirePoint.position, FirePoint.rotation).SetShell(Tank, 10, 20);
        PoolManager.Get("MuzzleFlash4", FirePoint.position, FirePoint.rotation);
    }
}
