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
            _reloadingTime = (Instance as Turret).TurretSO.ReloadTime;
            if (Instance.TryGetComponent<Turret_Sound>(ComponentType.Sound, out var turretSound))
            {
                turretSound.PlaySound(SoundType.Fire);
            }
            _onFire?.Invoke();
            PoolManager.Get("FireEffect_01", (Instance as Turret).FirePoint.position, (Instance as Turret).FirePoint.rotation);
            PoolManager.Get<Shell>("APHE", (Instance as Turret).FirePoint.position, (Instance as Turret).FirePoint.rotation).SetShell(Instance, (Instance as Turret).TurretSO.Power);
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
