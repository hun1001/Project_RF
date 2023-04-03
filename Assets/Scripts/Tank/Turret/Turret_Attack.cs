using System;
using UnityEngine;
using Pool;

public class Turret_Attack : Turret_Component
{
    private float _reloadingTime = 0;
    public float ReloadingTime => _reloadingTime;

    private bool _isReload = false;

    private Turret_Sound _turretSound = null;

    private Action _onFire = null;
    public void AddOnFireAction(Action action) => _onFire += action;

    private void Awake()
    {
        Turret.TryGetComponent(out _turretSound);
    }

    public void Fire()
    {
        if (_reloadingTime <= 0)
        {
            _reloadingTime = Turret.TurretData.ReloadTime;
            if (_turretSound != null)
            {
                _isReload = true;
                _turretSound.PlaySound(SoundType.Fire);
                _turretSound.PlaySound(SoundType.ShellDrop);
            }
            _onFire?.Invoke();
            PoolManager.Get<Shell>(Turret.CurrentShell.ID, Turret.FirePoint.position, Turret.FirePoint.rotation).SetShell(GetComponent<Tank>(), Turret.TurretData.Power);
            PoolManager.Get("FireEffect_01", Turret.FirePoint.position, Turret.FirePoint.rotation);
        }
    }

    private void Update()
    {
        if (_reloadingTime > 0)
        {
            _reloadingTime -= Time.deltaTime;
            if(_isReload == true && _reloadingTime < Turret.TurretSound.GetAudioClip(SoundType.Reload).length)
            {
                _isReload = false;
                _turretSound?.PlaySound(SoundType.Reload);
            }
        }
    }
}
