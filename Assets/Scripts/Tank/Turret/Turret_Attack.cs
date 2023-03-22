using System;
using UnityEngine;
using Pool;

public class Turret_Attack : Turret_Component
{
    private float _reloadingTime = 0;
    public float ReloadingTime => _reloadingTime;

    private Action _onFire = null;
    public void AddOnFireAction(Action action) => _onFire += action;

    public void Fire()
    {
        if (_reloadingTime <= 0)
        {
            _reloadingTime = Turret.TurretSO.ReloadTime;
            if (Turret.TryGetComponent<Turret_Sound>(ComponentType.Sound, out var turretSound))
            {
                turretSound.PlaySound(SoundType.Fire);
            }
            _onFire?.Invoke();
            PoolManager.Get<Shell>("APHE", Turret.FirePoint.position, Turret.FirePoint.rotation).SetShell(Turret.TurretSO.Power);
        }
    }

    private void Update()
    {
        if (_reloadingTime > 0)
        {
            _reloadingTime -= Time.deltaTime;
        }
    }
}
