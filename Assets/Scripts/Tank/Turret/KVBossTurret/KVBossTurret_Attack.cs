using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KVBossTurret_Attack : Turret_Attack
{
    public override void Fire()
    {
        if (ReloadingTime <= 0)
        {
            _reloadingTime = Turret.TurretData.ReloadTime;
            if (TurretSound != null)
            {
                _isReload = true;
                TurretSound.PlaySound(SoundType.Fire, AudioMixerType.Sfx);
                TurretSound.PlaySound(SoundType.ShellDrop, AudioMixerType.Sfx, 0.5f);
            }

            float atk = Turret.TurretData.AtkPower;
            float pen = Turret.TurretData.PenetrationPower;

            _onFire?.Invoke();

            Transform[] firePoints = new Transform[] { Turret.FirePoint, Turret.SecondFirePoint, (Turret as KVBossTurret).ThirdFirePoint };

            for (int i = 0; i < 3; ++i)
            {
                PoolManager.Get<Shell>(Turret.TurretData.Shells[i].ID, firePoints[i].position, firePoints[i].rotation).SetShell(GetComponent<Tank>(), atk, pen);
                PoolManager.Get("MuzzleFlash4", firePoints[i].position, firePoints[i].rotation);
            }
        }
    }
}
