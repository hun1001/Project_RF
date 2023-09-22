using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMachineGun : BaseSubArmament
{
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
        Quaternion randomRotation = FirePoint.rotation * Quaternion.Euler(1, 1, Random.Range(-5f, 5f));

        PoolManager.Get<Shell>(_shell.ID, FirePoint.position, randomRotation).SetShell(Tank);
        PoolManager.Get("MuzzleFlash4", FirePoint.position, FirePoint.rotation);
    }
}
