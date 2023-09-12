using System;
using UnityEngine;
using Pool;
using UnityEngine.EventSystems;

public class Turret_Attack : Turret_Component
{
    private float _reloadingTime = 0;
    public float ReloadingTime => _reloadingTime;

    private float _burstReloadTime = 0f;
    public float BurstReloadTime => _burstReloadTime;

    private int _magazineSize = 0;
    public int MagazineSize => _magazineSize;

    private bool _isReload = false;
    public bool IsReload => _isReload;

    private bool _isBurst = false;

    private Turret_Sound _turretSound = null;

    private Action _onFire = null;
    public void AddOnFireAction(Action action) => _onFire += action;

    protected Tank_Move _tankMove = null;

    protected virtual void Awake()
    {
        Turret.TryGetComponent(out _turretSound);
        _tankMove = GetComponent<Tank_Move>();

        if (Turret.TurretData.IsBurst)
        {
            _magazineSize = Turret.TurretData.BurstData.MagazineSize;
            _isBurst = Turret.TurretData.IsBurst;
        }
    }

    public virtual void Fire()
    {
        if (TutorialManager.Instance.IsTutorial && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (_isBurst)
        {
            if (_reloadingTime <= 0)
            {
                if (_magazineSize > 0)
                {
                    if (_burstReloadTime <= 0)
                    {
                        if (--_magazineSize <= 0)
                        {
                            _isReload = true;
                            _reloadingTime = Turret.TurretData.ReloadTime;
                        }
                        else
                        {
                            _burstReloadTime = Turret.TurretData.BurstData.BurstReloadTime;
                        }

                        if (_turretSound != null)
                        {
                            _turretSound.PlaySound(SoundType.Fire, AudioMixerType.Sfx);
                            _turretSound.PlaySound(SoundType.ShellDrop, AudioMixerType.Sfx, 0.5f);
                        }
                        Firing();
                    }
                }
            }
        }
        else
        {
            if (_reloadingTime <= 0)
            {
                _reloadingTime = Turret.TurretData.ReloadTime;
                if (_turretSound != null)
                {
                    _isReload = true;
                    _turretSound.PlaySound(SoundType.Fire, AudioMixerType.Sfx);
                    _turretSound.PlaySound(SoundType.ShellDrop, AudioMixerType.Sfx, 0.5f);
                }
                Firing();
            }
        }
    }

    private void Update()
    {
        if (_reloadingTime > 0)
        {
            _reloadingTime -= Time.deltaTime;
            if (_isReload == true && _reloadingTime < Turret.TurretSound.GetAudioClip(SoundType.Reload).length)
            {
                _isReload = false;
                _turretSound?.PlaySound(SoundType.Reload, AudioMixerType.Sfx);
                if (_isBurst)
                    _magazineSize = Turret.TurretData.BurstData.MagazineSize;
            }
        }
        if (_isBurst)
        {
            if (_burstReloadTime > 0)
            {
                _burstReloadTime -= Time.deltaTime;
            }
        }
    }

    private void Firing()
    {
        float atk = Turret.TurretData.AtkPower;
        float pen = Turret.TurretData.PenetrationPower;

        _onFire?.Invoke();
        PoolManager.Get<Shell>(Turret.CurrentShell.ID, Turret.FirePoint.position, Turret.FirePoint.rotation).SetShell(GetComponent<Tank>(), atk, pen);
        PoolManager.Get("MuzzleFlash4", Turret.FirePoint.position, Turret.FirePoint.rotation);
    }

    protected void ResetReloadTime()
    {
        _reloadingTime = Turret.TurretData.ReloadTime;
    }
}
